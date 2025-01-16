using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    #region Instance

    public static Inventory instance;
    [SerializeField] public Transform basicItemPrefab;

    public delegate void ItemChangedDelegate();
    public ItemChangedDelegate OnItemChangedCallback;

    [SerializeField] private int space = 20; // Maximum inventory size
    [SerializeField] private UIDocument inventoryUIDocument;

    private VisualElement inventoryUI;
    private List<VisualElement> itemSlots = new List<VisualElement>();
    private List<Item> items = new List<Item>(); // Holds all inventory items
    private Dictionary<int, Button> itemButtons = new Dictionary<int, Button>(); // Store button references

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple Inventory instances found!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        inventoryUI = inventoryUIDocument.rootVisualElement.Q<VisualElement>("PLAYER_INVENTORY_UI");

        // Initialize UI slots based on the `space` value
        for (int i = 1; i <= space; i++)
        {
            var slot = inventoryUI.Q<VisualElement>($"INVENTORY_ITEMSLOT_{i}");
            if (slot != null)
            {
                itemSlots.Add(slot);

                // Set up button once and store reference
                var button = slot.Q<Button>("INV_ITEM_BUTTON_1");
                if (button != null)
                {
                    int index = i - 1; // Corrected index
                    itemButtons[index] = button; // Store reference
                    button.clicked += () => OnItemClicked(index); // Assign static callback
                }
            }
        }

        AddItemChangeListener(UpdateUI); // Use safe listener management
        UpdateUI(); // Perform the initial UI update
    }

    public void AddItemChangeListener(ItemChangedDelegate listener)
    {
        OnItemChangedCallback -= listener; // Remove any existing subscription
        OnItemChangedCallback += listener; // Add new subscription
    }

    public void RemoveItemChangeListener(ItemChangedDelegate listener)
    {
        OnItemChangedCallback -= listener; // Remove the listener safely
    }

    #endregion

    /// <summary>
    /// Adds an item to the inventory. If the inventory is full, it returns false.
    /// </summary>
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough inventory space.");
            return false;
        }

        items.Add(item);
        OnItemChangedCallback?.Invoke(); // Notify using the debounced handler
        return true;
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    public void Remove(Item item)
    {
        if (items.Remove(item))
        {
            OnItemChangedCallback?.Invoke(); // Notify using the debounced handler
        }
    }

    /// <summary>
    /// Updates the inventory UI to reflect the current list of items.
    /// </summary>
    private void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            var slot = itemSlots[i];
            var spriteElement = slot.Q<VisualElement>("INV_ITEM_SPRITE");

            if (i < items.Count)
            {
                var item = items[i];
                spriteElement.style.backgroundImage = new StyleBackground(item.icon);
                spriteElement.style.visibility = Visibility.Visible;
            }
            else
            {
                spriteElement.style.backgroundImage = null;
                spriteElement.style.visibility = Visibility.Hidden;
            }
        }
    }

    /// <summary>
    /// Handles the click event when an inventory item is clicked.
    /// Delegates equipping logic to the EquipmentManager.
    /// </summary>
    private void OnItemClicked(int index)
    {
        if (index < 0 || index >= items.Count) return;

        var item = items[index];
        if (item is Equipment equipment)
        {
            Debug.Log($"Equipping item: {equipment.itemName} from inventory.");
            if (EquipmentManager.instance != null)
            {
                EquipmentManager.instance.Equip(equipment);
                Remove(equipment); // Remove the item from the inventory after equipping
            }
        }
        else
        {
            Debug.Log($"Item {item.itemName} is not equippable.");
        }
    }

    /// <summary>
    /// Exposes the current list of items for external usage.
    /// </summary>
    public List<Item> GetItems()
    {
        return items;
    }

    /// <summary>
    /// Sets the inventory capacity at runtime.
    /// </summary>
    public void SetCapacity(int newSpace)
    {
        space = newSpace;
        Debug.Log($"Inventory capacity set to {newSpace}.");
    }
}
