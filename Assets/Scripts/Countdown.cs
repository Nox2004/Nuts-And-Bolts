using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float time_in_seconds;
    [SerializeField] private TextMeshProUGUI text;

    private bool countdown_ended;
    private LevelManager level_manager;

    [SerializeField] private AudioClip time_is_ending_sound;
    private AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = time_is_ending_sound;
        audio_source.loop = true;

        countdown_ended = false;
        level_manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown_ended) return;

        time_in_seconds -= Time.deltaTime;
        int minutes = (int)time_in_seconds / 60;
        int seconds = (int)time_in_seconds % 60;

        text.text = minutes.ToString("00") + " " + seconds.ToString("00");

        if (time_in_seconds <= 60f && !audio_source.isPlaying)
        {
            audio_source.Play();
        }

        if (time_in_seconds <= 0) 
        {
            text.text = "00:00";
            countdown_ended = true;

            //finish connection
            if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            Singleton.Instance.CreateTransition(TransitionType.GearRolling, TransitionMode.LoadScene, "DemoEndLose");
            level_manager.ended = true;
        }
    }
}
