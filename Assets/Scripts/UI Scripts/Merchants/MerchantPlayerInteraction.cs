using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//To use the merchant UI in your game, you can set up interactions like this
public class MerchantPlayerInteraction : MonoBehaviour
{
    public MerchantUI merchantUI; // Reference to the MerchantUI script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Merchant"))
        {
            Merchant merchant = other.GetComponent<Merchant>();
            if (merchant != null)
            {
                merchantUI.OpenMerchantUI(merchant);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Merchant"))
        {
            merchantUI.CloseMerchantUI();
        }
    }
}
