using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
//using eaglib;
//EG REQUIRED
//using enableGame;
using UnityEngine.Networking;
#if UNITY_IOS 
//&& !UNITY_EDITOR
using UnityEngine.XR.ARFoundation;
#endif
#if UNITY_IOS 
using ARFoundationRemote.Runtime;
#endif
using Enablegames;
using Enablegames.Suki;
public class playerMovementScript : MonoBehaviour
{
    public egFloat speed = 0f;
    public Vector3 moveDirection;
    public Rigidbody rb;
    public bool moving;

    private egString sukiFile = "R_Hand.suki";
  //  public Transform transform;
    public float rotate;
    
    public static bool eatMode;
    public int pelletWin;
    public bool win;
    public Transform PlayerObject;
    public Transform teleport1;
    public Transform teleport2;
    public AudioSource pelletEat;
    public AudioSource teleport;
    public float teleportCooldown = 1f; // seconds
    private bool canTeleport = true;
    [SerializeField] float xThreshold = .3f;
    [SerializeField] float yThreshold = .3f;
    public int winShow;
    
    SukiInput sukiInput;
    // Start is called before the first frame update
    public void Awake()
    {
        sukiInput = SukiInput.Instance;
        
    }
    void Start()
    {
	    PlayerObject = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
       // transform = GetComponent<Transform>();
      ///  speed = 9;
        moving = false;
        if (SceneManager.GetActiveScene().name == "Exercise 1")
        {
            
        }
        else if (SceneManager.GetActiveScene().name == "Exercise 2")
        {
            pelletWin = 160;
        }
        else if (SceneManager.GetActiveScene().name == "Exercise 3")
        {
            pelletWin = 180;
        }
        else
        {
            pelletWin = 984;
        }
        // condition for ending the game via eating all pelletts
        GMScript.Instance.pacmanHighScore = 0;

    }

    // Update is called once per frame
    void Update()
    {
        winShow = pelletWin;
        if(pelletWin <= 1)// if 1 or no pellet left win the game load win scene
        {
            win = true;
           // SceneManager.LoadScene("winScene");
        }
        

        if (Input.GetAxisRaw("Horizontal") > 0)// if left or right rotate the object and round to a whole number
        {
           // transform.rotation = Quaternion.Euler(0, rotate, 0);
          //  rotate += Mathf.Ceil(1F);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
           // transform.rotation = Quaternion.Euler(0, rotate, 0);
           // rotate -= Mathf.Ceil(1F);
        }
  
    }

    public void FixedUpdate()// move the player object at a certain speed
    {

        

       // transform.rotation = Quaternion.Euler(0, rotate, 0);
        if (moving)
        {    
            speed = SMScript.PlayerSpeed;
            
            

            VariableHandler.Instance.Register(ParameterStrings.PlayerMoveSpeed, speed);
            VariableHandler.Instance.Register(ParameterStrings.sukiFile, sukiFile);
           // egFloat PlayerSpeed = SMScript.speed;
            //VariableHandler.Instance.Register(ParameterStrings.PlayerMoveSpeed, PlayerSpeed);
           // speed = PlayerSpeed;
           /* egFloat PlayerSpeed = SMScript.speed;
            VariableHandler.Instance.Register(ParameterStrings.PlayerMoveSpeed, PlayerSpeed);
            
            speed = PlayerSpeed;*/
        }
        if (!moving)
        {
          //  speed = 0;
        }
       
        var input = Vector2.zero;
        if (sukiInput.Location2DExists("righthand"))
        {
            input = sukiInput.GetLocation2D("righthand");
            input = input * 2 - new Vector2(1, 1);
        }
/*


        
                if (sukiInput.Location2DExists("lefthand"))
        {
            input = sukiInput.GetLocation2D("lefthand");
            input = input * 2 - new Vector2(1, 1);
        }
        else if (sukiInput.Location2DExists("RKnee"))
        {
            input = sukiInput.GetLocation2D("RKnee");
            input = input * 2 - new Vector2(1, 1);
        }
        else if (sukiInput.Location2DExists("LKnee"))
        {
            input = sukiInput.GetLocation2D("LKnee");
            input = input * 2 - new Vector2(1, 1);
        }*/
        print("2DInput = "  + input.ToString());
        print("2DExtent max = " + sukiInput.GetExtentMax2D("righthand") + " min = " + sukiInput.GetExtentMin("righthand"));
        
        if (Input.GetKey(KeyCode.W) || input.x > yThreshold)
        {
            rb.velocity = new Vector3(rb.velocity.x * 0, rb.velocity.y, -speed);
            Debug.Log("UP");
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S) || input.x < -yThreshold)
        {
            Debug.Log("Down");
            rb.velocity = new Vector3(rb.velocity.x * 0, rb.velocity.y, speed);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A) || input.y > xThreshold)
        {
            Debug.Log("Left");
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z * 0);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.D) || input.y < -xThreshold)
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z * 0);
            Debug.Log("Right");
            moving = true;
        }

        if (Input.GetKeyUp(KeyCode.W) || (input.y < -yThreshold && input.y > -yThreshold))// when not holding the button down stop moving and applying speed
        {

            //moving = false;
        }
        if (Input.GetKeyUp(KeyCode.S) || (input.y < -yThreshold && input.y > -yThreshold))
        {

            //moving = false;
        }
        if (Input.GetKeyUp(KeyCode.A) || (input.x < xThreshold && input.x > -xThreshold))// when not holding the button down stop moving and applying speed
        {

	      //  moving = false;
        }
        if (Input.GetKeyUp(KeyCode.D) || (input.x < xThreshold && input.x > -xThreshold))
        {

	       // moving = false;
        }
    }
    void OnTriggerEnter(Collider collider)// on collection with the 7th layer increase pellet by 10 and delete it
    {

        if(collider.gameObject.CompareTag("SmallPellet"))
        {
            Destroy(collider.gameObject);
            GMScript.Instance.pacmanHighScore += 10;
            pelletWin -= 1;
            pelletEat.Play();
        }

        if (collider.gameObject.CompareTag("BigPellet"))//power pellete increase count more and be able to kill ghosts
        {
            Destroy(collider.gameObject);
            GMScript.Instance.pacmanHighScore += 50;
            eatMode = true;
            pelletWin -= 1;
            pelletEat.Play();
        }

        if (!canTeleport) return;

        if (collider.gameObject.CompareTag("Teleporter1"))
        {
            teleport.Play();
            StartCoroutine(TeleportWithDelay(teleport2.position));
        }
        else if (collider.gameObject.CompareTag("Teleporter2"))
        {
            teleport.Play();
            StartCoroutine(TeleportWithDelay(teleport1.position));
        }
    }
    
    private IEnumerator TeleportWithDelay(Vector3 targetPosition)
    {
        canTeleport = false;

        PlayerObject.position = targetPosition;

        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;
    }
}
