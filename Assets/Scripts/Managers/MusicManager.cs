using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public struct LevelMusic
{
    public string level_name;
    public AudioClip music;
}

public class MusicManager : MonoBehaviour
{
    public LevelMusic[] level_music;
    private AudioSource audioSource;

    void Awaken()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //set audiosource to repeat
        audioSource.loop = true;
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //On enter Room
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        foreach (LevelMusic lm in level_music)
        {
            if (lm.level_name == scene.name)
            {
                if (lm.music == null)
                {
                    audioSource.Stop();
                    return;
                }

                //if music is already playing, return
                if (audioSource.clip == lm.music) return;

                audioSource.clip = lm.music;
                audioSource.Play();
                return;
            }
        }
    }
}
