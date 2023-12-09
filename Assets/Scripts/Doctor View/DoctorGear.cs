using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorGear : MonoBehaviour
{
    private DoctorItem myself;
    public GearProperties properties;
    private Image image;

    [SerializeField] private Sprite[] working_sprites = new Sprite[3];
    [SerializeField] private Sprite[] broken_sprites = new Sprite[3];

    [SerializeField] private bool randomized = false;

    public bool rotate = false;

    [SerializeField] private AudioClip rolling_sound;
    private AudioSource audio_source;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();

        //if (randomized) properties.type = (int) Random.Range(1, 4);

        myself = GetComponent<DoctorItem>();
        image = GetComponent<Image>();

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
            image.sprite = broken_sprites[properties.type-1];
        }
        else
        {
            image.sprite = working_sprites[properties.type-1];
        }

        //makes the image the right size
        image.rectTransform.sizeDelta = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            if (!audio_source.isPlaying)
            {
                audio_source.loop = true;
                audio_source.clip = rolling_sound;
                audio_source.pitch = Random.Range(0.8f, 1.2f);
                audio_source.Play();
            }
            transform.Rotate(0, 0, -80 * Time.deltaTime);
        }
    }
}
