using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistentGear : MonoBehaviour
{
    private AssistentItem myself;
    public GearProperties properties;

    [SerializeField] private Sprite[] working_sprites = new Sprite[3];
    [SerializeField] private Sprite[] broken_sprites = new Sprite[3];

    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponent<AssistentItem>();

        //if was received from other player, use fill properties using Item properties
        if (myself.received)
        {
            properties = (GearProperties) myself.properties;
        }
        else //if was created normally, fill Item properties using default properties
        {
            myself.properties = (ItemProperties) properties;
        }

        if (properties.broken)
        { 
            GetComponent<SpriteRenderer>().sprite = broken_sprites[properties.type-1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = working_sprites[properties.type-1];
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
