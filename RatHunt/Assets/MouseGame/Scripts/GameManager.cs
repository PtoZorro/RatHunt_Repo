using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Inicio declaración Singleton
    private static GameManager instance; //Declaración estática de la base de datos
    public static GameManager Instance //Declaración de la llave que accede a los datos públicos de la base de datos
    { 
        get
        {
            if (instance == null)
            {
                Debug.Log("GameManager is null");
            }
            return instance;
        }
    }
    //Fin de la declaración de Singleton

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public int lives;
    public int maxlives;
    public int points;
    public int winPoints;
    public bool playerDead;
    public bool levelDone;

    [Header("SceneManagement")]
    private int randomScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        maxHealth = 100;
        maxlives = 3;
        health = maxHealth;
        lives = maxlives;
        points = 0;
        playerDead = false;
        levelDone = false;
    }

    private void Update()
    {
        HealthControl();
    }

    public void SetStartPoins(int pointsToWin)
    {
        points = 0;
        winPoints = pointsToWin;
    }

    public void HealthControl()
    {
        if (health <= 0)
        {
            health = 0;
            playerDead = true;
        }
    }

    #region SceneManager

    public void RestartLevel()
    {
        int actualScene = SceneManager.GetActiveScene().buildIndex;
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);
        playerDead = false;
        health = maxHealth;

        if (actualScene == 1)
        {
            points = 0;
        }
        else if (actualScene == 2)
        {
            points = 2;
        }
        else if(actualScene == 3)
        {
            points = 4;
        }
    }

    public void RestoreStats()
    {
        maxHealth = 100;
        maxlives = 3;
        health = maxHealth;
        lives = maxlives;
        points = 0;
        playerDead = false;
    }
    public void NextScene()
    {
        levelDone = false;
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadRandomScene(int sceneQuantity)
    {
        randomScene = Random.Range(1, sceneQuantity);
        SceneManager.LoadScene(randomScene);
    }

    public void ExitGame()
    {
        Debug.Log("Exit game is a succes");
        Application.Quit();
    }

    #endregion
}
