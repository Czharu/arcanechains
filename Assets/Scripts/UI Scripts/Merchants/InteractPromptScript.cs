using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
//Script to enable an interatable button for the player
public class InteractPromptScript : MonoBehaviour
{
    public GameObject interactPrompt;  // Reference to the Interact Prompt GameObject
    public string actionName = "Interact";  // The name of the action to use, settable in the Inspector

    private TextMeshPro interactButtonLabelText;  // Reference to the TextMeshPro component (world-space text)
    private InputAction interactAction;  // Reference to the player's input actions
    private bool isInRange = false;  // Track if the player is in range to interact
    private bool isInteracting = false;  // Track if an interaction is currently happening

    private IInteractable interactableScript;  // Reference to the interactable script implementing IInteractable

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);  // Ensure the interact prompt is hidden on start
        }

        // Find InteractButtonLabel in the hierarchy and get its TextMeshPro component
        var interactButtonLabelObj = interactPrompt.transform.Find("InteractButtonLabel");
        if (interactButtonLabelObj != null)
        {
            interactButtonLabelText = interactButtonLabelObj.GetComponent<TextMeshPro>();
            if (interactButtonLabelText == null)
            {
                Debug.LogError("TextMeshPro component not found on InteractButtonLabel!");
            }
        }
        else
        {
            Debug.LogError("InteractButtonLabel GameObject not found as a child of InteractPrompt!");
        }

        // Find an IInteractable component on this GameObject
        interactableScript = GetComponent<IInteractable>();
        if (interactableScript == null)
        {
            Debug.LogError("No script implementing IInteractable found on this GameObject.");
        }

        // Access the PlayerInput component from the player GameObject
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                // Use the specified action name to retrieve the correct InputAction
                interactAction = playerInput.actions[actionName];
                if (interactAction != null)
                {
                    // Listen for the action's performed event
                    interactAction.performed += OnInteractPerformed;

                    // Retrieve the first keyboard binding for the specified action
                    var interactBinding = interactAction.bindings;
                    foreach (var binding in interactBinding)
                    {
                        if (!binding.isComposite && binding.path.Contains("Keyboard"))
                        {
                            if (interactButtonLabelText != null)
                            {
                                interactButtonLabelText.text = binding.path.Replace("<Keyboard>/", "").ToUpper();
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Action '{actionName}' not found in the PlayerInput actions!");
                }
            }
            else
            {
                Debug.LogError("Player GameObject with PlayerInput component not found!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject with PlayerInput component not found!");
        }
    }

    private void OnDestroy()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;  // Unsubscribe from the action
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            if (!isInteracting && interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (isInRange && !isInteracting && interactableScript != null)
        {
            StartInteraction();  // Start interaction when the player is in range and not already interacting
        }
    }

    private void StartInteraction()
    {
        isInteracting = true;  // Set interaction state
        interactPrompt.SetActive(false);  // Hide the prompt while interacting
        interactableScript.BeginInteraction(EndInteraction);  // Start the interaction
    }

    private void EndInteraction()
    {
        isInteracting = false;  // Reset interaction state
        if (isInRange)
        {
            interactPrompt.SetActive(true);
        }
    }
}