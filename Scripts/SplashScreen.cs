using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{

    public Image SplashImage; // Company splash image

    // Hide mouse cursor in intro scene
    IEnumerator Start()
    {
        SplashImage.canvasRenderer.SetAlpha(0.0f); // Hide the splash image
        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

    // Fade in function
    void FadeIn() { SplashImage.CrossFadeAlpha(1.0f, 1.5f, false); }
    // Fade out function
    void FadeOut() { SplashImage.CrossFadeAlpha(0.0f, 2.5f, false); }
}