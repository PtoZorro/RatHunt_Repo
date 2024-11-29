using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            GameManager.Instance.points ++;
            AudioManager.Instance.PlaySFX(5);
            gameObject.SetActive(false);
        }
    }
}
