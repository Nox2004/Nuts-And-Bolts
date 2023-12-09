using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorScrewDriver : MonoBehaviour
{
    private DoctorItem myself;
    [SerializeField] public ScrewDriverProperties properties;
    
    private Image image;
    private Sprite normal_sprite;
    [SerializeField] private Sprite[] type_sprites = new Sprite[3];
    [SerializeField] private Sprite screwing_sprite;

    public bool is_screwing = false;

    [SerializeField] private AudioClip screw_sound;
    private AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = screw_sound;
        audio_source.loop = true;
        audio_source.volume = 2.5f;

        myself = GetComponentInParent<DoctorItem>();
        image = GetComponent<Image>();
        
        //if was received from other player, use fill properties using Item properties
        if (myself.received)
        {
            properties = (ScrewDriverProperties) myself.properties;
        }
        else //if was created normally, fill Item properties using default properties
        {
            myself.properties = (ItemProperties) properties;
        }

        normal_sprite = type_sprites[properties.type-1];
    }

    // Update is called once per frame
    void Update()
    {
        if (myself.being_drag)
        {
            //if mouse is over screw, start screwing it


        }
        
        image.sprite = (myself.being_drag) ? screwing_sprite : normal_sprite;

        //rotate when screwing
        if (is_screwing)
        {
            if (!audio_source.isPlaying)
            {
                audio_source.Play();
            }
            transform.Rotate(0, 0, -300 * Time.deltaTime);
        }
        else 
        {
            if (audio_source.isPlaying)
            {
                audio_source.Stop();
            }
            transform.rotation = Quaternion.identity;
        }

        //change images size to match sprites
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<Image>().sprite.rect.width, GetComponent<Image>().sprite.rect.height);
        is_screwing = false;
    }
}
