using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    public string lastExitName;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("LastExitName") == lastExitName){
            if(PlayerInstanceScript.instance != null){
            PlayerInstanceScript.instance.transform.position = transform.position;
            PlayerInstanceScript.instance.transform.eulerAngles = transform.eulerAngles;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
