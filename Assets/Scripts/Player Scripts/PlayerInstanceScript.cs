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
        isRespawn = false;
        if (instance != null){
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        if (instance != null){
            if (gameObject.transform.parent == null && !isRespawn){
                DontDestroyOnLoad(gameObject);
            }
        }
    }
    }

    

    

