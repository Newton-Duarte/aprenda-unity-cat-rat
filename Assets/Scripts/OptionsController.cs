using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    [Header("Options Panel")]
    [SerializeField] GameObject optionsPanel;

    [Header("Volume Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;
    [SerializeField] Slider alertSlider;

    [Header("Audio Sources")]
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource fxSource;
    [SerializeField] internal AudioSource alertSource;

    [Header("Audio Clips")]
    [SerializeField] internal AudioClip titleClip;
    [SerializeField] internal AudioClip startClip;
    [SerializeField] internal AudioClip gameplayClip;

    [Header("UI Config.")]
    [SerializeField] Text musicVolumeText;
    [SerializeField] Text fxVolumeText;
    [SerializeField] Text alertVolumeText;

    // PlayerPrefs Constants
    string firstPlaythrough = "FirstPlaytrough";
    string musicVolume = "MusicVolume";
    string fxVolume = "FXVolume";
    string alertVolume = "AlertVolume";

    // Start is called before the first frame update
    void Start()
    {
        initializePlayerPrefs();
        StartCoroutine(changeMusic(titleClip));
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) { toggleOptions(); }
    }

    void initializePlayerPrefs()
    {
        if (PlayerPrefs.GetInt(firstPlaythrough) == 0)
        {
            PlayerPrefs.SetInt(firstPlaythrough, 1);
            PlayerPrefs.SetFloat(musicVolume, 1f);
            PlayerPrefs.SetFloat(fxVolume, 0.9f);
            PlayerPrefs.SetFloat(alertVolume, 0.8f);
        }

        float musicVol = PlayerPrefs.GetFloat(musicVolume); 
        float fxVol = PlayerPrefs.GetFloat(fxVolume); 
        float alertVol = PlayerPrefs.GetFloat(alertVolume);

        musicSlider.value = musicVol;
        fxSlider.value = fxVol;
        alertSlider.value = alertVol;

        musicSource.volume = musicVol;
        fxSource.volume = fxVol;
        alertSource.volume = alertVol;

        musicVolumeText.text = Mathf.Round(musicVol * 100).ToString();
        fxVolumeText.text = Mathf.Round(fxVol * 100).ToString();
        alertVolumeText.text = Mathf.Round(alertVol * 100).ToString();
    }

    void toggleOptions() => optionsPanel.SetActive(!optionsPanel.activeInHierarchy);

    public void changeMusicVolume()
    {
        float volume = musicSlider.value;

        musicSource.volume = volume;
        musicVolumeText.text = Mathf.Round(volume * 100).ToString();

        PlayerPrefs.SetFloat(musicVolume, volume);
    }

    public void changeFXVolume()
    {
        float volume = fxSlider.value;

        fxSource.volume = volume;
        fxVolumeText.text = Mathf.Round(volume * 100).ToString();

        PlayerPrefs.SetFloat(fxVolume, volume);
    }

    public void changeAlertVolume()
    {
        float volume = alertSlider.value;

        alertSource.volume = volume;
        alertVolumeText.text = Mathf.Round(volume * 100).ToString();

        PlayerPrefs.SetFloat(alertVolume, volume);
    }

    internal IEnumerator changeMusic(AudioClip clip, bool loop = true)
    {
        float currentVolume = musicSource.volume;

        for (float v = currentVolume; v > 0; v -= 0.01f)
        {
            musicSource.volume = v;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.pitch = 1;
        musicSource.Play();

        for (float v = 0; v < currentVolume; v += 0.01f)
        {
            musicSource.volume = v;
            yield return new WaitForEndOfFrame();
        }
    }

    public void quitGame() => Application.Quit();
}
