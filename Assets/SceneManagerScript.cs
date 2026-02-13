using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Map1()
    {
        SceneManager.LoadScene("Map_2");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Map_3");
    }
    public void Map3()
    {
        SceneManager.LoadScene("Map_4");
    }
}
