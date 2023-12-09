using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssistentScrewDriver : MonoBehaviour
{
    private AssistentItem myself;
    [SerializeField] public ScrewDriverProperties properties;
    
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] type_sprites = new Sprite[3];

    public bool is_screwing = false;


    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponentInParent<AssistentItem>();
        sr = GetComponent<SpriteRenderer>();
        
        //if was received from other player, use fill properties using Item properties
        if (myself.received)
        {
            properties = (ScrewDriverProperties) myself.properties;
        }
        else //if was created normally, fill Item properties using default properties
        {
            myself.properties = (ItemProperties) properties;
        }
        sr.sprite = type_sprites[properties.type-1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
