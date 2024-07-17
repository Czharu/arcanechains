using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Import TextMeshPro namespace
//Script to make the interact button appear and trigger
public class InteractPromptController : MonoBehaviour
{
    public GameObject interactPrompt; // Reference to the UI element
    private TMP_Text promptText; // The TextMeshPro component of the prompt
    public TalkingOverlayController talkingOverlayController; // Reference to the TalkingOverlayController
    private bool isInteracting = false;
    private GameObject player; // Store the player GameObject
    private PlayerInput playerInput;

    private void Start()
    {
        promptText = interactPrompt.GetComponentInChildren<TMP_Text>();
        interactPrompt.SetActive(false); // Ensure the prompt is hidden initially
    }

    private void Update()
    {
        if (interactPrompt.activeSelf)
        {
            // Update the position of the interact prompt to be above the interactable object
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
            interactPrompt.transform.position = screenPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject; // Store reference to the player GameObject
            playerInput = other.GetComponent<PlayerInput>();
            playerInput.actions["Interact"].performed += OnInteract;

            // Display the interact prompt
            interactPrompt.SetActive(true);
            SetInteractButtonText(playerInput);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInput.actions["Interact"].performed -= OnInteract;
            player = null; // Clear reference to the player GameObject

            // Hide the interact prompt
            interactPrompt.SetActive(false);
            talkingOverlayController.HideOverlay(); // Hide the talking overlay when the player leaves
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isInteracting || player == null) return;  // Ensure only one interaction and player is valid
        isInteracting = true;

        // Show the talking overlay
        ShowTalkingOverlay();

        // Hide the interact prompt when talking begins
        interactPrompt.SetActive(false);

        // Remove the interaction action handler
        playerInput.actions["Interact"].performed -= OnInteract;

        isInteracting = false;  // Reset interaction flag after handling
    }

    private void SetInteractButtonText(PlayerInput playerInput)
    {
        var interactAction = playerInput.actions["Interact"];
        var binding = interactAction.bindings[0]; // Assuming the first binding is the primary one
        string buttonText = ExtractButtonName(binding.effectivePath).ToUpper();
        promptText.text = $"{buttonText}";
    }

    private string ExtractButtonName(string effectivePath)
    {
        // Extract the last part of the path, which is the actual button name
        var parts = effectivePath.Split('/');
        return parts[parts.Length - 1];
    }

    private void ShowTalkingOverlay()
    {
        // Example dialog data
        string characterName = "Croc";
        string dialog = "Good meet. I have more precious!";
        string action1Text = "Shop";
        UnityEngine.Events.UnityAction action1 = () => OpenMerchantShop();
        string action2Text = "Nothing";
        UnityEngine.Events.UnityAction action2 = () => talkingOverlayController.HideOverlay();

        talkingOverlayController.ShowOverlay(characterName, dialog, action1Text, action1, action2Text, action2);
    }

    private void OpenMerchantShop()
    {
        // Implement the logic to open the merchant's shop
        Debug.Log("Opening shop for Croc");
        talkingOverlayController.HideOverlay();
        Merchant merchant = FindObjectOfType<Merchant>();
        if (merchant != null)
        {
            FindObjectOfType<MerchantUI>().OpenMerchantUI(merchant);
        }
        else
        {
            Debug.LogError("Merchant not found in the scene.");
        }
    }

    public void ShowInteractPrompt()
    {
        if (player != null)
        {
            interactPrompt.SetActive(true);
            playerInput.actions["Interact"].performed += OnInteract;
        }
    }

    public void HideInteractPrompt()
    {
        if (player != null)
        {
            interactPrompt.SetActive(false);
            playerInput.actions["Interact"].performed -= OnInteract;
        }
    }
}