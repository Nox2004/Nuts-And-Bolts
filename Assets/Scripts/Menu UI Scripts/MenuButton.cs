using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private AudioClip click_sound;
    [SerializeField] private AudioClip select_sound;
    private AudioSource audio_source;

    //Image component
    private Image image;

    //Click event
    [SerializeField] private UnityEvent on_click;
    private bool isMouseOver = false;
    public bool control;
    public bool pressed = false;

    //Target propertys
    private float current_color, target_color;
    private float current_scale, target_scale;
    [SerializeField] private float smooth_ratio = 10f;

    //When mouse is over
    public void OnMouseOver()
    {
        //play sound at highter pitch
        audio_source.pitch = 1.1f;
        if (audio_source != null) audio_source.PlayOneShot(select_sound);

        target_color = 1f;
        target_scale = 1.06f;
    }

    //When mouse is not over
    public void OnMouseExit()
    {
        //play sound at lower pitch
        audio_source.pitch = 0.9f;
        if (audio_source != null) audio_source.PlayOneShot(select_sound);

        target_color = 0.9f;
        target_scale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Adds audio source
        audio_source = gameObject.AddComponent<AudioSource>();

        //Gets image component
        image = GetComponent<Image>();

        OnMouseExit();
        current_color = target_color; current_scale = target_scale;
    }

    // Update is called once per frame
    void Update()
    {
        //If the mouse is over the image
        if ((RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition)) )
        {
            //If the mouse is clicked
            if (Input.GetMouseButtonDown(0))
            {
                //play click sound
                audio_source.pitch = 1f;
                if (audio_source != null) audio_source.PlayOneShot(click_sound);

                on_click.Invoke();
                pressed = true;
            }

            if (!isMouseOver) { isMouseOver = true; OnMouseOver(); }
        }
        else
        {
            if (isMouseOver) { isMouseOver = false; OnMouseExit(); }
        }

        //Lerp the color and scale
        current_color += (target_color-current_color) / (smooth_ratio / Time.deltaTime);
        current_scale += (target_scale-current_scale) / (smooth_ratio / Time.deltaTime);

        //Set the color and scale
        image.color = new Color(current_color,current_color,current_color, 1f);
        image.rectTransform.localScale = new Vector3(current_scale, current_scale, 1f);
    }
}
