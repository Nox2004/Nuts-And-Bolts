using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTitle : MonoBehaviour
{
    [SerializeField] private float angle_wave_amplitude = 8f;
    [SerializeField] private float wave_frequency = 1f;
    [SerializeField] private float wave_speed = 1f;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {    
        var temp = transform.localEulerAngles;
        temp.z = Mathf.Sin(Time.time * wave_speed * wave_frequency) * angle_wave_amplitude;
        transform.localEulerAngles = temp;
    }
}
