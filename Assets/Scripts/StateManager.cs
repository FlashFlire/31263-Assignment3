using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{

    private MusicController musicController;
    private UITextManager textManager;

    [SerializeField]
    private PacStudentMovementController pacController;

    [SerializeField]
    private List<GhostController> ghostControllers;

    [SerializeField]
    private CherryController cherryController;

    [SerializeField]
    private Tweener tweener;

    private Image readyImage;
    private Image gameOverImage;

    IEnumerator LevelIntro() {

        GameManager.gamePlaying = false;
        readyImage.enabled = true;
        Time.timeScale = 0;

        yield return StartCoroutine(musicController.PlayIntroMusic());

        readyImage.enabled = false;
        GameManager.gamePlaying = true;
        Time.timeScale = 1;

    }


    IEnumerator GameOver() {
        GameManager.gamePlaying = false;
        Time.timeScale = 0;
        musicController.audioSource.Stop();

        readyImage.enabled = false;
        gameOverImage.enabled = true;
        yield return new WaitForSecondsRealtime(3f);

        // update scores if necessary...

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    IEnumerator DefeatEffects() {

        GameManager.gamePlaying = false;
        Time.timeScale = 0;

        musicController.audioSource.Stop();
        musicController.audioSource.PlayOneShot(musicController.audioClips[4]);

        // play pacman defeat animation
        pacController.PlayDefeatAnimation();

        yield return new WaitWhile(() => musicController.audioSource.isPlaying);


        GameManager.lives -= 1;

        if (GameManager.lives == 0) {
            // display game over screen, then boot player out to main menu
            yield return GameOver();
        } else {

            textManager.UpdateLives();

            tweener.Reset();

            pacController.Reset();
            for (int i = 0; i < 4; i++) {
                ghostControllers[i].Reset();
            }
            cherryController.Reset();

            StartCoroutine(LevelIntro());

        }



    }


    public void QuitLevel() {
        StopAllCoroutines();
        StartCoroutine(GameOver());
    }


    public void PacStudentDefeated() {
        StartCoroutine(DefeatEffects());
    }


    // Start is called before the first frame update
    void Start()
    {

        GameManager.score = 0;
        GameManager.lives = 3;

        musicController = GetComponent<MusicController>();
        textManager = GetComponent<UITextManager>();
        readyImage = GameObject.FindWithTag("ReadyImage").GetComponent<Image>();
        gameOverImage = GameObject.FindWithTag("GameOverImage").GetComponent<Image>();
        gameOverImage.enabled = false;

        StartCoroutine(LevelIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gamePlaying) {

            if (GameManager.ghostScareTime > 0) {
                GameManager.ghostScareTime -= Time.deltaTime;
            }

            musicController.audioState = (GameManager.ghostsDefeated > 0) ? 3 : ((GameManager.ghostScareTime > 0) ? 2 : 0);

        }
    }
}
