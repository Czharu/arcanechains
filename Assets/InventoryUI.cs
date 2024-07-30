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
        inventory.OnItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

  public void OpenInventory(InputAction.CallbackContext context)
    {
        if(inventoryInUse == false){
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        inventoryIsClosed = !inventoryIsClosed;
        }
    }

    public void OpenInventoryFromMerchant(){
        if((inventoryIsClosed == false) && (!inventoryInUse)){

        }
        else{
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        inventoryIsClosed = !inventoryIsClosed; 
        }
        inventoryInUse = true;
    }

    public void CloseInventory(){
        OpenInventoryFromMerchant();
        inventoryInUse = false;
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
        return inventoryIsClosed;
    }
}
