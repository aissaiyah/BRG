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
    public TMP_Text score;
    public playerMovementScript playerMovement;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {
        playerMovement ??= FindObjectOfType<playerMovementScript>();

        if (score != null)
        {
            score.text = "Highscore: " + pacmanHighScore;
        }

        if (playerMovement != null && playerMovement.win)
        {
            if (score != null)
            {
                score.text = "YOU WIN! Your Score: " + pacmanHighScore;
                score.transform.position = new Vector3(450f, 300f, 0f);
            }
        }
        else if (playerMovement != null && playerMovement.lost)
        {
            if (score != null)
            {
                score.text = "GAME OVER! Your Score: " + pacmanHighScore;
                score.transform.position = new Vector3(450f, 300f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            pacmanHighScore = 0;
            if (playerMovement != null)
            {
                playerMovement.win = false;
                playerMovement.lost = false;
            }
            Time.timeScale = 1f;
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("LevelSelect");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Scenes/eag_MainMenu");
        }
    }
}
