using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorNotes : MonoBehaviour
{
    private float closed_x;
    private PolygonCollider2D collider;
    [SerializeField] private float opened_position;

    float target_x;

    private UnityEngine.UI.Image image;
    [SerializeField] float smoth;

    [SerializeField] private AudioClip open_sound;
    private AudioSource audio_source;

    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        image = GetComponent<UnityEngine.UI.Image>();
        closed_x = transform.localPosition.x;

        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = open_sound;
        audio_source.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        target_x = closed_x;
        //checks if mouse is inside the collider

        // Get the mouse position
        Vector3 mousePosition = Input.mousePosition;

        // Check if the mouse is colliding with the Item
        if (collider.OverlapPoint(mousePosition))
        {
            if (closed_x-transform.localPosition.x < 10f && !audio_source.isPlaying)
            {
                //Play Sound
                audio_source.pitch = Random.Range(0.9f, 1.1f);
                audio_source.Play();
            }
            target_x = opened_position;
        }

        //if (RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition)) target_x = opened_position;
        
        var aux = transform.localPosition;
        aux.x += (target_x-aux.x) / (smoth / Time.deltaTime);
        transform.localPosition = aux;
    }
}
