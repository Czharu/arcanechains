using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a Merchant class to handle different types of merchants and their inventories:

public class MerchantInventory : MonoBehaviour
{
    public string merchantName;
    public List<Item> inventory = new List<Item>(); // Assuming you have an Item class

    // Optionally, you can use this method to initialize the merchant's inventory
    public void InitializeMerchant(string name, List<Item> items)
    {
        merchantName = name;
        inventory = items;
    }
}
