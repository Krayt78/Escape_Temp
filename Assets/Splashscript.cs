using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splashscript : MonoBehaviour
{
    [SerializeField] public List<Image> images;
    // liste de sons ?

    public string[] splashScreenSoundPath;

    private void Start()
    {
        if(images != null && images.Count > 0) {
            foreach (Image image in images) {
                image.canvasRenderer.SetAlpha(0.0f);
            }
            StartCoroutine(SplashscreenCoroutine());
        }
    }

    private IEnumerator SplashscreenCoroutine() {
        int index = 0;

        yield return new WaitForSeconds(1);
        // peut etre remplacer foreach par for, si liste sons
        foreach (Image image in images) {
            // apparition du sprite
            FadeIn(image);
            PlaySplashScreenSound(index);
             yield return new WaitForSeconds(2.5f);
            // fondu au noir
            FadeOut(image);
            yield return new WaitForSeconds(3.5f);
            index++;
        }
        GameController.Instance.LoadScene(1, false);
        yield return null;
    } 

    private void FadeIn(Image image) {
        image.CrossFadeAlpha(1, 2.5f, false);
    }

    private void FadeOut(Image image) {
        image.CrossFadeAlpha(0, 2.5f, false);
    }

    private void PlaySplashScreenSound(int index)
    {
        if (splashScreenSoundPath == null)
            return;
        if (index >= splashScreenSoundPath.Length)
            return;

        FMODUnity.RuntimeManager.PlayOneShot(splashScreenSoundPath[index]);
    }

}
