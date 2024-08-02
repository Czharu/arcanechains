using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemScript : MonoBehaviour
{
    public static BaseItemScript SpawnItem(Vector2 position, Item item){
        Transform transform = Instantiate(Inventory.instance.basicItemPrefab, position, Quaternion.identity);

        BaseItemScript droppableItem = transform.GetComponent<BaseItemScript>();
        droppableItem.SetItem(item);

        return droppableItem;
    }

    public static BaseItemScript DropItem(Vector2 dropPosition, Item item){
        
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    // Ensure there's enough horizontal force
        randomDirection.x = randomDirection.x < 0 ? Mathf.Min(randomDirection.x, -0.5f) : Mathf.Max(randomDirection.x, 0.5f);

        BaseItemScript itemDrop = SpawnItem(dropPosition + randomDirection, item);

        float forceStrength = Random.Range(.5f, .5f);

        itemDrop.GetComponent<Rigidbody2D>().AddForce(randomDirection * forceStrength, ForceMode2D.Impulse);

        return itemDrop;
    }
    private Item item;
    private SpriteRenderer spriteRenderer;
    private ItemPickup itemPickupScript;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemPickupScript = GetComponent<ItemPickup>();
    }
     public void SetItem (Item item) {
        this.item = item;
        spriteRenderer.sprite = item.icon;
        itemPickupScript.item = item;
    }
}
