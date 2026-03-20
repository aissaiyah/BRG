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
    private bool timerStarted = false;

    void Start()
    {
        // Hide panels at start
        if (breakPanel != null) breakPanel.SetActive(false);
        if (gameCompletePanel != null) gameCompletePanel.SetActive(false);

        // Wait until scene is fully loaded before starting the timer
        StartCoroutine(WaitToStartTimer());
    }

    IEnumerator WaitToStartTimer()
    {
        // Skip the first 2 frames (scene loading causes huge deltaTime spikes)
        yield return null;
        yield return null;
        timerStarted = true;
    }

    void Update()
    {
        if (gameComplete) return;
        if (!timerStarted) return;

        // Skip timer updates if SMScript has the game paused
        if (SMScript.Instance != null && SMScript.Instance.paused) return;

        // Use unscaledDeltaTime so session timer keeps running during breaks
        totalTimeElapsed += Time.unscaledDeltaTime;
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
        if (sessionTimerText == null) return;
        float timeRemaining = Mathf.Max(0f, totalGameplayTime - totalTimeElapsed);
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        sessionTimerText.text = string.Format("Session: {0:00}:{1:00}", minutes, seconds);
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;
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
        if (breakPanel != null) breakPanel.SetActive(true);
        Time.timeScale = 0f;

        float breakTimeRemaining = forcedBreakDuration;

        while (breakTimeRemaining > 0)
        {
            int seconds = Mathf.CeilToInt(breakTimeRemaining);
            if (breakCountdownText != null) breakCountdownText.text = seconds.ToString();

            yield return new WaitForSecondsRealtime(1f);
            breakTimeRemaining -= 1f;
        }

        // Resume gameplay
        if (breakPanel != null) breakPanel.SetActive(false);
        Time.timeScale = 1f;
        isInBreak = false;
        isGameplayActive = true;
        currentGameplayTime = 0f;
    }

    public void StartLevelBreak()
    {
        isGameplayActive = false;
        StartCoroutine(LevelBreakCountdown());
    }

    IEnumerator LevelBreakCountdown()
    {
        if (breakPanel != null) breakPanel.SetActive(true);
        Time.timeScale = 0f;

        float breakTimeRemaining = levelBreakDuration;

        while (breakTimeRemaining > 0)
        {
            int seconds = Mathf.CeilToInt(breakTimeRemaining);
            if (breakCountdownText != null) breakCountdownText.text = seconds.ToString();

            yield return new WaitForSecondsRealtime(1f);
            breakTimeRemaining -= 1f;
        }

        if (breakPanel != null) breakPanel.SetActive(false);
        Time.timeScale = 1f;
        currentGameplayTime = 0f;
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

        // Get final score from PelletManager or GMScript
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
        else if (GMScript.Instance != null)
        {
            finalScore = GMScript.Instance.pacmanHighScore;
        }

        if (finalScoreNumberText != null)
        {
            finalScoreNumberText.text = finalScore.ToString();
        }

        if (finalPelletCountText != null)
        {
            finalPelletCountText.text = pelletsCollected + " / " + totalPellets;
        }
    }

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

    public bool IsGameComplete()
    {
        return gameComplete;
    }
}