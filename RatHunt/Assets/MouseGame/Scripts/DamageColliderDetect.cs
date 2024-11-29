using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliderDetect : MonoBehaviour
{
    [SerializeField] PlayerControllerX playerController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Dañado");
            playerController.damaged = true;
            GameManager.Instance.health -= 34;
        }
    }
}
