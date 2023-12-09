using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; // needed for Regex
using TMPro;
using UnityEngine.UI;

public class InputCharRestriction : MonoBehaviour
{
    //Input Component
    private TMP_InputField inputField;

    private AudioSource audioSource;
    [SerializeField] AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        //add audio source
        audioSource = gameObject.AddComponent<AudioSource>();

        inputField = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        inputField.text = Regex.Replace(inputField.text, "[^a-zA-Z0-9 ]", "");
        //makes all the input uppercase
        inputField.text = inputField.text.ToUpper();
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(sound);
    }
}
