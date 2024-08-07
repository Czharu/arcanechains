using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Instance

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public int space = 20;

    public static Inventory instance;
    private void Awake() {
        if(instance != null){
            Debug.LogWarning("More than 1 Inventory Instance");
            return;
        }
        instance = this;
    }

    #endregion

    [SerializeField] public Transform basicItemPrefab;
    public List<Item> items = new List<Item>();

    public bool Add (Item item){
        if(!item.isDefaultItem){
            if(items.Count >= space){
                Debug.Log("Not enough inventory space.");
                return false;
            }
        items.Add(item);
        if(OnItemChangedCallback != null){
        OnItemChangedCallback.Invoke();
        }
        }
        return true;
    }

    public void Remove(Item item){
        items.Remove(item);
        if(OnItemChangedCallback != null){
            OnItemChangedCallback.Invoke();
        }
    }
}
