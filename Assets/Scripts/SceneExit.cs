using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public string sceneToLoad;
    public string exitName;
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
        PlayerPrefs.SetString("LastExitName", exitName);
        SceneManager.LoadScene(sceneToLoad);
        }
    }
}
