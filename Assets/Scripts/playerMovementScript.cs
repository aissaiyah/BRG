using System.Collections;
using System.Collections.Generic;
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
    public float speed;
    public Vector3 moveDirection;
    public Rigidbody rb;
    public bool moving;
  //  public Transform transform;
    public float rotate;
    public static int pelletCount;
    public static bool eatMode;
    public int pelletWin;
    public static bool win;
    public Transform PlayerObject;
    [SerializeField] float xThreshold = .3f;
    [SerializeField] float yThreshold = .3f;
    
    SukiInput sukiInput;
    // Start is called before the first frame update
    public void Awake()
    {
        sukiInput = SukiInput.Instance;
    }
    void Start()
    {
	    speed = 9;
	    PlayerObject = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
       // transform = GetComponent<Transform>();
        speed = 9;
        moving = false;
        pelletWin = 492;// condition for ending the game via eating all pelletts

    }

    // Update is called once per frame
    void Update()
    {
        if(pelletWin <= 1)// if 1 or no pellet left win the game load win scene
        {
            win = true;
            SceneManager.LoadScene("winScene");
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
            speed = 9;
        }
        if (!moving)
        {
            speed = 0;
        }
        
        var input = Vector2.zero;

        if (sukiInput.Location2DExists("righthand"))
        {
            input = sukiInput.GetLocation2D("righthand");
            input = input * 2 - new Vector2(1, 1);
        }
        print("2DInput = "  + input.ToString());
        print("2DExtent max = " + sukiInput.GetExtentMax2D("righthand") + " min = " + sukiInput.GetExtentMin("righthand"));
        
        if (Input.GetKey(KeyCode.W) || input.y > yThreshold)// control direction velocity is enforced
        {
            rb.velocity = transform.forward * speed;
            moving = true;
        }
        if (Input.GetKey(KeyCode.S) || input.y < -yThreshold)
        {
            rb.velocity = -transform.forward * speed;
            moving = true;
        }
        if (Input.GetKey(KeyCode.A) || input.x < -xThreshold)// control direction velocity is enforced
        {
	        rb.velocity = -transform.right * speed;
	        moving = true;
        }
        if (Input.GetKey(KeyCode.D) || input.x > xThreshold)
        {
	        rb.velocity = transform.right * speed;
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

        if(collider.gameObject.layer == 7)
        {
            Destroy(collider.gameObject);
            pelletCount += 10;
            pelletWin -= 1;
        }

        if (collider.gameObject.layer == 9)//power pellete increase count more and be able to kill ghosts
        {
            Destroy(collider.gameObject);
            pelletCount += 50;
            eatMode = true;
            pelletWin -= 1;
        }
    }
}
