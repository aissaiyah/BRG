using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour
{
    public static GMScript Instance;
    public int pacmanHighScore;
    public TMP_Text score;// sets tmp text as codable
    public playerMovementScript playerMovement;
    // Start is called before the first frame update
    
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (Instance is not null &&  Instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement ??= FindObjectOfType<playerMovementScript>();
        //highscore = 
        score.text = "Highscore: " + pacmanHighScore;// change text to include numerical score in highscore

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (playerMovement.win)// if the game ended print high score and new position
        {
            pacmanHighScore = pacmanHighScore;
            score.text = "YOU WIN!!!! Your Highscore is: " + pacmanHighScore;
            score.transform.position = new Vector3(450f, 300f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.R))// if R is pressed reset game reset all values destroy this object
        {
            SceneManager.LoadScene("SampleScene");
            pacmanHighScore = 0;
            playerMovement.win = false;
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.L))// if R is pressed reset game reset all values destroy this object
        {
            SceneManager.LoadScene("LevelSelect");
        }
        if (Input.GetKeyDown(KeyCode.M))// if R is pressed reset game reset all values destroy this object
        {

            SceneManager.LoadScene("Scenes/eag_MainMenu");
        }

    }
}
