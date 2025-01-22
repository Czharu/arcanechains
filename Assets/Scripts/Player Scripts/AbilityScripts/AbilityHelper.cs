using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHelper : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartAbilityCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
