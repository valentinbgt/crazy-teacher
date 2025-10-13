using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Header("Config Initiale")]
    [SerializeField] public int startingLives = 2;
    [SerializeField] public int difficulty = 1;
    [SerializeField] private int lives;
    public int Lives { get; private set; }

    [Header("Timer Partagé")]
    [SerializeField] private bool timerRunning;
    public bool TimerRunning { get; private set; }
    [SerializeField] private float remainingTime;
    public float RemainingTime { get; private set; }
    [SerializeField] private float duration;
    public float Duration { get; private set; }
    private Coroutine _timerCo;
    public event Action OnTimerEnded;
    public event Action<float> OnTimerTick;

    [Header("Leaderboard")]
    [SerializeField] private int roundsPlayed;
    public int RoundsPlayed { get; private set; }

    [Header("UI")]
    [SerializeField] public TimerUI timerUI;
    [SerializeField] private LivesUI livesUI;

    //ACTION À EFFECTUER À LA FIN D'UN MINI-JEU
    public event Action OnMinigameWon;
    public event Action OnMinigameFailed;
    private ScenesLoader scenesLoader;

    private string currentGame = "";

    // public void getRandomGame()
    // {
    //     string[] scenesList = Directory.GetFiles("Assets/Scenes/MiniGames", "*.unity");
    //     for (int i = 0; i < scenesList.Length; i++)
    //     {
    //         scenesList[i] = Path.GetFileNameWithoutExtension(scenesList[i]);
    //     }
    //     System.Random rand = new System.Random();
    //     int index = rand.Next(scenesList.Length);
    //     if (currentGame == scenesList[index])
    //     {
    //         index = (index + 1) % scenesList.Length;
    //     }
    //     if (Input.GetButton("P1_B6"))
    //     {   
    //         scenesLoader.LoadMiniGame(scenesList[index]);
    //     }
    //     currentGame = scenesList[index];
    // }

    void Awake()
    {
        scenesLoader = GetComponent<ScenesLoader>();
        Lives = startingLives;
        livesUI?.SetLives(Lives);
        RoundsPlayed = 0;
        // getRandomGame();
    }

    public void AddRound()
    {
        RoundsPlayed++;
        //ON POURRA RAJOUTER D'AUTRES ACTIONS AU CHANGEMENT DE ROUND ICI
    }

    //GESTION DES VIES
    public void LoseLife()
    {
        Lives--;
        livesUI?.SetLives(Lives);
    }

    public void ResetLives()
    {
        Lives = startingLives;
        livesUI?.SetLives(Lives);
    }

    //GESTION DU TIMER
    public void StartTimer(float seconds)
    {
        StopTimer(); //pour être sur qu'on en a pas deux qui tournent
        Duration = Mathf.Max(0f, seconds);
        RemainingTime = Duration;
        TimerRunning = true;
        timerUI?.Show(Duration);
        _timerCo = StartCoroutine(CoTimer());
    }

    public void StopTimer()
    {
        if (_timerCo != null) StopCoroutine(_timerCo);
        _timerCo = null;
        TimerRunning = false;
        timerUI?.Hide();
    }

    IEnumerator CoTimer()
    {
        while (RemainingTime > 0f)
        {
            RemainingTime -= Time.deltaTime;
            if (RemainingTime < 0f) RemainingTime = 0f;
            timerUI?.UpdateTime(RemainingTime, Duration);
            OnTimerTick?.Invoke(RemainingTime);
            yield return null;
        }
        TimerRunning = false;
        OnTimerEnded?.Invoke();
    }

    //ACTIONS QUI SE LANCENT QUAND ON GAGNE OU PERD UN MINI-JEU
    public void NotifyWin()
    {
        StopTimer();
        OnMinigameWon?.Invoke();
    }

    public void NotifyFail()
    {
        StopTimer();
        OnMinigameFailed?.Invoke();
    }

    //TOUT CE QUI EST EN DESSOUS : NE PAS UTILISER - A REFACTORER SUR MON MINI-JEU
    public TMP_Text livesText;

    void Update()
    {
        livesText.text = "Vies: " + lives;
        if (Input.GetButtonDown("P1_B6"))
        {
            string nextGame = GetRandomGame(); //ou alors le jeu que vous voulez tester comme ça : nextGame = "SlotMachine";
            scenesLoader.LoadMiniGame(nextGame);
            currentGame = nextGame;
        }
        if (Input.GetButtonDown("P1_B3"))
        {
            if (currentGame != "")
            {
                scenesLoader.UnloadMiniGame(currentGame);
            }
        }
    }

    private string GetRandomGame()
    {
        string[] scenesList = Directory.GetFiles("Assets/Scenes/MiniGames", "*.unity");
        for (int i = 0; i < scenesList.Length; i++)
        {
            scenesList[i] = Path.GetFileNameWithoutExtension(scenesList[i]);
        }

        System.Random rand = new System.Random();
        int index = rand.Next(scenesList.Length);

        if (currentGame == scenesList[index])
        {
            index = (index + 1) % scenesList.Length;
        }

        return scenesList[index];
    }
}
