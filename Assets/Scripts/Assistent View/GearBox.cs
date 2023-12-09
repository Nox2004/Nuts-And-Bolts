using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBox : AssistentObject
{
    [SerializeField] private int type = 0;
    [SerializeField] private GameObject gear_prefab;
   

    public override GameObject OnInteract(GameObject holding_item)
    {
        if (holding_item != null) return holding_item;

        Debug.Log("Instantiate gear " + type);
        GameObject gear = Instantiate(gear_prefab, transform.position, Quaternion.identity);
        gear.GetComponent<AssistentGear>().properties.type = type;

        return gear;
    }    
}
