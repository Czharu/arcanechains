using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    Item item;

    public void AddItem(Item newItem){
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        removeButton.image.enabled = true;
    }

    public void ClearSlot(){
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        removeButton.image.enabled = false;
    }

    public void OnRemoveButton (){
        GameObject player = GameObject.FindWithTag("Player");
        BaseItemScript.DropItem(player.transform.position, item);
        Inventory.instance.Remove(item);
    }

    public void UseItem (){
        if( item != null){
            item.Use();
        }
    }
}
