using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public float fadeSpeed =2f;
     public float fadeOutMultiplier = 2f;
     public float preFadeDelay = 0.01f;
    public float sceneLoadDelay = 0.1f;
   
    [SerializeField] private CanvasGroup sceneFade;
    private float t = 1f;
    private Coroutine activeRoutine;

    private void Awake()
    {
        if (sceneFade == null)
            sceneFade = GetComponentInChildren<CanvasGroup>(true);
    }

    private void Start()
    {
        if (sceneFade == null)
        {
            Debug.LogError("SceneManagerScript: No CanvasGroup assigned/found for scene fade.");
            return;
        }

        sceneFade.gameObject.SetActive(true);
        t = 1f;
        sceneFade.alpha = 1f;

        activeRoutine = StartCoroutine(BlackFadeOut());
    }

    IEnumerator BlackFadeOut()
    {
        float speed = fadeSpeed * Mathf.Max(0.01f, fadeOutMultiplier);

        while (t > 0f)
        {
            if (sceneFade == null) yield break;

            t -= Time.unscaledDeltaTime * speed;
            if (t < 0f) t = 0f;

            sceneFade.alpha = t;
            yield return null;
        }
    }

    IEnumerator BlackFadeIn()
    {
         while (t < 1f)
        {
            if (sceneFade == null) yield break;

            t += Time.unscaledDeltaTime * fadeSpeed;
            if (t > 1f) t = 1f;

            sceneFade.alpha = t;
            yield return null;
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
       if (preFadeDelay > 0f)
            yield return new WaitForSecondsRealtime(preFadeDelay);

        if (sceneFade != null)
            sceneFade.gameObject.SetActive(true);

        yield return StartCoroutine(BlackFadeIn());

        if (sceneLoadDelay > 0f)
            yield return new WaitForSecondsRealtime(sceneLoadDelay);

        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
       if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(ExitingGame());
    }

    IEnumerator ExitingGame()
    {
       if (preFadeDelay > 0f)
            yield return new WaitForSecondsRealtime(preFadeDelay);

        if (sceneFade != null)
            sceneFade.gameObject.SetActive(true);

        yield return StartCoroutine(BlackFadeIn());

        if (sceneLoadDelay > 0f)
            yield return new WaitForSecondsRealtime(sceneLoadDelay);

        Application.Quit();
    }
}
