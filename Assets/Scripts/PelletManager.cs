using UnityEngine;
using TMPro;

public class PelletManager : MonoBehaviour
{
    [Header("Pellet Settings")]
    [SerializeField] private int totalPellets = 0; // Set this to total pellets in level
    [SerializeField] private int pointsPerPellet = 10;
    
    [Header("UI References")]
    [SerializeField] private TMP_Text pelletCountText;
    [SerializeField] private TMP_Text scoreText;
    
    private int pelletsCollected = 0;
    private int currentScore = 0;
    
    void Start()
    {
        // Auto-count pellets if not set manually
        if (totalPellets == 0)
        {
            GameObject pelletParent = GameObject.Find("Pellets");
            if (pelletParent != null)
            {
                totalPellets = pelletParent.transform.childCount;
            }
        }
        
        UpdateUI();
    }
    
    public void CollectPellet(int points)
    {
        pelletsCollected++;
        currentScore += points;
        UpdateUI();
        
        // Check if all pellets collected
        if (pelletsCollected >= totalPellets)
        {
            OnAllPelletsCollected();
        }
    }
    
    void UpdateUI()
    {
        if (pelletCountText != null)
        {
            pelletCountText.text = pelletsCollected + " / " + totalPellets;
        }
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
    
    void OnAllPelletsCollected()
    {
        Debug.Log("All pellets collected! Level complete!");
        // You can trigger level completion here
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
        {
            timer.StartLevelBreak();
        }
    }
    
    public int GetPelletsCollected()
    {
        return pelletsCollected;
    }
    
    public int GetTotalPellets()
    {
        return totalPellets;
    }
    
    public int GetScore()
    {
        return currentScore;
    }
}