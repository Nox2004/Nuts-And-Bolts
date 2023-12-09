using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurveAnimation : MonoBehaviour
{
    //Serialized animation curve 
    [SerializeField] private AnimationCurve curve_in;
    [SerializeField] private AnimationCurve curve_out;
    [SerializeField] private bool reverse = false;

    //Serialized duration of the animation
    [SerializeField] private float duration_in = 1f;
    [SerializeField] private float duration_out = 0f;

    //Get the Rect Transform component
    private RectTransform rect_transform;

    //Target positions
    [SerializeField] private Vector3 start_position;
    [SerializeField] private Vector3 target_position;
    [SerializeField] private Vector3 target_position_out = new Vector3(0, 0, 0);

    //Serialized boolean to check if the animation is playing
    private bool is_playing = true;
    public bool goBack = false;

    private float timer = 0f;
    
    public bool IsPlaying()
    {
        return is_playing || goBack;
    }

    public void GoBack()
    {
        goBack = true;
        //Resets timer if goback animation isnt reversed
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get the rect transform component
        rect_transform = GetComponent<RectTransform>();

        //Set the target position
        rect_transform.localPosition = start_position;
    }

    // Update is called once per frame
    void Update()
    {
        //In animation
        if (is_playing)
        {
            timer += Time.deltaTime;
            if (timer < duration_in)
            {
                float curve_value = curve_in.Evaluate(timer / duration_in); //Get the current curve value
                rect_transform.localPosition = Vector3.LerpUnclamped(start_position, target_position, curve_value); //Set the new position
            }
            else
            {
                rect_transform.localPosition = target_position; //Set the new position   
                is_playing = false; //Set the animation to false
                
                if (!reverse) timer = 0f; //Resets timer if animation isnt reversed
            }
        }

        //Go back animation
        if (goBack)
        {
            //Reversed in animation
            if (reverse)
            {
                timer -= Time.deltaTime;
                if (timer < 0f)
                {
                    rect_transform.localPosition = start_position; //Set the new position
                    goBack = false; //Set the animation to false
                }
                else
                {
                    float curve_value = curve_in.Evaluate(timer / duration_in); //Get the current curve value
                    rect_transform.localPosition = Vector3.LerpUnclamped(start_position, target_position, curve_value); //Set the new position
                }
            }
            else //Normal out animation
            {
                timer += Time.deltaTime;

                if (timer < duration_out)
                {
                    float curve_value = curve_out.Evaluate(timer / duration_out); //Get the current curve value
                    rect_transform.localPosition = Vector3.LerpUnclamped(target_position, target_position_out, curve_value); //Set the new position
                }
                else
                {
                    rect_transform.localPosition = target_position_out; //Set the new position
                    goBack = false; //Set the animation to false
                }
            }
        }
    }
}
