using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loading_text;
    [SerializeField] private float time_between_dots = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        loading_text.text = "Loading";
        StartCoroutine(LoadingText());
    }

    IEnumerator LoadingText()
    {
        while (true)
        {
            yield return new WaitForSeconds(time_between_dots);
            if (loading_text.text == "Loading...")
            {
                loading_text.text = "Loading";
            }
            loading_text.text += ".";
        }
    }
}
