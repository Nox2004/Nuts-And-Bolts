using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGear : MonoBehaviour
{
    //Angular speed of the gear
    [SerializeField] private float angular_speed = 1f;
    //Image component
    private Image image;

    void Start()
    {
        //Get the image component
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotates the image by the angular speed
        image.transform.Rotate(0, 0, angular_speed * Time.deltaTime);
    }
}
