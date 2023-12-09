using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonLayout : MonoBehaviour
{
    [SerializeField] private CurveAnimation title_anim;

    //Get my childs
    private MenuButton[] buttons;
    private CurveAnimation animation;

    //Wave animation properties
    [SerializeField] private float wave_amplitude = 0.1f;
    [SerializeField] private float wave_frequency = 1f;
    [SerializeField] private float wave_speed = 1f;

    private float real_y;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<MenuButton>();
        animation = GetComponent<CurveAnimation>();

        real_y = transform.localPosition.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //If the animation is playing, disable the buttons
        if (animation.IsPlaying()) 
        {
            foreach (MenuButton button in buttons) button.control = false;
        }
        //If the animation is not playing, enable the buttons
        else
        {
            foreach (MenuButton button in buttons) button.control = true;
        }

        //If any of the buttons was pressed, go back
        foreach (MenuButton button in buttons)
        {
            if (button.pressed)
            {
                animation.GoBack();
                title_anim.GoBack();
                //button.pressed = false;
            }
        }

        //Wavey animation
        if (animation.IsPlaying())
        {
            real_y = transform.localPosition.y;
        }
        else 
        {
            timer += Time.deltaTime;
            var temp = transform.localPosition;
            temp.y = real_y + Mathf.Sin(timer * wave_speed * wave_frequency) * wave_amplitude;
            transform.localPosition = temp;
        }
    }
}
