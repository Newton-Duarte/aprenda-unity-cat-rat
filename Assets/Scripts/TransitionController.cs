using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField] Animator animator;
    int sceneTogo;

    internal void startFade(int sceneIndex)
    {
        sceneTogo = sceneIndex;
        animator.SetTrigger("FadeOut");
    }

    internal void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneTogo);
    }
}
