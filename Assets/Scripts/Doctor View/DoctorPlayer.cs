using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorPlayer : MonoBehaviour
{
    public DoctorItem selected_item;
    private DoctorItem[] items;

    [SerializeField] private AudioClip send_item_sound;
    [SerializeField] private AudioClip get_item_sound;
    [SerializeField] private AudioClip drop_item_sound;
    private AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        items = FindObjectsOfType<DoctorItem>();
    }

    // Update is called once per frame
    void Update()
    {
        items = FindObjectsOfType<DoctorItem>();
        
        if (Input.GetMouseButtonDown(0))
        {
            foreach (DoctorItem obj in items)
            {
                //if mouse over image
                if (RectTransformUtility.RectangleContainsScreenPoint(obj.gameObject.GetComponent<UnityEngine.UI.Image>().rectTransform, Input.mousePosition))
                {
                    if (selected_item != null)
                    {
                        selected_item.being_drag = false;
                    }
                    selected_item = obj.GetComponent<DoctorItem>();
                    selected_item.being_drag = true;

                    //Play Sound
                    audio_source.pitch = Random.Range(0.9f, 1.1f);
                    audio_source.PlayOneShot(get_item_sound);

                    break;
                }
            }
        }

        if (selected_item != null)
        {
            if (!selected_item.being_drag)
            {
                selected_item = null;

                //Play Sound
                audio_source.pitch = Random.Range(0.9f, 1.1f);
                audio_source.PlayOneShot(drop_item_sound);
            }
        }
    }
}
