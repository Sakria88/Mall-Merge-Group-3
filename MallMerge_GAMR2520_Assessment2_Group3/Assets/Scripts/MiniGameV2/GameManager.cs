using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    public ScoreManager Score;
    public UIManager UI;
    public Timer Timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void EndGame(bool victory)
    {
        if (State != GameState.Playing) return;

        State = victory ? GameState.Victory : GameState.GameOver;
        Time.timeScale = 0f;

        UI.ShowGameOver(victory);
        Score.SaveHighScore();
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            if (State != GameState.Playing) return;
            State = GameState.Paused;
            Time.timeScale = 0f;
            UI.ShowPause(true);
        }
        else
        {
            if (State != GameState.Paused) return;
            State = GameState.Playing;
            Time.timeScale = 1f;
            UI.ShowPause(false);
        }
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UI = FindObjectOfType<UIManager>();
        Score = FindObjectOfType<ScoreManager>();
    }

}
