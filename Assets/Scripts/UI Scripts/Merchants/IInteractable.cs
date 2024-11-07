using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simple Scr0ipt that enables the interact prompt to link to scripts on the same game object
public interface IInteractable
{
    void BeginInteraction(System.Action onComplete);
}
