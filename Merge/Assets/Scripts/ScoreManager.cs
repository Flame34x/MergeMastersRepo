using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    public static PlayerPrefs highscore;
    private static float _highscore;
    
    public TMP_Text scoreText;
    public TMP_Text highscoreText;


    public TMP_Text GO_score;
    public TMP_Text GO_highscore;

    private static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();

                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("ScoreManager");
                    _instance = managerObject.AddComponent<ScoreManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _highscore = PlayerPrefs.GetFloat("Highscore", 0);
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        highscoreText.text = "Highscore: " + _highscore.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void GameOver()
    {
        if (score > _highscore)
        {
            _highscore = score;
            PlayerPrefs.SetFloat("Highscore", _highscore);
        }

        GO_score.text = "Score: " + score.ToString();
        GO_highscore.text = "Highscore: " + _highscore.ToString();

    }
    
}
