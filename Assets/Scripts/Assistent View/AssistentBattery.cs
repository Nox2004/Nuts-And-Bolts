using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistentBattery : MonoBehaviour
{
    private SpriteRenderer sr;

    private AssistentItem myself;
    public BatteryProperties properties;

    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponent<AssistentItem>();

        //if was received from other player, use fill properties using Item properties
        if (myself.received)
        {
            properties = (BatteryProperties) myself.properties;
        }
        else //if was created normally, fill Item properties using default properties
        {
            myself.properties = (ItemProperties) properties;
        }

        sr = GetComponent<SpriteRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = sprites[(int) Mathf.Floor((sprites.Length-1) * properties.charge)];
    }
}
