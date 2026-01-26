using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public float fadeSpeed;
    public float sceneLoadDelay = 1f;

    CanvasGroup sceneFade;
    float t = 1;

    private void Start()
    {//GameObject.Find("Canvas").GetComponent<CanvasGroup>() ;
        sceneFade = FindObjectOfType<CanvasGroup>();
        sceneFade.gameObject.SetActive(true);
        sceneFade.alpha = 1;
        StartCoroutine(BlackFadeOut());
    }

    IEnumerator BlackFadeOut()
    {
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            sceneFade.alpha = t;
            yield return null;
        }
        t = 0;
    }

    IEnumerator BlackFadeIn()
    {
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            sceneFade.alpha = t;
            yield return null;
        }
        t = 1;
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(BlackFadeIn());
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        StartCoroutine(ExitingGame());
    }

    IEnumerator ExitingGame()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(BlackFadeIn());
        yield return new WaitForSeconds(sceneLoadDelay);
        Application.Quit();
    }
}
