using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
//This script handles displaying individual items in the merchant's inventory:
public class InventoryItemUI : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;

    public void Setup(Item newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("Item is null!");
            return;
        }
        itemIcon.sprite = newItem.icon;
        itemNameText.text = newItem.itemName;
        itemPriceText.text = newItem.price.ToString();
    }
}