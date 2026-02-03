using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTherapeuticBehavior : MonoBehaviour
{
    [Header("Speed Adjustment")]
    [SerializeField] private bool adjustSpeed = true;
    [SerializeField] private float customSpeed = 3f;
    
    [Header("Tired Mode")]
    [SerializeField] private bool enableTiredMode = true;
    [SerializeField] private float chaseTimeBeforeTired = 15f; // Seconds of chasing before getting tired
    [SerializeField] private float tiredDuration = 5f;         // Seconds to rest
    [SerializeField] private float tiredSpeedMultiplier = 0.5f; // Speed during tired mode (0.5 = half speed)
    
    [Header("Difficulty Reduction")]
    [SerializeField] private bool startDisabled = false; // Start this enemy inactive
    [SerializeField] private float delayBeforeActivation = 30f; // Seconds before activating
    
    [Header("Visual Feedback")]
    [SerializeField] private bool changeColorWhenTired = true;
    [SerializeField] private Color tiredColor = Color.gray;
    
    private NavMeshAgent nav;
    private float originalSpeed;
    private bool isTired = false;
    private float chaseTimer = 0f;
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isActive = true;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponent<Renderer>();
        
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
        
        // Adjust speed if enabled
        if (adjustSpeed && nav != null)
        {
            nav.speed = customSpeed;
            originalSpeed = customSpeed;
        }
        else if (nav != null)
        {
            originalSpeed = nav.speed;
        }
        
        // Start disabled if needed
        if (startDisabled)
        {
            DisableEnemy();
            StartCoroutine(ActivateAfterDelay());
        }
    }

    void Update()
    {
        if (!isActive) return;
        
        // Tired mode logic
        if (enableTiredMode && !isTired && nav != null && nav.enabled)
        {
            // Only count time when enemy is actually moving/chasing
            if (nav.velocity.magnitude > 0.1f)
            {
                chaseTimer += Time.deltaTime;
                
                if (chaseTimer >= chaseTimeBeforeTired)
                {
                    StartCoroutine(TiredMode());
                }
            }
        }
    }
    
    IEnumerator TiredMode()
    {
        isTired = true;
        
        // Visual feedback
        if (changeColorWhenTired && enemyRenderer != null)
        {
            enemyRenderer.material.color = tiredColor;
        }
        
        // Slow down
        if (nav != null)
        {
            nav.speed = originalSpeed * tiredSpeedMultiplier;
        }
        
        // Stay tired for duration
        yield return new WaitForSeconds(tiredDuration);
        
        // Recover
        if (nav != null)
        {
            nav.speed = originalSpeed;
        }
        
        if (changeColorWhenTired && enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
        
        isTired = false;
        chaseTimer = 0f; // Reset timer
    }
    
    void DisableEnemy()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
    
    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeActivation);
        gameObject.SetActive(true);
        isActive = true;
    }
    
    // Public methods to control from other scripts
    public void SetSpeed(float newSpeed)
    {
        customSpeed = newSpeed;
        originalSpeed = newSpeed;
        if (nav != null)
        {
            nav.speed = newSpeed;
        }
    }
    
    public void EnableTiredMode(bool enable)
    {
        enableTiredMode = enable;
    }
    
    public void ResetTiredTimer()
    {
        chaseTimer = 0f;
        isTired = false;
    }
}