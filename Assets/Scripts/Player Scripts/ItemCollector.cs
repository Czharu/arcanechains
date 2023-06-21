using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcollector : MonoBehaviour
{
    public Item item;
    private int gems = 0;
    [SerializeField] private Text gemsText;
    [SerializeField] private int restoredHealth = 20;

    public PlayerInteraction playerLife;
    [SerializeField] private AudioSource collectionSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Gem"))
        {
            PickUp(collision.gameObject);
            collectionSoundEffect.Play();
            gems++;
            gemsText.text = "gems: " + gems;
            playerLife.Heal(restoredHealth);
        }
    }

    void PickUp(GameObject gameObject){
        Debug.Log("Picking up" + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        if(wasPickedUp){
        Destroy(gameObject);
    }
    }
}
