using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearRollingTransition : Transition
{
    [SerializeField] private float angular_speed = 1f;
    [SerializeField] private float horizontal_speed = 1f;

    [SerializeField] private Transform gear_transform;

    [SerializeField] private AudioClip audio_clip;
    private AudioSource audio_source;
    private float pitch;

    void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = audio_clip;
        audio_source.loop = true;
        audio_source.Play();

        pitch = Random.Range(0.9f, 1.1f);
        audio_source.pitch = pitch;
        
    }

    void Update()
    {
        gear_transform.Rotate(0, 0, angular_speed * Time.deltaTime);

        
        var temp = gear_transform.localPosition;
        temp.x+=horizontal_speed * Time.deltaTime;
            
        
        if ((temp.x >= 0) && (!transited))
        {
            temp.x = 0; OnTransition();
        }

        //if exited screen (x is greater than half screens width plus half of the gear's width) 
        if (temp.x > (Screen.width/2 + gear_transform.GetComponent<RectTransform>().rect.width/2))
        {
            Destroy(gameObject);
        }

        gear_transform.localPosition = temp;
    }
}
