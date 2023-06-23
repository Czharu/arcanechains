
using UnityEngine;

public class Interactable : MonoBehaviour
{   
        bool hasInteracted = false;

        private Rigidbody2D rb;

        void Start(){
            rb = GetComponent<Rigidbody2D>();
        }

           private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player") && hasInteracted == false){
            Interact();
            hasInteracted = true;
        }
    }

    public virtual void Interact (){
        //To be overwritten
    }
}
