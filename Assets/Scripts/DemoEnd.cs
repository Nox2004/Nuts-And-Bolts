using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnd : MonoBehaviour
{
    [SerializeField] private float wave_speed;
    [SerializeField] private float wave_amplitude;
    [SerializeField] private float wave_frequency;

    private bool clicked = false;
    private Vector3 initial_position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = initial_position + new Vector3(0, Mathf.Sin(Time.time * wave_frequency) * wave_amplitude, 0);

        if ((clicked == false) && (Input.GetMouseButtonDown(0)))
        {
            clicked = true;
            Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.LoadScene, "Menu");
        }
    }
}
