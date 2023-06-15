using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPersistence : MonoBehaviour
{
    void Start()
    {
        for (int a = 0; a < Object.FindObjectsOfType<ObjectPersistence>().Length; a++){
            if(Object.FindObjectsOfType<ObjectPersistence>()[a] != this){
                if (Object.FindObjectsOfType<ObjectPersistence>()[a].name == gameObject.name){
                    Debug.Log("Destroyed:" + gameObject);
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
