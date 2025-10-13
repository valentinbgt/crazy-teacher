using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

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
    private AudioListener _activeAudioListener;

    // Back to menu manager
    private float afkTimer = 0f;
    private readonly float timeBeforeKick = 10f; //seconds
    private bool afk = false;
    private float quitTimer = 0f;
    private readonly float timeBeforeQuit = 2f;
    private bool quit = false;
    //[END] Back to menu manager

    public void getRandomGame()
    {
        string[] scenesList = {
        "SlotMachine",
        "BallDropper",
        "TriPommePoire",
        };
        System.Random rand = new System.Random();
        int index = rand.Next(scenesList.Length);
        scenesLoader.LoadMiniGame(scenesList[index]);
    }

    void Awake()
    {
        scenesLoader = GetComponent<ScenesLoader>();
        Lives = startingLives;
        livesUI?.SetLives(Lives);
        RoundsPlayed = 0;
        EnsureSingleAudioListener();
        getRandomGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        EnsureSingleAudioListener();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
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
        if (!HasExactlyOneActiveAudioListener())
        {
            EnsureSingleAudioListener();
        }

        livesText.text = "Vies: " + lives;

        // Back to menu manager
        if (Input.GetButton("P1_Vertical") ||
            Input.GetButton("P1_Horizontal") ||
            Input.GetButton("P2_Vertical") ||
            Input.GetButton("P2_Horizontal") ||
            Input.GetButton("P1_Start") ||
            Input.GetButton("P1_B1") ||
            Input.GetButton("P1_B2") ||
            Input.GetButton("P1_B3") ||
            Input.GetButton("P1_B4") ||
            Input.GetButton("P1_B5") ||
            Input.GetButton("P1_B6") ||
            Input.GetButton("P2_Start") ||
            Input.GetButton("P2_B1") ||
            Input.GetButton("P2_B2") ||
            Input.GetButton("P2_B3") ||
            Input.GetButton("P2_B4") ||
            Input.GetButton("P2_B5") ||
            Input.GetButton("P2_B6") ||
            Input.GetButton("Coin"))
        {
            afkTimer = 0f;
        }

        if (afkTimer < timeBeforeKick)
        {
            afk = false;
        }

        if (afk || Input.GetButton("Coin"))
        {
            quit = true;
        }
        else
        {
            quit = false;
            quitTimer = 0f;
        }
        //[END] Back to menu manager
    }

    void FixedUpdate()
    {
        // Back to menu manager
        if (afkTimer < timeBeforeKick)
        {
            afkTimer += Time.fixedDeltaTime;
            Debug.Log("afkTimer is : " + afkTimer);
        }
        else
        {
            if (!afk)
            {
                Debug.Log("You will be kicked in 2 seconds");
                afk = true;
            }
        }

        if (quit)
        {
            quitTimer += Time.fixedDeltaTime;
        }
        else
        {
            quitTimer = 0f;
        }

        if (quitTimer >= timeBeforeQuit)
        {
            //ASK API TO QUIT
            Debug.Log("QUITTING (TBD)");
        }
        //[END] Back to menu manager
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureSingleAudioListener();
    }

    private void EnsureSingleAudioListener()
    {
        var listeners = FindObjectsOfType<AudioListener>();
        AudioListener primary = null;

        for (int i = 0; i < listeners.Length; i++)
        {
            var listener = listeners[i];
            if (listener == null) continue;

            if (listener.isActiveAndEnabled)
            {
                if (primary == null)
                {
                    primary = listener;
                    continue;
                }

                listener.enabled = false;
                continue;
            }

            if (primary == null)
            {
                primary = listener;
                if (!primary.enabled) primary.enabled = true;
                continue;
            }

            listener.enabled = false;
        }

        if (primary == null)
        {
            primary = AttachListenerToMainCamera();
        }

        if (primary == null)
        {
            primary = GetComponent<AudioListener>() ?? gameObject.AddComponent<AudioListener>();
            primary.enabled = true;
        }

        if (!primary.enabled)
        {
            primary.enabled = true;
        }

        _activeAudioListener = primary;
    }

    private AudioListener AttachListenerToMainCamera()
    {
        var mainCamera = Camera.main;
        if (mainCamera == null) return null;

        var listener = mainCamera.GetComponent<AudioListener>();
        if (listener == null)
        {
            listener = mainCamera.gameObject.AddComponent<AudioListener>();
        }

        listener.enabled = true;
        return listener;
    }

    private bool HasExactlyOneActiveAudioListener()
    {
        var listeners = FindObjectsOfType<AudioListener>();
        int activeCount = 0;

        for (int i = 0; i < listeners.Length; i++)
        {
            var listener = listeners[i];
            if (listener == null) continue;

            if (listener.enabled && listener.gameObject.activeInHierarchy)
            {
                activeCount++;
                if (activeCount > 1)
                {
                    return false;
                }
            }
        }

        return activeCount == 1;
    }
}
