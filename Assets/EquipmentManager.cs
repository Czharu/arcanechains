using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    private bool isEquipping = false; //recursive issue

    #region instance

    private void Awake()
    {
        instance = this;
    }

    #endregion

    Inventory inventory;

    Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;



    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public Equipment Equip(Equipment newItem)
    {
        if (isEquipping) return null; // Prevent reentrant calls
        isEquipping = true;

        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = currentEquipment[slotIndex];

        currentEquipment[slotIndex] = newItem; // Set the new item in the equipped slot

        onEquipmentChanged?.Invoke(newItem, oldItem); // Notify listeners

        Debug.Log($"Equipped: {newItem.itemName} in slot {slotIndex}");

        isEquipping = false;
        return oldItem; // Return old item so Inventory can swap it correctly
    }

    public void Unequip(int slotIndex)
    {
        if (isEquipping) return; // Prevent reentrant calls
        isEquipping = true;

        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            if (inventory.Add(oldItem))
            {
                onEquipmentChanged?.Invoke(null, oldItem);
                currentEquipment[slotIndex] = null;

                Debug.Log($"Unequipped: {oldItem.itemName} from slot {slotIndex}");
            }
            else
            {
                Debug.LogError("Failed to add unequipped item back to inventory.");
            }
        }

        isEquipping = false;
    }

}
