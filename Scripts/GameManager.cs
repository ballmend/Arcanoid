using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Plane plane;
    public GameState State;
    public GameObject blöcke;
    public GameObject bälle;
    public LevelManager levelManager;
    public int currentLevel;
    public int score;
    public int highscore;
    public GameObject ballPrefab;
    public GameObject playerControllerPrefab;
    public GameObject startMenuScreen;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreTextMeshPro;
    public TextMeshProUGUI inGameScoreTextMeshPro;


    public static event Action<GameState> OnGameStateChanged;
    void Awake()
    {
        score = 0;
        highscore = 0;
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.StartScreen);
    }

    public void UpdateScore()
    {
        inGameScoreTextMeshPro.text = score.ToString();
    }

    private void Update()
    {
        if(blöcke.transform.childCount == 0 && State == GameState.InGame)   //Level Passed
        {
            UpdateGameState(GameState.LevelPassed);
        }
        if(bälle.transform.childCount == 0 && State == GameState.InGame)    //Level Failed || HP--
        {
            if (playerControllerPrefab.GetComponent<PlayerController>().playerHP < 2)
                UpdateGameState(GameState.Loose);
            else playerControllerPrefab.GetComponent<PlayerController>().lostHP();
        }
    }

    public void UpdateGameState(GameState newState)
    {

        State = newState;

        switch (newState)
        {
            case GameState.StartScreen: //Press Start to Start the first Level
                HandleStartScreen();
                break;
            case GameState.InGame:  //Start the Game with the actual Level
                HandleInGame();
                break;
            case GameState.LevelPassed: //State is reached after the Level Is finished & Switches The Screne
                currentLevel += 1;
                HandleNextLevel();
                break;
            case GameState.Victory: //All Levels Passed
                break;
            case GameState.Loose:   //Lost all HP - Level Start Again
                HandleGameOverScreen();
                break;
            default:
                Console.WriteLine("GameState nicht Initialiseirt!");
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    /*
     * Diese Methode wird vor jedem Levelstart einmal Abgerufen
     */ 
    public void HandleNextLevel()
    {
        inGameScoreTextMeshPro.gameObject.SetActive(true);
        startMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        Debug.Log("Level: " + currentLevel);                                    //print current level
        levelManager.CreateLevel(currentLevel);                                 //build map
        ballPrefab.GetComponent<BallController>().godmode = false;              //initialize ballPrefab
        playerControllerPrefab.GetComponent<PlayerController>().Initialize();   //initialize playerpaddle
        playerControllerPrefab.GetComponent<PlayerController>().updateStats();


        State = GameState.InGame;                                               //change Gamestateto ingame
    }

    private void HandleInGame()
    {
        inGameScoreTextMeshPro.gameObject.SetActive(true);
        Debug.Log("InGame"); 
        
    }

    public void HandleGameOverScreen()
    {
        inGameScoreTextMeshPro.gameObject.SetActive(false);
        for (int i = 0; i < blöcke.transform.childCount; i++)
            Destroy(blöcke.transform.GetChild(i).gameObject);
        playerControllerPrefab.GetComponent<PlayerController>().GameOver();
        scoreTextMeshPro.text = score.ToString();
        currentLevel = 0;
        score = 0;
        playerControllerPrefab.GetComponent<PlayerController>().playerHP = 3;

        gameOverScreen.SetActive(true);
    }
    public void HandleStartScreen()    // ChangeScene to Level 0/ 
    {
        Debug.Log("StartScreen");
        currentLevel = 0; //delete bälle
        score = 0;
        playerControllerPrefab.GetComponent<PlayerController>().playerHP = 3;
        startMenuScreen.SetActive(true);
        //Button to start && Set Level 0
        //on buttonpressed
    }

    public enum GameState{
        StartScreen,
        InGame,
        LevelPassed,
        Victory,
        Loose
    }
}
