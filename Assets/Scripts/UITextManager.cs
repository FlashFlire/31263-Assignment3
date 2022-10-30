using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{

    private Text scoreText;
    private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<Text>();
        timeText = GameObject.FindWithTag("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.score.ToString();

        timeText.text = System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString("mm':'ss':'ff");
        
    }
}
