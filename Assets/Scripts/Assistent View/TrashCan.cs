using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : AssistentObject
{
    public override GameObject OnInteract(GameObject held_item)
    {
        if (held_item == null) return null;

        //destroy held item
        Destroy(held_item);

        return null;
    } 

    public override void OnSelected(GameObject held_item)
    {
        //if player is not holding anything, don't select
        if (held_item == null) return;

        selected = true;
    }   
}
