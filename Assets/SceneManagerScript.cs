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
}
