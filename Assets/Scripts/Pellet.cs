using UnityEngine;

public class Pellet : MonoBehaviour
{
    [Header("Pellet Settings")]
    [SerializeField] private int points = 10;
    [SerializeField] private bool isMegaPellet = false;
    [SerializeField] private int megaPelletPoints = 50;
    
    private PelletManager pelletManager;
    private bool isCollected = false; // Prevent double collection
    
    void Start()
    {
        pelletManager = FindObjectOfType<PelletManager>();
        
        if (pelletManager == null)
        {
            Debug.LogError("PelletManager not found in scene!");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Prevent collecting the same pellet twice
        if (isCollected) return;

        // Skip if this pellet has SmallPellet/BigPellet tag
        // (playerMovementScript handles those and notifies PelletManager)
        if (gameObject.CompareTag("SmallPellet") || gameObject.CompareTag("BigPellet"))
            return;

        if (other.CompareTag("Player"))
        {
            CollectPellet();
        }
    }
    
    void CollectPellet()
    {
        isCollected = true; // Mark as collected immediately
        
        int pelletPoints = isMegaPellet ? megaPelletPoints : points;
        
        if (pelletManager != null)
        {
            pelletManager.CollectPellet(pelletPoints);
        }
        
        // Destroy the pellet
        Destroy(gameObject);
    }
}