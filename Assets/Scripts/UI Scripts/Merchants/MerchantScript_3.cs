using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;  // Import DOTween namespace

public class MerchantShopScript_3 : MonoBehaviour, IInteractable
{
    public GameObject uiGameObject;  // Reference to the GameObject with the UIDocument component

    private VisualElement npcChatUI;
    private VisualElement buttonArea; // Reference to the parent of BUTTON_A and BUTTON_B
    private Label npcNameLabel;
    private Label merchantNpcNameLabel;
    private Label npcTextLabel;
    private Button buttonA;
    private Button buttonB;
    private float typingDuration = 2f;  // Duration for the typewriter effect

    // Text content for the interaction
    private string merchantName = "Bozo The Shopkeeper";
    private string npcText = "Welcome! Have a look at my wares!";
    private string buttonAText = "Shop";
    private string buttonBText = "Leave";

    void Start()
    {
        StartCoroutine(InitializeUIAfterDelay());
    }

    private IEnumerator InitializeUIAfterDelay()
    {
        yield return new WaitForEndOfFrame(); // Wait one frame to ensure the UI is ready
        InitializeUI();
    }

    private void InitializeUI()
    {
        if (uiGameObject == null)
        {
            Debug.LogError("UI GameObject is not assigned!");
            return;
        }

        var uiDocument = uiGameObject.GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on the assigned UI GameObject!");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Accessing elements within the container
        var container = root.Q<VisualElement>("CONTAINER");
        if (container == null)
        {
            Debug.LogError("CONTAINER element not found. Check the UXML hierarchy.");
            return;
        }

        npcChatUI = container.Q<VisualElement>("NPC_CHAT_UI");
        buttonArea = container.Q<VisualElement>("BUTTON_AREA"); // Reference the BUTTON_AREA
        npcNameLabel = container.Q<Label>("NPC_NAME");
        merchantNpcNameLabel = container.Q<Label>("MERCHANT_NPC_NAME");
        npcTextLabel = container.Q<Label>("NPC_TEXT");
        buttonA = container.Q<Button>("BUTTON_A");
        buttonB = container.Q<Button>("BUTTON_B");

        // Check for missing elements
        if (npcChatUI == null || buttonArea == null || npcNameLabel == null ||
            merchantNpcNameLabel == null || npcTextLabel == null || buttonA == null || buttonB == null)
        {
            Debug.LogError("One or more UI elements were not found. Check the UXML hierarchy and IDs.");
            return;
        }

        // Initially hide the NPC chat UI and button area
        npcChatUI.style.display = DisplayStyle.None;
        buttonArea.style.display = DisplayStyle.None;

        // Set up button actions
        buttonA.clicked += OnShopButtonPressed;
        buttonB.clicked += OnLeaveButtonPressed;
    }

    public void BeginInteraction(System.Action onComplete)
    {
        Debug.Log("Merchant interaction started!");
        ShowMerchantUI();
        StartCoroutine(InteractionCoroutine(onComplete));
    }

    private IEnumerator InteractionCoroutine(System.Action onComplete)
    {
        yield return new WaitForSeconds(20f); // Simulate interaction time
        Debug.Log("Merchant interaction ended!");
        HideMerchantUI();
        onComplete?.Invoke();
    }

    private void ShowMerchantUI()
    {
        npcNameLabel.text = merchantName;
        merchantNpcNameLabel.text = merchantName;
        npcTextLabel.text = npcText;
        buttonA.text = buttonAText;
        buttonB.text = buttonBText;

        npcChatUI.style.display = DisplayStyle.Flex;
        buttonArea.style.display = DisplayStyle.None; // Initially hide buttons

        // Use DOTween to animate the typewriter effect
        AnimateTextTypewriter(npcText);
    }

    private void HideMerchantUI()
    {
        npcChatUI.style.display = DisplayStyle.None;
        buttonArea.style.display = DisplayStyle.None; // Hide buttons
    }

    private void OnShopButtonPressed()
    {
        Debug.Log("Shop button pressed. Opening shop...");
        // Add functionality to open the shop interface here
    }

    private void OnLeaveButtonPressed()
    {
        Debug.Log("Leave button pressed. Ending interaction...");
        HideMerchantUI();
    }

    private void AnimateTextTypewriter(string text)
    {
        npcTextLabel.text = "";  // Clear existing text first

        // Animate the text being revealed using DOTween
        DOTween.To(() => npcTextLabel.text, x => npcTextLabel.text = x, text, typingDuration)
            .SetEase(Ease.Linear)  // Linear typing for consistent effect
            .OnComplete(() =>
            {
                Debug.Log("Typewriter effect completed!");
                // Show the buttons after the typewriter effect
                buttonArea.style.display = DisplayStyle.Flex;
            });
    }
}
