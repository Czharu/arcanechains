using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public Transform itemsParent;
    public GameObject inventoryUI;
    public static bool inventoryIsOpen = true;

    InventorySlot[] slots;

    Inventory inventory;
    void Start()
    {   
        inventory = Inventory.instance;
        inventory.OnItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

  public void OpenInventory(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        inventoryIsOpen = !inventoryIsOpen;
        
    }

    void UpdateUI(){
        for (int i = 0; i < slots.Length; i++){
            if( i < inventory.items.Count){
                slots[i].AddItem(inventory.items[i]);
            }
            else {
               slots[i].ClearSlot(); 
            }
        }
    }

    public bool GetInventoryState(){
        return inventoryIsOpen;
    }
}
