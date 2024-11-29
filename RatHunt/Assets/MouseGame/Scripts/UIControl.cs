using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] Image hpBar;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject lvlCompletePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Image[] lives;
    [SerializeField] Image[] pickUps;
    public bool levelDone;
    public bool gameOverDone;

    // Start is called before the first frame update
    void Start()
    {
        levelDone = false;
        gameOverDone = false;
        pauseMenu.SetActive(false);
        lvlCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarSystem();
        LivesSystem();
        PickUpSystem();

        if (!levelDone) { LevelDone(); }

        if (Input.GetKeyDown(KeyCode.Escape)) { PauseMenu(); }
    }

    public void HealthBarSystem()
    {

        hpBar.fillAmount = GameManager.Instance.health / GameManager.Instance.maxHealth;

    }

    public void LivesSystem()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i >= GameManager.Instance.lives)
            {
                lives[i].enabled = false;
            }
            else
            {
                lives[i].enabled = true;
            }
        }

        if(GameManager.Instance.lives <= 0 && !gameOverDone) { StartCoroutine(GameOverText()); }
    }

    public void PickUpSystem()
    {
        for (int i = 0; i < pickUps.Length; i++)
        {
            if (i >= GameManager.Instance.points)
            {
                pickUps[i].enabled = false;
            }
            else
            {
                pickUps[i].enabled = true;
            }
        }
    }

    public void PauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void LevelDone()
    {
        int actualScene = SceneManager.GetActiveScene().buildIndex;

        if ((actualScene == 1 && GameManager.Instance.points >= 2) || (actualScene == 2 && GameManager.Instance.points >= 4))
        {
            StartCoroutine(LevelCompletedText());
        }
    }

    IEnumerator LevelCompletedText()
    {
        int actualScene = SceneManager.GetActiveScene().buildIndex;
        levelDone = true;
        GameManager.Instance.levelDone = true;
        lvlCompletePanel.SetActive(true);
        AudioManager.Instance.PlaySFX(3);
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlayMusic(actualScene + 1);
        GameManager.Instance.NextScene();
    }

    IEnumerator GameOverText()
    {
        gameOverDone = true;
        gameOverPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(4);
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlayMusic(0);
        GameManager.Instance.LoadScene(0);
    }

    public void ButtonExit()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ExitGame();
    }

    public void ButtonMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayMusic(0);
        GameManager.Instance.LoadScene(0);
    }

    public void ButtonBack()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void ButtonRestartLvl()
    {
        Time.timeScale = 1f;
        int actualScene = SceneManager.GetActiveScene().buildIndex;

        if (actualScene == 1) { GameManager.Instance.points = 0; } else if (actualScene == 2) { GameManager.Instance.points = 2; } else if (actualScene == 3) { GameManager.Instance.points = 4; }

        GameManager.Instance.LoadScene(actualScene);
    }
}
