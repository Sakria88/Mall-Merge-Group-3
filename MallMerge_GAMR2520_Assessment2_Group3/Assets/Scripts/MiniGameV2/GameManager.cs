using UnityEngine;

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
            Instance = this;
        else
            Destroy(gameObject);
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
        State = pause ? GameState.Paused : GameState.Playing;
        Time.timeScale = pause ? 0f : 1f;
        UI.ShowPause(pause);
    }
}
