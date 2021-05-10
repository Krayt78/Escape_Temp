using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splashscript : MonoBehaviour
{
    [SerializeField] public List<Image> images;
    // liste de sons ?
    
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
        // peut etre remplacer foreach par for, si liste sons
        foreach (Image image in images) {
            yield return new WaitForSeconds(4f);
            // apparition du sprite
            FadeIn(image);
            yield return new WaitForSeconds(5f);
            // fondu au noir
            FadeOut(image);
        }
        GameController.Instance.LoadScene(1);
        yield return null;
    } 

    private void FadeIn(Image image) {
        image.CrossFadeAlpha(1, 2.5f, false);
    }

    private void FadeOut(Image image) {
        image.CrossFadeAlpha(0, 2.5f, false);
    }

}
