using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text timerText;
    public Text scoreText;
    [SerializeField] private AudioManager audioManager;

    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public Text titleText;
    public Text finalScoreText;
    
    public void UpdateTimer(float time)
    {
        timerText.text = Mathf.Ceil(time).ToString();
        if (time < 10) timerText.color = Color.red;
    }

    public void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
    }

    public void ShowPause(bool show)
    {
        //
    }

    public void ShowGameOver(bool victory)
    {
        gameOverPanel.SetActive(true);
        // Play appropriate sound
        if (audioManager != null)
    {
        if (victory) audioManager.PlayWinner();
        else audioManager.PlayExplosion();
    }

        titleText.text = victory ? "CONGRATULATIONS" : "GAME OVER";
        titleText.color = victory ? Color.green : Color.red;
        finalScoreText.text = "Score: " + GameManager.Instance.Score.Score;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
