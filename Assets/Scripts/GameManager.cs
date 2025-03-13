using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance;

    public GameObject PlayerMovement;
    public GameObject TutorialEnemy;
    public GameObject Enemy;
    public SceneManager sceneManager;
    public Transform spawnPoint;
    public int currentLevel = 1;
    public bool isVictory = false;
    public bool isCredits = false;
    public bool isStartScreen = true;

    //Scene loading
    public void mainMenu()
    {
        sceneManager.LoadNextScene("mainMenu");
        Debug.Log("Start Screen has loaded.");
    }

    public void Outside()
    {
        sceneManager.LoadNextScene("finalExterior");
        Instantiate(PlayerMovement, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Outside has loaded");
    }

    public void Inside()
    {
        sceneManager.LoadNextScene("finalInterior");
        Instantiate(PlayerMovement, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Inside has loaded");
    }

    public void Attic()
    {
        sceneManager.LoadNextScene("finalAttic");
        Instantiate(PlayerMovement, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Attic has loaded");
    }

    public void HideWaypoints()
    {
        GameObject.Find("Waypoint").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Spawnpoint").transform.localScale = new Vector3(0, 0, 0);
        for (int i = 2; i < 7; i++)
        {
            GameObject.Find("Waypoint " + i).transform.localScale = new Vector3(0, 0, 0);
        }
    }

    //Possible death for player when attacked by enemy
    private bool isAlive;

    public static bool isGameOver = false;

    //Scene loading for when something happens. Will call on it
    public void GameOver()
    {
        if (isGameOver) 
        {
            sceneManager.LoadNextScene("gameOver");
        }
        //Show game over screen
        Debug.Log("Game Over!");
    }

    public void Credits()
    {
        if (isCredits) 
        {
            sceneManager.LoadNextScene("credits");
        }
        //Show credits screen
        Debug.Log("Credits Scene");
    }

    public void StartScreen()
    {
        if (isStartScreen) 
        {
            sceneManager.LoadNextScene("mainMenu");
        }
        //Show title screen
        Debug.Log("Start Scene");
    }

    public void Victory()
    {
        if (isVictory) return;
        {
            sceneManager.LoadNextScene("youWin");
        }
        Debug.Log("Final Scene");

        //Trying to figure out transitions for code. Waiting until more information
        /*void RestartGame()
        {
            SceneManager.LoadScene("Main Menu");
        }

        void LoadLevel2()
        {
            Debug.Log("Transitioning to Level Two");

            SceneManager.LoadScene("LevelTwo");
        }
        */


    }

    // Update for key buttons
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("Game has quit.");
        }
    }
}
