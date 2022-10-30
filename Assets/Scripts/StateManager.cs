using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{

    private MusicController musicController;

    private Image readyImage;
    private Image gameOverImage;

    IEnumerator LevelIntro() {

        GameManager.gamePlaying = false;
        readyImage.enabled = true;

        // display "ready?" screen here

        yield return StartCoroutine(musicController.PlayIntroMusic());

        // hide "ready?" screen here

        readyImage.enabled = false;
        GameManager.gamePlaying = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        musicController = GetComponent<MusicController>();
        readyImage = GameObject.FindWithTag("ReadyImage").GetComponent<Image>();
        gameOverImage = GameObject.FindWithTag("GameOverImage").GetComponent<Image>();

        StartCoroutine(LevelIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
