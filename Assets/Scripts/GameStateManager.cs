using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        PAUSE,
        BUILDING,
        SIMULATING
    }

    public GameState Game;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Game = GameState.BUILDING;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildVessel()
    {
        Game = GameState.SIMULATING;
        DontDestroyOnLoad(GameObject.FindObjectOfType<Pod>());
        SceneManager.LoadScene("SandBox");
        GameObject.FindObjectOfType<Pod>().Build();
    }

    public void LoadEditor()
    {
        Game = GameState.BUILDING;
        
        foreach(Transform obj in FindObjectsOfType<Transform>())
            Destroy(obj.gameObject);

        SceneManager.LoadScene("BuildEditor");
    }
}
