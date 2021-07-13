using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour
{
    OptionsController _optionsController;
    TransitionController _transitionController;

    bool isVerified;

    // Start is called before the first frame update
    void Start()
    {
        _optionsController = FindObjectOfType(typeof(OptionsController)) as OptionsController;
        _transitionController = FindObjectOfType(typeof(TransitionController)) as TransitionController;
        _optionsController.musicSource.loop = false;
    }

    void Update()
    {
        if (!isVerified && !_optionsController.musicSource.isPlaying)
        {
            isVerified = true;
            _optionsController.StartCoroutine(_optionsController.changeMusic(_optionsController.gameplayClip));
            _transitionController.startFade(3);
        }
    }
}
