using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    GearRolling
}

[System.Serializable]
public struct TransitionObject
{
    public TransitionType type;
    public GameObject prefab;
}

public enum TransitionMode
{
    LoadScene,
    OnlineLoadScene,
    ExitGame
}

public enum GameState
{
    Menu,
    OnlineMenu,
    Game,
    OnlineGame
}

public class Singleton : MonoBehaviour
{
    //Singleton Instance
    public static Singleton Instance;

    [SerializeField] private TransitionObject[] transitions;
    public CharacterID player_character = CharacterID.None;

    //Awake
    void Awake()
    {
        //If there is no instance, set it to this
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
    }
    
    void Start()
    {
        //GoToScene("Menu");
    }

    void Update()
    {
        
    }

    public void CreateTransition(TransitionType type, TransitionMode mode, string scene_target = null)
    {
        GameObject transition_object = null;
        foreach (TransitionObject t in transitions)
        {
            if (t.type == type) { transition_object = Instantiate(t.prefab); }
        }
        
        if (transition_object == null) return;

        Transition transition = transition_object.GetComponent<Transition>();
        transition.mode = mode;
        transition.scene_target = scene_target;
    }

    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
