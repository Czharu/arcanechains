using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Import TextMeshPro namespace

public class InteractableGameObject : MonoBehaviour
{
    public GameObject followerPrefab;
    public GameObject interactPrompt; // Reference to the UI element
    private TMP_Text promptText; // The TextMeshPro component of the prompt

    private bool isInteracting = false;
    private GameObject player; // Store the player GameObject

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
            var playerInput = other.GetComponent<PlayerInput>();
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
            var playerInput = other.GetComponent<PlayerInput>();
            playerInput.actions["Interact"].performed -= OnInteract;
            player = null; // Clear reference to the player GameObject

            // Hide the interact prompt
            interactPrompt.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isInteracting || player == null) return;  // Ensure only one interaction and player is valid
        isInteracting = true;

        var playerController = player.GetComponent<PlayerMovement>();
        if (playerController != null)
        {
            var follower = Instantiate(followerPrefab, player.transform.position, Quaternion.identity);
            playerController.AddFollower(follower);
        }

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
}