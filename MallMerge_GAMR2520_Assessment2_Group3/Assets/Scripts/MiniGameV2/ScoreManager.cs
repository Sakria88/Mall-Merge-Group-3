using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }

    public void Add(int value)
    {
        Score += value;
        if (Score < 0) Score = 0;
        GameManager.Instance.UI.UpdateScore(Score);
    }

    public void SaveHighScore()
    {
        int best = PlayerPrefs.GetInt("HighScore", 0);
        if (Score > best)
            PlayerPrefs.SetInt("HighScore", Score);
    }
}
