using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLeft = 60f;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        timeLeft -= Time.deltaTime;
        GameManager.Instance.UI.UpdateTimer(timeLeft);

        if (timeLeft <= 0)
            GameManager.Instance.EndGame(true);
    }
}
