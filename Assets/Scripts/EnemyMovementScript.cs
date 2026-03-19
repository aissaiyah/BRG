using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Enablegames;
using UnityEngine.SceneManagement;

public class EnemyMovementScript : MonoBehaviour
{
    public NavMeshAgent nav;

    public Transform player;
    public Transform front;
    public Transform randomTarget;

    public Transform CornerInky;
    public Transform CornerPinky;
    public Transform CornerBlinky;
    public Transform CornerClyde;
    public AudioSource death;
    private playerMovementScript playerMovement;

    public float speedIncreaseAmount = 0.5f;
    private float speedMultiplier = 1f;

    bool eatRoutineRunning = false;

    void Start()
    {
        playerMovement ??= FindObjectOfType<playerMovementScript>();
        nav = GetComponent<NavMeshAgent>();

        StartCoroutine(EnemyCycle());
        if (SceneManager.GetActiveScene().name == "Exercise 2")
        {
            StartCoroutine(SpeedIncreaseLoop());
        }
       
    }

    void Update()
    {
        if (playerMovementScript.eatMode && !eatRoutineRunning)
        {
            StartCoroutine(EatModeTimer());
        }
    }

    IEnumerator EnemyCycle()
    {
        while (true)
        {
            Scatter();
            yield return new WaitForSeconds(10f);

            Chase();
            yield return new WaitForSeconds(20f);
        }
    }

    IEnumerator SpeedIncreaseLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(40f);
            speedMultiplier += speedIncreaseAmount;
            Debug.Log("Speed multiplier increased to: " + speedMultiplier);
        }
    }

    void Scatter()
    {
        if (gameObject.name == "Inky")
        {
            SetGhostSpeed(SMScript.InkySpeed * speedMultiplier, ParameterStrings.InkyMoveSpeed);
            nav.SetDestination(CornerInky.position);
        }
        else if (gameObject.name == "Pinky")
        {
            SetGhostSpeed(SMScript.PinkySpeed * speedMultiplier, ParameterStrings.PinkyMoveSpeed);
            nav.SetDestination(CornerPinky.position);
        }
        else if (gameObject.name == "Blinky")
        {
            SetGhostSpeed(SMScript.BlinkySpeed * speedMultiplier, ParameterStrings.BlinkyMoveSpeed);
            nav.SetDestination(CornerBlinky.position);
        }
        else if (gameObject.name == "Clyde")
        {
            SetGhostSpeed(SMScript.ClydeSpeed * speedMultiplier, ParameterStrings.ClydeMoveSpeed);
            nav.SetDestination(CornerClyde.position);
        }
    }

    void Chase()
    {
        if (gameObject.name == "Inky")
        {
            nav.SetDestination(player.position);
        }
        else if (gameObject.name == "Pinky")
        {
            nav.SetDestination(front.position);
        }
        else if (gameObject.name == "Blinky")
        {
            nav.SetDestination(player.position);
        }
        else if (gameObject.name == "Clyde")
        {
            nav.SetDestination(player.position);
        }
    }

    void SetGhostSpeed(float speedValue, string parameterName)
    {
        nav.speed = speedValue;

        VariableHandler.Instance.Register(
            parameterName,
            (egFloat)speedValue
        );
    }

    IEnumerator EatModeTimer()
    {
        eatRoutineRunning = true;

        yield return new WaitForSeconds(10f);
        playerMovementScript.eatMode = false;

        eatRoutineRunning = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerMovementScript.eatMode)
            {
                death.Play();
                Destroy(collision.gameObject);
                playerMovement.win = true;
            }
            else
            {
                death.Play();
                Destroy(gameObject);
                GMScript.Instance.pacmanHighScore += 200;
            }
        }
    }
}