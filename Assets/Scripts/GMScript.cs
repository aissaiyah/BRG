using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour
{
    public TMP_Text score;// sets tmp text as codable
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Highscore: " + playerMovementScript.pelletCount;// change text to include numerical score in highscore

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (playerMovementScript.win)// if the game ended print high score and new position
        {
            score.text = "Your Highscore is: " + playerMovementScript.pelletCount;
            score.transform.position = new Vector3(450f, 300f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.R))// if R is pressed reset game reset all values destroy this object
        {
            SceneManager.LoadScene("SampleScene");
            playerMovementScript.pelletCount = 0;
            playerMovementScript.win = false;
            Destroy(gameObject);
        }

    }
}
