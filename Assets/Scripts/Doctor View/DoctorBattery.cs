using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorBattery : MonoBehaviour
{
    private Image image;

    private DoctorItem myself;
    public BatteryProperties properties;

    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponent<DoctorItem>();

        //if was received from other player, use fill properties using Item properties
        if (myself.received)
        {
            properties = (BatteryProperties) myself.properties;
        }
        else //if was created normally, fill Item properties using default properties
        {
            myself.properties = (ItemProperties) properties;
        }

        image = GetComponent<Image>();    
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = sprites[(int) Mathf.Floor((sprites.Length-1) * properties.charge)];
    }
}
