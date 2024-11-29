using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillColliderDetect : MonoBehaviour
{
    public bool enemyDying;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlatformCollider")
        {
            enemyDying = true;
        }
    }
}
