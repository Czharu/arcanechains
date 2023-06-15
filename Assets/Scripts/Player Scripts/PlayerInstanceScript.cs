using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstanceScript : MonoBehaviour
{
    public static PlayerInstanceScript instance = null;
    public static bool isRespawn = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!isRespawn){
        if (instance != null){
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    else {
        Destroy(gameObject);
        isRespawn = false;
        }
    }

    

    
}
