using UnityEngine;
using UnityEngine.SceneManagement;

public  class LevelFade : MonoBehaviour
{

    public Animator animator;

    public void FadeIn() {
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut() {
        animator.SetTrigger("FadeOut");
    }

}