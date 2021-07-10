using UnityEngine;

public class Commands : MonoBehaviour
{
    TransitionController _transitionController;

    void Start() => _transitionController = FindObjectOfType(typeof(TransitionController)) as TransitionController;

    public void titleScreen() => _transitionController.startFade(0);
    public void startGame() => _transitionController.startFade(1);
    public void quitGame() => Application.Quit();
}
