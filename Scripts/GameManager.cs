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
    public int currentLevel = 1;
    public bool isVictory = false;
    public bool isCredits = false;
    public bool isStartScreen = true;

    //Scene loading
    public void mainMenu()
    {
        SceneManager.LoadScene("StartScreen");
        Debug.Log("Start Screen has loaded.");
    }

    public void Outside()
    {
        SceneManager.LoadScene("Outside");
        Debug.Log("Outside has loaded");
    }

    public void Inside()
    {
        SceneManager.LoadScene("Inside");
        Debug.Log("Inside has loaded");
    }

    public void Attic()
    {
        SceneManager.LoadScene("Attic");
        Debug.Log("Attic has loaded");
    }

    public void HideWaypoints()
    {
        GameObject.Find("Waypoint").transform.localScale = new Vector3(0, 0, 0);
        for (int i = 2; i < 7; i++)
        {
            GameObject.Find("Waypoint " + i).transform.localScale = new Vector3(0, 0, 0);
        }
    }

    //Possible death for player when attacked by enemy
    private bool isAlive;

    public static bool isGameOver = false;

    //Scene loading for when something happens. Will call on it
    public static void GameOver()
    {
        if (isGameOver) return;
        {
            SceneManager.LoadScene("GameOver");
        }
        //Show game over screen
        Debug.Log("Game Over!");
    }

    public void Credits()
    {
        if (isCredits) return;
        {
            SceneManager.LoadScene("Credits");
        }
        //Show credits screen
        Debug.Log("Credits Scene");
    }

    public void StartScreen()
    {
        if (isStartScreen) return;
        {
            SceneManager.LoadScene("StartScreen");
        }
        //Show title screen
        Debug.Log("Start Scene");
    }

    public void Victory()
    {
        if (isVictory) return;
        {
            SceneManager.LoadScene("FinalScene");
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
