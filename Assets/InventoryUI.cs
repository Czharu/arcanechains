using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public static bool inventoryIsClosed = true;

    public bool inventoryOpenFromMerchant = false;

    private bool inventoryInUse = false;

    InventorySlot[] slots;

    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.AddItemChangeListener(UpdateUI); // Use the safe method for subscription
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Optionally, if you need to remove it later:
    void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.RemoveItemChangeListener(UpdateUI);
        }
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (!inventoryInUse)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            inventoryIsClosed = !inventoryIsClosed;
        }
    }

    public void OpenInventoryFromMerchant()
    {
        if (inventoryIsClosed && !inventoryInUse)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            inventoryIsClosed = !inventoryIsClosed;
        }
        inventoryInUse = true;
    }

    public void CloseInventory()
    {
        OpenInventoryFromMerchant();
        inventoryInUse = false;
    }

    void UpdateUI()
    {
        var items = inventory.GetItems(); // Use the getter to access items
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].AddItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public bool GetInventoryState()
    {
        return inventoryIsClosed;
    }
}
