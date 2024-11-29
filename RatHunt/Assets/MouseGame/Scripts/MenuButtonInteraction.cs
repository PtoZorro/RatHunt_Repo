using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonInteraction : MonoBehaviour
{
    public void ButtonStart()
    {
        AudioManager.Instance.PlayMusic(1);
        GameManager.Instance.LoadScene(1);
        GameManager.Instance.RestoreStats();
    }

    public void ButtonExit()
    {
        GameManager.Instance.ExitGame();
    }

    public void ButtonMenu()
    {
        GameManager.Instance.LoadScene(0);
        AudioManager.Instance.PlayMusic(0);
    }
}
