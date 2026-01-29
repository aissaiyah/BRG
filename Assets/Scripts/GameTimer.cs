using UnityEngine;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float totalGameplayTime = 540f; // 9 minutes total
    [SerializeField] private float gameplayDuration = 60f; // 1 minute before forced break
    [SerializeField] private float forcedBreakDuration = 30f; // 30 second break
    [SerializeField] private float levelBreakDuration = 60f; // 1 minute between levels
    
    [Header("UI References")]
    [SerializeField] private TMP_Text sessionTimerText; // Shows total session time
    [SerializeField] private TMP_Text timerText; // Shows current gameplay segment time
    [SerializeField] private TMP_Text breakCountdownText;
    [SerializeField] private GameObject breakPanel;
    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private TMP_Text finalScoreNumberText; // Just the score number
    [SerializeField] private TMP_Text finalPelletCountText; // Just the pellet count
    
    private float totalTimeElapsed = 0f;
    private float currentGameplayTime = 0f;
    private bool isInBreak = false;
    private bool isGameplayActive = true;
    private bool gameComplete = false;
    
    void Update()
    {
        if (gameComplete) return;
        
        // Always track total time (even during breaks)
        totalTimeElapsed += Time.deltaTime;
        UpdateSessionTimerDisplay();
        
        // Check if total gameplay time is up
        if (totalTimeElapsed >= totalGameplayTime)
        {
            EndGameSession();
            return;
        }
        
        if (isGameplayActive && !isInBreak)
        {
            currentGameplayTime += Time.deltaTime;
            UpdateTimerDisplay();
            
            // Check if we need to force a break
            if (currentGameplayTime >= gameplayDuration)
            {
                StartForcedBreak();
            }
        }
    }
    
    void UpdateSessionTimerDisplay()
    {
        float timeRemaining = totalGameplayTime - totalTimeElapsed;
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        sessionTimerText.text = string.Format("Session: {0:00}:{1:00}", minutes, seconds);
    }
    
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentGameplayTime / 60f);
        int seconds = Mathf.FloorToInt(currentGameplayTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void StartForcedBreak()
    {
        isInBreak = true;
        isGameplayActive = false;
        StartCoroutine(ForcedBreakCountdown());
    }
    
    IEnumerator ForcedBreakCountdown()
    {
        breakPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        
        float breakTimeRemaining = forcedBreakDuration;
        
        while (breakTimeRemaining > 0)
        {
            int seconds = Mathf.CeilToInt(breakTimeRemaining);
            breakCountdownText.text = seconds.ToString();
            
            yield return new WaitForSecondsRealtime(1f);
            breakTimeRemaining -= 1f;
        }
        
        // Resume gameplay
        breakPanel.SetActive(false);
        Time.timeScale = 1f;
        isInBreak = false;
        isGameplayActive = true;
        currentGameplayTime = 0f; // Reset timer for next cycle
    }
    
    public void StartLevelBreak()
    {
        isGameplayActive = false;
        StartCoroutine(LevelBreakCountdown());
    }
    
    IEnumerator LevelBreakCountdown()
    {
        breakPanel.SetActive(true);
        Time.timeScale = 0f;
        
        float breakTimeRemaining = levelBreakDuration;
        
        while (breakTimeRemaining > 0)
        {
            int seconds = Mathf.CeilToInt(breakTimeRemaining);
            breakCountdownText.text = seconds.ToString();
            
            yield return new WaitForSecondsRealtime(1f);
            breakTimeRemaining -= 1f;
        }
        
        breakPanel.SetActive(false);
        Time.timeScale = 1f;
        currentGameplayTime = 0f;
        // Call your level transition code here
    }
    
    void EndGameSession()
    {
        gameComplete = true;
        isGameplayActive = false;
        Time.timeScale = 0f;
        
        if (gameCompletePanel != null)
        {
            gameCompletePanel.SetActive(true);
        }
        
        // Get final score from PelletManager
        PelletManager pelletManager = FindObjectOfType<PelletManager>();
        int finalScore = 0;
        int pelletsCollected = 0;
        int totalPellets = 0;
        
        if (pelletManager != null)
        {
            finalScore = pelletManager.GetScore();
            pelletsCollected = pelletManager.GetPelletsCollected();
            totalPellets = pelletManager.GetTotalPellets();
        }
        
        // Only update the numbers, you control the text in UI
        if (finalScoreNumberText != null)
        {
            finalScoreNumberText.text = finalScore.ToString();
        }
        
        if (finalPelletCountText != null)
        {
            finalPelletCountText.text = pelletsCollected + " / " + totalPellets;
        }
    }
    
    // Public method to end game early when all pellets collected
    public void EndGameSessionEarly()
    {
        EndGameSession();
    }
    
    public void PauseGame()
    {
        isGameplayActive = false;
    }
    
    public void ResumeGame()
    {
        if (!isInBreak && !gameComplete)
        {
            isGameplayActive = true;
        }
    }
    
    public void ResetTimer()
    {
        currentGameplayTime = 0f;
    }
}