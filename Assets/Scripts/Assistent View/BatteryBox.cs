using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBox : AssistentObject
{
    [SerializeField] private GameObject battery_prefab;
   

    public override GameObject OnInteract(GameObject holding_item)
    {
        if (holding_item != null) return holding_item;

        Debug.Log("Instantiate new battery");
        GameObject battery = Instantiate(battery_prefab, transform.position, Quaternion.identity);
        battery.GetComponent<AssistentBattery>().properties.charge = 0;

        return battery;
    }    
}
