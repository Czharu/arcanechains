using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class ClassManagerScript : MonoBehaviour
{
    public static ClassManagerScript instance;
    void Awake(){
        instance = this;
    }
    private CharacterStats characterStats;

    public BasicClass spearman;

    public List<BasicClass> classList;
    public List<BasicClass> activeClasses;

    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        classList = new List<BasicClass>();
        activeClasses = new List<BasicClass>();
        //ClassList();
        spearman = new SpearmanClass(characterStats);
        classList.Add(spearman);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainClass(){

    }

    public void AddSubclass(){
    }

    public void CheckRequirements(){
        foreach(BasicClass classtype in classList){
            if(classtype.CheckRequirements() == true && activeClasses.Contains(classtype) == false){
                Debug.Log("Adding Passive Call Check Req...");
                classtype.ActivatePassive();
                activeClasses.Add(classtype);
            }
            else{
                if(activeClasses.Contains(classtype)){
                    Debug.Log("Removing Passive Log");
                    activeClasses.Remove(classtype);
                    classtype.DeactivatePassive();
                }
            }

        }
    }

/*
    private void ClassList(){ // enter new classes here
        spearman = new SpearmanClass();
        classList.Add(spearman);
    }
    */

}
