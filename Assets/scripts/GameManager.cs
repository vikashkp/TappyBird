using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnGameStarted;
    public static event PlayerDelegate OnGameOverConfirmed;
    
    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameoverPage;
    public GameObject countdownPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }
    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountDownFinish += OnCountDownFinish;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;

    }
    void OnDisable()
    {
        CountdownText.OnCountDownFinish -= OnCountDownFinish;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }
    void OnCountDownFinish()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameoverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        //activated when replay button is hit
        OnGameOverConfirmed();//event sent to TapController
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
        {
        //activated when play button is hit
        SetPageState(PageState.Countdown);
    }
}