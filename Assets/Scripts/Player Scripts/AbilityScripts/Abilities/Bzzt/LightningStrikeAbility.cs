using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class LightningStrikeAbility : Ability
{

    [SerializeField] private GameObject lightningBolt;
    private GameObject lightningBoltInst;
    
    public override void Activate(GameObject parent)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float topOfScreenY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y;

        Vector2 spawnPosition = new Vector2(worldPosition.x, topOfScreenY);

        lightningBoltInst = Instantiate(lightningBolt, spawnPosition, Quaternion.identity);
        lightningBoltInst.transform.rotation = Quaternion.Euler(0, 0, 270); // SPRITE HAS TO GO SIDEWAYS
    }

    
}
