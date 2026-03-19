using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{


    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Map1()
    {
        SceneManager.LoadScene("Exercise 1");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Exercise 2");
    }
    public void Map3()
    {
        SceneManager.LoadScene("Exercise 3");
    }
    public void Map5()
    {
        SceneManager.LoadScene("Map_5");
    }
    public void Map6()
    {
        SceneManager.LoadScene("Map_6");
    }
    public void Map7()
    {
        SceneManager.LoadScene("Map_7");
    }
    
    public void Collect()
    {
        SceneManager.LoadScene("GameScene2");
    }
    
    public void Parameters()
    {
        SceneManager.LoadScene("enAblegamesLibrary/Scenes/eag_MainMenu");
    }
}
