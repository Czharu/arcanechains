using System.Collections;
using UnityEngine;

public class MerchantShopScript_3 : MonoBehaviour, IInteractable
{
    public void BeginInteraction(System.Action onComplete)
    {
        Debug.Log("Merchant interaction started!");
        
        // Simulate an interaction (e.g., showing a UI)
        StartCoroutine(InteractionCoroutine(onComplete));
    }

    private IEnumerator InteractionCoroutine(System.Action onComplete)
    {
        yield return new WaitForSeconds(2f); // Simulate interaction time
        Debug.Log("Merchant interaction ended!");

        // Invoke the callback to signal that the interaction is complete
        onComplete?.Invoke();
    }
}
