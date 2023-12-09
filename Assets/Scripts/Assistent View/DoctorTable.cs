using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorTable : AssistentObject
{
    [SerializeField] private LevelManager level_manager;

    [SerializeField] private AudioClip send_item_sound;
    private AudioSource audio_source;

    void Start()
    {
        base.Start();
        audio_source = gameObject.AddComponent<AudioSource>();
    }
    public override GameObject OnInteract(GameObject holding_item)
    {
        if (holding_item == null) return null;

        AssistentItem item = holding_item.GetComponent<AssistentItem>();

        //give item to doctor
        if (level_manager.SendItem(new SendItemEventData(item.doctor_counterpart.name,item.properties)))
        {
            //Destroy Item
            Destroy(holding_item);

            //play sound
            audio_source.PlayOneShot(send_item_sound);
            return null;   
        }

        return holding_item;
    } 

    public override void OnSelected(GameObject held_item)
    {
        //if player is not holding anything, don't select
        if (held_item == null) return;

        selected = true;
    }   
}
