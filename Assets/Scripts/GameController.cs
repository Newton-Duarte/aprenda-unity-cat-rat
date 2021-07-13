using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal enum gameState {
    gameplay, gamewin, gameover
}

public class GameController : MonoBehaviour
{
    int levelTime = 90;
    int score;

    TransitionController _transitionController;
    OptionsController _optionsController;

    [Header("Gameplay Config.")]
    internal gameState currentState;
    [SerializeField] internal Transform ratHole;

    [Header("Boundary Config.")]
    [SerializeField] internal Transform leftPlayerBoundary;
    [SerializeField] internal Transform rightPlayerBoundary;

    [Header("UI Config.")]
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;

    [Header("Audio Clips")]
    [SerializeField] AudioClip fxCollect;
    [SerializeField] AudioClip fxEnemy;
    [SerializeField] AudioClip fxLevelFailed;
    [SerializeField] AudioClip fxLevelSuccess;

    [Header("Enemy Config.")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] int[] enemySpawnTimes;
    bool isSpawned;
    internal Transform targetSpawn;

    // Start is called before the first frame update
    void Start()
    {
        _transitionController = FindObjectOfType(typeof(TransitionController)) as TransitionController;
        _optionsController = FindObjectOfType(typeof(OptionsController)) as OptionsController;
        timeText.text = levelTime.ToString();
        StartCoroutine(levelCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawned && currentState == gameState.gameplay)
        {
            isSpawned = true;
            StartCoroutine(spawnEnemy());
        }
    }

    void LateUpdate()
    {
        if (levelTime == 30)
        {
            _optionsController.musicSource.pitch = 1.3f;
        }
    }

    public void setIsSpawned(bool value)
    {
        isSpawned = value;
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(enemySpawnTimes[0], enemySpawnTimes[1]));

        int rand = Random.Range(0, 100);

        if (rand < 50)
        {
            _optionsController.alertSource.panStereo = -1;
            _optionsController.alertSource.PlayOneShot(fxEnemy);
            yield return new WaitForSeconds(2f);

            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(leftSpawn.position.x, enemy.transform.position.y, enemy.transform.position.z);
            targetSpawn = rightSpawn;
        }
        else
        {
            _optionsController.alertSource.panStereo = 1;
            _optionsController.alertSource.PlayOneShot(fxEnemy);
            yield return new WaitForSeconds(1f);

            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(rightSpawn.position.x, enemy.transform.position.y, enemy.transform.position.z);
            targetSpawn = leftSpawn;
        }
    }

    IEnumerator levelCountdown()
    {
        yield return new WaitForSeconds(1);
        levelTime--;
        timeText.text = levelTime.ToString();

        if (levelTime == 0)
        {
            StopCoroutine(levelCountdown());
            gameOver();
            yield break;
        }

        StartCoroutine(levelCountdown());
    }

    public void gameOver()
    {
        Debug.LogError("Game Over!");
        currentState = gameState.gameover;
        _optionsController.StartCoroutine(_optionsController.changeMusic(fxLevelFailed, false));
        StartCoroutine(loadSceneWithDelay(5, 1.5f));
    }

    public void setScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        _optionsController.fxSource.PlayOneShot(fxCollect);
    }

    internal void gameWin()
    {
        Debug.Log("Game Win!");
        currentState = gameState.gamewin;
        _optionsController.StartCoroutine(_optionsController.changeMusic(fxLevelSuccess, false));
        StartCoroutine(loadSceneWithDelay(4, 2f));
    }

    internal IEnumerator loadSceneWithDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        _transitionController.startFade(sceneIndex);
    }
}
