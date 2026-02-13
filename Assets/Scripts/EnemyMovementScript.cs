using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enablegames;


public class EnemyMovementScript : MonoBehaviour
{
    public NavMeshAgent nav;
    public Transform player;
    public Transform front;
    public Transform Random;
    public Transform CornerInky;
    public Transform CornerPinky;
    public Transform CornerBlinky;
    public Transform CornerClyde;
    public float Speed;
    public GameObject[] inky, pinky, blinky, clyde; // create multiple game objects

    // Start is called before the first frame update

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();// set nav mesh to nav

        egFloat InkySpeed = Speed;
        VariableHandler.Instance.Register(ParameterStrings.InkyMoveSpeed, InkySpeed);
        
        
        egFloat PinkySpeed = Speed;
        VariableHandler.Instance.Register(ParameterStrings.PinkyMoveSpeed, PinkySpeed);

        
        egFloat BlinkySpeed = Speed;
        VariableHandler.Instance.Register(ParameterStrings.BlinkyMoveSpeed, BlinkySpeed);

        
        egFloat ClydeSpeed = Speed;
        VariableHandler.Instance.Register(ParameterStrings.ClydeMoveSpeed, ClydeSpeed);

    }

    // Update is called once per frame
    void Update()// every frame run enemyCycle if eat mode is enabled run kill enemy and if false stop running it
    {

        StartCoroutine(enemyCycle());

        if (playerMovementScript.eatMode)
        {
            
            StartCoroutine(killEnemy());
        }
        if (!playerMovementScript.eatMode)
        {
            StopCoroutine(killEnemy());
           
        }


    }

    void Scatter()// function for scattering ghosts to 4 corners start of game
    {
        if (gameObject.name == "Inky")//track the front of the player
        {
            egFloat InkySpeed = Speed;
            VariableHandler.Instance.Register(ParameterStrings.InkyMoveSpeed, InkySpeed);
            nav.speed = InkySpeed;
            nav.SetDestination(CornerInky.position);

        }

        if (gameObject.name == "Pinky")//track the front of the player
        {
            egFloat PinkySpeed = Speed;
            VariableHandler.Instance.Register(ParameterStrings.PinkyMoveSpeed, PinkySpeed);
            nav.speed = PinkySpeed;
            nav.SetDestination(CornerPinky.position);
        }

        if (gameObject.name == "Blinky")//track the front of the player
        {
            egFloat BlinkySpeed = Speed;
            VariableHandler.Instance.Register(ParameterStrings.BlinkyMoveSpeed, BlinkySpeed);
            nav.speed = BlinkySpeed;
            nav.SetDestination(CornerBlinky.position);
        }

        if (gameObject.name == "Clyde")//track the front of the player
        {
            egFloat ClydeSpeed = Speed;
            VariableHandler.Instance.Register(ParameterStrings.ClydeMoveSpeed, ClydeSpeed);
            nav.speed = ClydeSpeed;
            nav.SetDestination(CornerClyde.position);
        }
    }

    void Chase()// funciton for how to chase the player 2 ghosts chase the player while the other ghosts try to flank and trap the player
    {
        if (gameObject.name == "Inky")// chase after amount of dots are consumed
        {
            nav.SetDestination(Random.position);
        }

        if (gameObject.name == "Pinky")//track the front of the player
        {
            nav.SetDestination(front.position);
        }

        if (gameObject.name == "Blinky")// always track player then speed up after amount of dots are consumed
        {
            nav.SetDestination(player.position);
        }

        if (gameObject.name == "Clyde")// track the player after amount of dots are consumed then in distance go to another point on the screen
        {
            nav.SetDestination(player.position);
        }
    }

    void OnCollisionEnter(Collision collision)// on collision with player destroy the object end the game
    {

        if (collision.gameObject.tag == "Player")
        {

            if (!playerMovementScript.eatMode)
            {
                Destroy(collision.gameObject);
                playerMovementScript.win = true;
            }

            if (playerMovementScript.eatMode)// on collision if eat mode destroy the ghost instead
            {
                Destroy(gameObject);
                playerMovementScript.pelletCount += 200;
            }
            
        }
    }

     IEnumerator enemyCycle()// run scatter funciton for 10 secs then run chase function permanetly
     {


        Scatter();
        yield return new WaitForSeconds(10);
        Chase();
        
     }

    IEnumerator killEnemy()// wait for 1 second then disable eatmode
    {
        yield return new WaitForSeconds(10);
        playerMovementScript.eatMode = false;
        yield return null;

    }


}
