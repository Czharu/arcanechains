using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class MerchantShopScript_3 : MonoBehaviour, IInteractable
{
    public GameObject uiGameObject;  // Reference to the GameObject with the UIDocument component
    public InteractPromptScript interactPromptScript;  // Reference to the InteractPromptScript
    public MerchantInventory merchantInventory;  // Reference to the MerchantInventory script

    private VisualElement npcChatUI;
    private VisualElement buttonArea;
    private VisualElement merchantShopWindowUI;
    private Button closeShopButton;
    private Label npcNameLabel;
    private Label merchantNpcNameLabel;
    private Label npcTextLabel;
    private Button buttonA;
    private Button buttonB;
    private float typingDuration = 2f;

    private List<VisualElement> itemSlots = new List<VisualElement>(); // To hold item slot elements

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
        yield return new WaitForEndOfFrame();
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

        var container = root.Q<VisualElement>("CONTAINER");
        if (container == null)
        {
            Debug.LogError("CONTAINER element not found. Check the UXML hierarchy.");
            return;
        }

        npcChatUI = ValidateUIElement<VisualElement>(container, "NPC_CHAT_UI");
        buttonArea = ValidateUIElement<VisualElement>(container, "BUTTON_AREA");
        merchantShopWindowUI = ValidateUIElement<VisualElement>(container, "MERCHANT_SHOP_UI");
        closeShopButton = ValidateUIElement<Button>(container, "MERCHANT_SHOP_UI_CLOSE_BUTTON");
        npcNameLabel = ValidateUIElement<Label>(container, "NPC_NAME");
        merchantNpcNameLabel = ValidateUIElement<Label>(container, "MERCHANT_NPC_NAME");
        npcTextLabel = ValidateUIElement<Label>(container, "NPC_TEXT");
        buttonA = ValidateUIElement<Button>(container, "BUTTON_A");
        buttonB = ValidateUIElement<Button>(container, "BUTTON_B");

        // Find item slots
        for (int i = 1; i <= 6; i++)
        {
            var itemSlot = container.Q<VisualElement>($"MERCHANT_ITEM_SLOT_{i}");
            if (itemSlot != null)
            {
                itemSlots.Add(itemSlot);
            }
        }

        if (itemSlots.Count == 0)
        {
            Debug.LogError("No item slots found in the UI!");
        }

        // Initially hide UI elements
        HideMerchantUI();

        // Set up button actions
        buttonA.clicked += OnShopButtonPressed;
        buttonB.clicked += OnLeaveButtonPressed;
        closeShopButton.clicked += OnCloseShopButtonPressed;
    }

    private T ValidateUIElement<T>(VisualElement parent, string name) where T : VisualElement
    {
        var element = parent.Q<T>(name);
        if (element == null)
        {
            Debug.LogError($"UI element '{name}' of type '{typeof(T).Name}' not found. Check the UXML hierarchy.");
        }
        return element;
    }

    public void BeginInteraction(System.Action onComplete)
    {
        Debug.Log("Merchant interaction started!");
        if (interactPromptScript != null)
        {
            interactPromptScript.DisableInteraction();
        }
        ShowMerchantUI();
    }

    private void ShowMerchantUI()
    {
        if (npcNameLabel != null) npcNameLabel.text = merchantName;
        if (merchantNpcNameLabel != null) merchantNpcNameLabel.text = merchantName;
        if (npcTextLabel != null) npcTextLabel.text = npcText;
        if (buttonA != null) buttonA.text = buttonAText;
        if (buttonB != null) buttonB.text = buttonBText;

        if (npcChatUI != null) npcChatUI.style.display = DisplayStyle.Flex;
        if (buttonArea != null) buttonArea.style.display = DisplayStyle.None;

        AnimateTextTypewriter(npcText);
    }

    private void HideMerchantUI()
    {
        if (npcChatUI != null) npcChatUI.style.display = DisplayStyle.None;
        if (buttonArea != null) buttonArea.style.display = DisplayStyle.None;
        if (merchantShopWindowUI != null) merchantShopWindowUI.style.display = DisplayStyle.None;

        if (interactPromptScript != null)
        {
            interactPromptScript.EnableInteraction();
        }
    }

    private void OnShopButtonPressed()
    {
        Debug.Log("Shop button pressed. Transitioning to shop window...");
        if (npcChatUI != null) npcChatUI.style.display = DisplayStyle.None;
        if (merchantShopWindowUI != null) merchantShopWindowUI.style.display = DisplayStyle.Flex;

        PopulateMerchantInventory();
    }

    private void OnLeaveButtonPressed()
    {
        Debug.Log("Leave button pressed. Ending interaction...");
        HideMerchantUI();
    }

    private void OnCloseShopButtonPressed()
    {
        Debug.Log("Close button pressed. Closing shop UI and ending interaction...");
        HideMerchantUI();
    }

    private void PopulateMerchantInventory()
    {
        if (merchantInventory == null || merchantInventory.inventory == null)
        {
            Debug.LogError("MerchantInventory or its items are not set!");
            return;
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i < merchantInventory.inventory.Count)
            {
                var item = merchantInventory.inventory[i];

                // Populate slot with item data
                var itemSlot = itemSlots[i];
                var spriteElement = itemSlot.Q<VisualElement>("MERCHANT_ITEM_SPRITE");
                var nameElement = itemSlot.Q<Label>("ITEM_NAME");
                var priceElement = itemSlot.Q<Label>("ITEM_PRICE");

                if (spriteElement != null) spriteElement.style.backgroundImage = new StyleBackground(item.icon);
                if (nameElement != null) nameElement.text = item.itemName;
                if (priceElement != null) priceElement.text = $"${item.price}";
            }
            else
            {
                // Clear empty slots
                var itemSlot = itemSlots[i];
                var spriteElement = itemSlot.Q<VisualElement>("MERCHANT_ITEM_SPRITE");
                var nameElement = itemSlot.Q<Label>("ITEM_NAME");
                var priceElement = itemSlot.Q<Label>("ITEM_PRICE");

                if (spriteElement != null) spriteElement.style.backgroundImage = null;
                if (nameElement != null) nameElement.text = "";
                if (priceElement != null) priceElement.text = "";
            }
        }
    }

    private void AnimateTextTypewriter(string text)
    {
        if (npcTextLabel == null) return;

        npcTextLabel.text = "";

        DOTween.To(() => npcTextLabel.text, x => npcTextLabel.text = x, text, typingDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("Typewriter effect completed!");
                if (buttonArea != null) buttonArea.style.display = DisplayStyle.Flex;
            });
    }
}
