using UnityEngine;

public class MiniGame_CalculManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CalculLogic calculLogic;
    [SerializeField] private CalculUIManager calculUIManager;  

    private int wrongAttempts;

    void Start()
    {
        if (calculLogic == null) calculLogic = FindObjectOfType<CalculLogic>();
        if (calculUIManager == null) calculUIManager = FindObjectOfType<CalculUIManager>();
        
        gameManager.OnTimerEnded += HandleTimerEnded;
        gameManager.StartTimer(25f);
        Debug.Log("[CalculManager] Timer started for 25s");
        wrongAttempts = 0;
        GenerateNewCalculation();
    }

    void OnDestroy()
    {
        gameManager.OnTimerEnded -= HandleTimerEnded;
    }

    public void GenerateNewCalculation()
    {
        if (calculUIManager == null)
        {
            Debug.LogWarning("[CalculManager] Cannot generate calculation: missing refs");
            return;
        }
        var calcul = calculLogic.GenerateCalculation();
        Debug.Log($"[CalculManager] New calculation generated -> {calcul.Question} | {string.Join(", ", calcul.Answers)}");
        calculUIManager.DisplayCalculation(calcul);
    }

    public bool OnAnswerSelected(int index)
    {
        // Check if game is over (no lives left)
        if (gameManager.Lives <= 0)
        {
            Debug.Log("[CalculManager] Game over - no lives left, ignoring input");
            return false;
        }
        
        bool correct = calculLogic.CheckAnswer(index);
        
        if (correct)
        {
            Debug.Log("[CalculManager] Correct answer selected");
            // Do not end the minigame on each correct answer; UI will advance to next question
            return true;
        }
        
        wrongAttempts++;
        
            gameManager.LoseLife(); // visually update lives on each wrong answer
            
            // Check if game is over after losing a life
            if (gameManager.Lives <= 0)
            {
                Debug.Log("[CalculManager] Game over - no lives left!");
                gameManager.NotifyFail();
                return false;
        }
        else
        {
            Debug.LogError("[CalculManager] gameManager is null - cannot lose life!");
        }
        Debug.Log($"[CalculManager] Wrong answer. Attempts={wrongAttempts}/3");
        if (wrongAttempts >= 3)
        {
            Debug.Log("[CalculManager] Max wrong attempts reached. Ending.");
            gameManager.NotifyFail();
            return false;
        }
        return false; 
    }

    private void HandleTimerEnded()
    {
        Debug.Log("[CalculManager] Timer ended → failing minigame");
        gameManager?.NotifyFail();
    }
}
