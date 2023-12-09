using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeStation : AssistentObject
{
    [SerializeField] private float charge_time = 15f;
    [SerializeField] private Vector3 battery_offset;

    private Animator animator;
    private bool charging = false;
    private AssistentBattery battery = null;
    
    //Sound stuff
    private AudioSource put_battery_audio_source;
    private AudioSource charge_audio_source;
    [SerializeField] private AudioClip charging_sound;
    [SerializeField] private AudioClip charged_sound;
    [SerializeField] private AudioClip put_battery_sound;

    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        put_battery_audio_source = gameObject.AddComponent<AudioSource>();
        charge_audio_source = gameObject.AddComponent<AudioSource>();
        charge_audio_source.loop = true;
    }

    void Update()
    {
        base.Update();

        if (battery != null)
        {
            if (charging)
            {
                animator.SetInteger("Stage", 1);
                battery.properties.charge += Time.deltaTime / charge_time;
                
                if (battery.properties.charge >= 1)
                {
                    charging = false;
                    battery.properties.charge = 1;
                    charge_audio_source.clip = charged_sound;
                    charge_audio_source.Play();
                }
            }
            else
            {
                animator.SetInteger("Stage", 2);
            }
        }
        else
        {
            animator.SetInteger("Stage", 0);
        }	
    }

    public override void OnSelected(GameObject held_item)
    {
        if (battery == null)
        {
            if (held_item == null) return;
            
            AssistentBattery held_battery = held_item.GetComponent<AssistentBattery>();

            //if player is not holding a battery, don't select
            if (held_battery == null) return;
        }
        else
        {
            //if player is holding something, don't select
            if (held_item != null) return;
        }

        selected = true;
    }

    public override GameObject OnInteract(GameObject held_item)
    {
        if (battery == null)
        {
            AssistentBattery held_battery = held_item.GetComponent<AssistentBattery>();

            //if player is not holding a battery, don't interact
            if (held_battery == null) return held_item;

            //put the battery in charger

            battery = held_battery;
            charging = true;

            //set battery rotation and position
            battery.transform.position = transform.position + battery_offset;
            battery.transform.rotation = Quaternion.identity;

            //disable battery collision
            battery.GetComponent<Collider2D>().enabled = false;
            
            //play sound
            put_battery_audio_source.PlayOneShot(put_battery_sound);
            charge_audio_source.PlayOneShot(charging_sound);

            return null;
        }
        else
        {
            //if player is holding something, don't select
            if (held_item != null) return held_item;

            //take the battery from charger

            var _battery = battery;
            charging = false;
            battery = null;

            _battery.GetComponent<Collider2D>().enabled = true;

            //stop sound
            charge_audio_source.Stop();

            return _battery.gameObject;
        }

        return null;
    }    
}
