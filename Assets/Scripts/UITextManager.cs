using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{

    private Text scoreText;
    private Text timeText;
    private Text scareTimeText;
    private Text livesText;


    public void UpdateLives() {
        livesText.text = GameManager.lives.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<Text>();
        timeText = GameObject.FindWithTag("Time").GetComponent<Text>();
        scareTimeText = GameObject.FindWithTag("ScareTime").GetComponent<Text>();
        livesText = GameObject.FindWithTag("Lives").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.score.ToString();

        timeText.text = System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString("mm':'ss':'ff");

        if (GameManager.ghostScareTime > 0) {
            scareTimeText.enabled = true;
            scareTimeText.text = ((int)GameManager.ghostScareTime + 1).ToString(); // +1 to counteract the "floor" effect of converting to int
        } else {
            scareTimeText.enabled = false;
        }
        
    }
}
