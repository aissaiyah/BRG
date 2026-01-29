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
    public Transform transform;
    public float rotate;
    public static int pelletCount;
    public static bool eatMode;
    public int pelletWin;
    public static bool win;
    public Transform PlayerObject;
    // Start is called before the first frame update
    public void Awake()
    {
	    egAwake();
    }
    void Start()
    {
	    PlayerObject = GetComponent<Transform>();
        egStart();
        egBeginSession();
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
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
            transform.rotation = Quaternion.Euler(0, rotate, 0);
            rotate += Mathf.Ceil(1F);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, rotate, 0);
            rotate -= Mathf.Ceil(1F);
        }
    }

    public void FixedUpdate()// move the player object at a certain speed
    {

        transform.rotation = Quaternion.Euler(0, rotate, 0);
        if (moving)
        {
            speed = 9;
        }
        if (!moving)
        {
            speed = 0;
        }
        if (Input.GetKey(KeyCode.W))// control direction velocity is enforced
        {
            rb.velocity = transform.forward * speed;
            moving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -transform.forward * speed;
            moving = true;
        }

        if (Input.GetKeyUp(KeyCode.W))// when not holding the button down stop moving and applying speed
        {

            moving = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {

            moving = false;
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

    void OnApplicationQuit()
    {
	    egEndSession();
    }

/// BEGIN ENABLEGAMES REQUIRED CODE
	/// </summary>
	/// 
	/// 
	/// 
	public ParameterHandler ph; 

	public Enablegames.SkeletonData Skeleton;  		//holds the body data for the avatar

	public RoboticData roboticData;
	private SukiInput suki = null; //maps avatar body data to game input

	//egFloat,etc. are custom variables that can be attached to parameters in the settings menu and portal
	//They are attached to the parameters in the egAwake function below.
	egFloat Speed=1.0f;		//speed of player
	egFloat Gravity=-1.0f;	//falling cylinder's gravity (-1.0 is unity default)
	egInt GameLength=300000; 	//in seconds

	// Use this for initialization
	void egStart () {
		suki = SukiInput.Instance;
		suki.Skeleton = Skeleton;
		Debug.Log("suki.skel = "+ suki.Skeleton);
		// Bind Speed to the variable "STARTING SPEED" from the settings menu
		//NOTE:Binding will be skipped if ParameterHandler not loaded (i.e. running this scene 
		//without first running MainMenu scene)
		//Also, parameters must be added to DefaultParameters.json file (located in StreamingAssets folder).
	}

	bool egStarted = false;

	bool egInitialized()
	{
		if (egStarted)
			return true;
		print("egInitialized: "+ egStarted);
		if (ph == null)
			return false;	
		GameParameters gp = (GameParameters)ParameterHandler.Instance.AllParameters[0];
		//if (gp == null || gp.initialized == false)
			//return false;
		egStart();
		if (suki == null)
			return false;
		if (suki.Skeleton == null)
			return false;
		egStarted = true;

		return true;	
	
	}
	
	NetworkSkeletonOSC skeletonOSC = null;
	AvatarSkeleton avatarSkeleton = null;
	// Use this for initialization 
	void egAwake () {
		ph = ParameterHandler.Instance;
        	string playerID = PlayerPrefs.GetString(_LAST_USED_USER_NAME);
		GameParameters gp = (GameParameters)ParameterHandler.Instance.AllParameters[0];
		print("egBeginSession:playerID = "+ playerID);
//		Session session = SessionCreator.Instance.CurrentSession;
        	SessionCreator.Instance.SessionName = playerID;
        	SessionCreator.Instance.CreateSession();

		print ("egAwake");
		// initialize SUKI
		if (Skeleton == null)
			Skeleton = GameObject.Find ("Tracking Avatar").GetComponentInChildren<SkeletonData> ();
		avatarSkeleton = GameObject.Find ("Tracking Avatar").GetComponent<AvatarSkeleton> ();
		skeletonOSC = FindObjectOfType<NetworkSkeletonOSC>();
			
//		suki = SukiInput.Instance;
//		suki.Skeleton = Skeleton;
//		Debug.Log("suki.skel = "+ suki.Skeleton);

/*
		suki.Skeleton.roboticData = roboticData;
		if (roboticData==null)
			roboticData = new RoboticData();
		if (roboticData.data.Count==0){
			RoboticDatum rd = new RoboticDatum();
			rd.Value = 0f;
			roboticData.data["R1"]=rd;
		}

		suki.Skeleton.roboticData = roboticData;
*/		

		print ("egAwake:Trying to connect...");

		// connect the client skeleton to the server skeleton (running in the enablegames launcher app)
		string address = PlayerPrefs.GetString(egParameterStrings.LAUNCHER_ADDRESS);
		print ("Address= " + address);
		print ("egAwake:after connect.");


	}

	// Update is called once per frame
	void egUpdate () {
		if (!egInitialized())
			return;
		// Return to main menu
		
		if (Input.GetKeyDown(KeyCode.A))
		{
			roboticData.data["R1"].Value -= 1f;
			Debug.Log("Left key: " + suki.Skeleton.roboticData.data["R1"].Value);
		}


		if (Input.GetKeyDown(KeyCode.S))
		{
			roboticData.data["R1"].Value += 1f;
			Debug.Log("Right key: " + suki.Skeleton.roboticData.data["R1"].Value);
		}


		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//EndGame();
		}
	}
	private const string _LAST_USED_USER_NAME = "lastUsedUserName";

	void egBeginSession()
	{

		print("egBeginSession:Tracker");
		Tracker.Instance.BeginTracking ();
	}

	void egEndSession()
	{
		Tracker.Instance.Interrupt((int)egEvent.Type.CustomEvent, "GameEnd");
		Tracker.Instance.StopTracking(); //writes footer
	}

	float timeSinceLastLaneMove = 0f;
	/// <summary>
	/// Main game loop. Checks SUKI Input, updates game time, etc.
	/// </summary>
	private void egGetSukiInput()
	{
		if (!suki)
			return;
		//print ("egGetSukiInput-------------------");
		timeSinceLastLaneMove += Time.deltaTime;
		/*
			float duration = Time.time - startTime;
			if (duration >= GameLength)  //is game time over?
				showGameOverPanel ();
			timeSinceLastLaneMove += Time.deltaTime;
			*/

		//Get translated game input from SUKI
		// no-op if SUKI is not currently giving us input data

		/*NO LONGER NEED NETSKELETON TO KNOW IF CONNECTED..USES MOVEMENT FROM T-POSE INSTEAD (suki.Updating)
		//print("Game:FixedUpdate:" + suki.Updating);
		if (netskeleton && netskeleton.moving)
		{
			print ("netskel moving:" + suki.Skeleton.Moving);
			suki.Skeleton.moving = true;
			suki.Skeleton.resetMinMax = true;
		}
		*/
		if (!suki.Updating)
		{
				print("Game:suki not updating.");
			return;
		}
				print("Game:suki updating.");
//		return;
		///
		/// Read the various Suki inputs (depending on what suki file was loaded)
		/// Below contains examples for different types of input, including joint angles, bone positions, etc.
		/// 
		/// 
		// read the placement range input and move the cube
		//elbow angle suki schema profile is set as "placement", but probably should use better name.
		//In X-Z movement, can be used for Z-movement together with "joystick" for X
		if (suki.RangeExists("placement"))  
		{
			// we can use a range value as a placement to move left and right
			float range = suki.GetRange("placement");
			print("placement min = " + suki.GetExtentMin("placement"));
//			print("placement max = " + suki.GetExtentMax("placement"));
			// convert 0f to 1f to -1f to 1f
			float xPercent = (range * 2) - 1f;
//			print("placement mode:" + range + ":" + xPercent);
			// add a deadzone of +/- %
			float deadzone = 0.2f;
			if (xPercent > -deadzone && xPercent < deadzone)
			{
				xPercent = 0f;
			}
			// move the object
            Vector3 pos = PlayerObject.transform.localPosition;  //REPLACE PlayerObject with whatever object or vector you want to be updated
			pos.x = pos.x + (xPercent * Speed/40); // we use speed as a position scaler
			PlayerObject.transform.localPosition = pos;
			rb.velocity = transform.forward * speed;
			moving = true;
		}

		//shoulder profile is set as "joystick"
		//In X-Z movement, can be used for X-movement togther with "placement" for Z.
		if (suki.RangeExists("joystick"))
		{
			// we can use a range value as a placement to move left and right
			float range = suki.GetRange("joystick");
			print("joystick min = " + suki.GetExtentMin("joystick"));
			// convert 0f to 1f to -1f to 1f
			float xPercent = (range * 2) - 1f;
//			print("joysick mode:" + range + ":" + xPercent);
			// move the object
			float deadzone = 0.2f;
			if (xPercent > -deadzone && xPercent < deadzone)
			{
				xPercent = 0f;
			}

			Vector3 pos = PlayerObject.transform.localPosition; //REPLACE PlayerObject with whatever object or vector you want to be updated
			pos.y = pos.y + (xPercent * Speed/40); // we use speed as position scaler
			PlayerObject.transform.localPosition = pos;
		}

		//moving in discrete steps/lanes
		if (suki.SignalExists("moveLeft") && suki.SignalExists("moveRight"))
		{
			// we can use a pair of triggers to move left or move right
			bool moveLeft = suki.GetSignal("moveLeft");
			bool moveRight = suki.GetSignal("moveRight");
print("Moveleft= "+ moveLeft + ", Moverightt= "+ moveRight);
			Vector3 pos = PlayerObject.transform.localPosition;

			// only if there is a direction to move, and it's been some time since our last move
			// Instead of changing the speed of the movement here we change the pause between movements
			if ((!moveLeft && !moveRight) || (moveLeft && moveRight) || (timeSinceLastLaneMove < 1 / Speed)) // we use speed as a time scaler
			{
				return;
			}
			else if (moveLeft)
			{
				pos.x = (pos.x - 0.2f);
			}
			else if (moveRight)
			{
				pos.x = (pos.x + 0.2f);
			}
			PlayerObject.transform.localPosition = pos; //REPLACE PlayerObject with whatever object or vector you want to be updated
			timeSinceLastLaneMove = 0f;

		}
		//using foot or hand x-y position to control player position
		//You could also use each independently as Kollect does to control the hand/footprints.
		if (suki.Location2DExists ("leftfoot") || suki.Location2DExists ("rightfoot") || suki.Location2DExists ("lefthand") || suki.Location2DExists ("righthand")) {
			Vector2 fpos;
			if (suki.Location2DExists ("leftfoot"))
				fpos = suki.GetLocation2D ("leftfoot");
			else if (suki.Location2DExists ("rightfoot"))
				fpos = suki.GetLocation2D ("rightfoot");
			else if (suki.Location2DExists ("lefthand"))
				fpos = suki.GetLocation2D ("lefthand");
			else if (suki.Location2DExists ("righthand"))
				fpos = suki.GetLocation2D ("righthand");
			else
				fpos = new Vector2 ();
			print("fpos= "+ fpos);
			Vector3 pos = PlayerObject.transform.localPosition; //REPLACE PlayerObject with whatever object or vector you want to be updated
			// convert 0f to 1f to -1f to 1f
			float xPercent = (fpos.x * 2) - 1f;
			float yPercent = (fpos.y * 2) - 1f;
			float weight = 10f;
			pos.x = (pos.x * (weight-1) + (xPercent * Speed*4))/weight; // we use speed as position scaler
			pos.y = (pos.y * (weight-1) + (yPercent * Speed*4))/weight; // we use speed as position scaler
			//pos.x = pos.x + (fpos.x * Speed/40); // we use speed as position scaler
			//PlayerObject.transform.position = Vector3.Lerp(LeftFoot.transform.position, new Vector3(newX, newY, newZ), 1f);
			PlayerObject.transform.localPosition = pos;
		}
		checkRange ();
	}
	void checkRange()
	{
		float maxX=4f, maxY= 3f;
		Vector3 pos = PlayerObject.transform.localPosition;  //REPLACE PlayerObject with whatever object or vector you want to be updated
		if (pos.x> maxX)
			pos.x=maxX;
		if (pos.x< -maxX)
			pos.x= -maxX;
		if (pos.y> maxY)
			pos.y=maxY;
		if (pos.y< -maxY)
			pos.y= -maxY;
		PlayerObject.transform.localPosition = pos;

	}

	///
	/// END ENABLEGAMES REQUIRED CODE
	///////////////////////////////////////////////////////////////////////////////
	public void ToggleHand()
	{
		avatarSkeleton.handTracking = !avatarSkeleton.handTracking;
		skeletonOSC.handTracking = !skeletonOSC.handTracking;
	}
}
