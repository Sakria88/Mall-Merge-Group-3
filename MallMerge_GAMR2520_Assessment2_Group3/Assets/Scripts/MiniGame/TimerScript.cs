using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float targetTime = 60f;

    public GameObject gameOverPanel;
    public Text titleText;
    public Text scoreText;

    private bool gameEnded = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        targetTime -= Time.deltaTime;

        Text timerText = GameObject.Find("Timer_Text").GetComponent<Text>();
        timerText.text = Mathf.Ceil(targetTime).ToString();

        if (targetTime < 10f)
            timerText.color = Color.red;

        if (targetTime <= 0f)
        {
            EndGame(GameEndType.Win);
        }
    }

    public void EndGame(GameEndType endType)
    {
        if (gameEnded) return;
        gameEnded = true;

        // UI
        gameOverPanel.SetActive(true);

        if (endType == GameEndType.Win)
        {
            titleText.text = "CONGRATULATIONS";
            titleText.color = Color.green;
        }
        else
        {
            titleText.text = "GAME OVER";
            titleText.color = Color.red;
        }

        BasketControllerScript basket = FindObjectOfType<BasketControllerScript>();
        if (basket != null)
        {
            scoreText.text = basket.energyCounter + "Energy!!";
        }

        // Pausar TODO
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public enum GameEndType
    {
        Win,
        GameOver
    }

}
