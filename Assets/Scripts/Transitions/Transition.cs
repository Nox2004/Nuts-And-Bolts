using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    protected bool transited = false;
    public TransitionMode mode;
    public string scene_target;

    protected void Awake()
    {    
        DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
    }
    protected void OnTransition()
    {
        switch (mode)
        {
            case TransitionMode.LoadScene:
                Singleton.Instance.GoToScene(scene_target);
            break;
            case TransitionMode.OnlineLoadScene:
                NetworkManager.Instance.GoToLevel(scene_target);
            break;
            case TransitionMode.ExitGame:
                Singleton.Instance.EndGame();
            break;
            default:
            break;
        }
        transited = true;
    }
}
