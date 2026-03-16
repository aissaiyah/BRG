using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Enablegames;

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
    private playerMovementScript playerMovement;

    bool eatRoutineRunning = false;

    void Start()
    {
        playerMovement ??= FindObjectOfType<playerMovementScript>();
        nav = GetComponent<NavMeshAgent>();

        StartCoroutine(EnemyCycle());
    }

    void Update()
    {
        // Handle eat mode timer safely
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

    void Scatter()
    {
        if (gameObject.name == "Inky")
        {
            SetGhostSpeed(SMScript.InkySpeed, ParameterStrings.InkyMoveSpeed);
            nav.SetDestination(CornerInky.position);
        }
        else if (gameObject.name == "Pinky")
        {
            SetGhostSpeed(SMScript.PinkySpeed, ParameterStrings.PinkyMoveSpeed);
            nav.SetDestination(CornerPinky.position);
        }
        else if (gameObject.name == "Blinky")
        {
            SetGhostSpeed(SMScript.BlinkySpeed, ParameterStrings.BlinkyMoveSpeed);
            nav.SetDestination(CornerBlinky.position);
        }
        else if (gameObject.name == "Clyde")
        {
            SetGhostSpeed(SMScript.ClydeSpeed, ParameterStrings.ClydeMoveSpeed);
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
                Destroy(collision.gameObject);
                playerMovement.win = true;
            }
            else
            {
                Destroy(gameObject);
                GMScript.Instance.pacmanHighScore += 200;
            }
        }
    }
}