using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
//a script to manage the merchant UI and bind it to the current merchant:

public class MerchantUI : MonoBehaviour
{
    public TMP_Text merchantNameText;
    public Transform inventoryPanel; // The panel where the merchant's inventory items will be displayed
    public GameObject inventoryItemPrefab; // Prefab for displaying items
    public Button closeButton; // Reference to the close button
    public InteractPromptController interactPromptController;

    private Merchant currentMerchant;

    void Start()
    {
        closeButton.onClick.AddListener(CloseMerchantUI);
    }

    public void OpenMerchantUI(Merchant merchant)
    {
        if (merchant == null)
        {
            Debug.LogError("Merchant is null.");
            return;
        }

        currentMerchant = merchant;
        merchantNameText.text = currentMerchant.merchantName;
        UpdateInventory();
        gameObject.SetActive(true); // Show the UI
        interactPromptController.HideInteractPrompt(); // Hide the interact prompt when the Merchant UI is shown
    }

    public void CloseMerchantUI()
    {
        gameObject.SetActive(false); // Hide the UI
        interactPromptController.ShowInteractPrompt(); // Re-enable the interact prompt when the Merchant UI is hidden
    }

    private void UpdateInventory()
    {
        // Clear existing items
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Populate inventory items
        foreach (Item item in currentMerchant.inventory)
        {
            GameObject itemObject = Instantiate(inventoryItemPrefab, inventoryPanel);
            InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();
            itemUI.Setup(item); // Assuming you have a method to setup the UI with item details
        }
    }

    // Example method to handle button click event for a specific item
    public void OnItemClicked(Item item)
    {
        Debug.Log("Item clicked: " + item.itemName);
        // Add your logic here to handle item click, e.g., buying the item
    }
}