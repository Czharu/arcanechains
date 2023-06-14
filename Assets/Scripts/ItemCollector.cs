using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcollector : MonoBehaviour
{
    private int gems = 0;
    [SerializeField] private Text gemsText;
    [SerializeField] private int restoredHealth = 20;

    public PlayerLife playerLife;
    [SerializeField] private AudioSource collectionSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            collectionSoundEffect.Play();
            gems++;
            gemsText.text = "gems: " + gems;
            playerLife.Heal(restoredHealth);
        }
    }
}
