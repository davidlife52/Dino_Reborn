using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public PlayerMovement player;

    public RectTransform mPanelGameOver;
    public Text mTxtGameOver;

    public Text scoreText;
    int score = 0;

    public Text timer;
    public float timeRemaining = 30;
    public bool timerIsRunning = false;

    public Text lifeCount;
    int lifes = 2;

    int newLifePoints = 500;
    int newLifesCounter = 0;

    bool end = false;
    bool TimeForNext = false;

    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        lifeCount.text = "x" + lifes.ToString();
        timerIsRunning = true;
        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (TimeForNext && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                DisplayTime(timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
        else if(Mathf.FloorToInt(timeRemaining) <= 0)
        {
            CancelInvoke();
        }
    }
    public void AddLife()
    {
        lifes++;
        lifeCount.text = "x" + lifes.ToString();
    }

    public void LooseLife()
    {
        if (lifes >= 0)
        {
            if (lifes <= 0)
            {
                end = true;
                GameEnd();
            }
            else
            {
                lifes -= 1;
                lifeCount.text = "x" + lifes.ToString();
            }
            
        }
    }

    void DisplayTime(float remainingTime)
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
        if (score / newLifePoints > newLifesCounter)
        {
            newLifesCounter++;
            AddLife();
        }
    }

    void AddAlieansClearedPoints()
    {
        if (Mathf.FloorToInt(timeRemaining) > 0)
        {
            timeRemaining -= 1;
            DisplayTime(timeRemaining);
            AddPoints(100);
        }
    }

    void AddBlocksClearedPoints()
    {
        if (Mathf.FloorToInt(timeRemaining) > 0)
        {
            timeRemaining -= 1;
            DisplayTime(timeRemaining);
            AddPoints(1000);
        }
    }

    public void OnLevelClearedNoAliens()
    {
        timerIsRunning = false;
        InvokeRepeating("AddAlieansClearedPoints", 0, 0.05f);
        GameEnd();
    }

    public void OnLevelClearedNoBlocks()
    {
        timerIsRunning = false;
        InvokeRepeating("AddBlocksClearedPoints", 0, 0.05f);
        GameEnd();
    }

    public void GameEnd()
    {
        player.Stop = true;
        mPanelGameOver.gameObject.SetActive(true);
        if (end) { 
            mTxtGameOver.text = "YOU LOSE";
        }
        else
        {
            mTxtGameOver.text = "NEXT LEVEL";
            TimeForNext = true;
        }
    }
}
