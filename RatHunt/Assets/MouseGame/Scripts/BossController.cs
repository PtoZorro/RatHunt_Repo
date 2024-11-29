using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject enemySprite;
    [SerializeField] GameObject killColliderRef;
    [SerializeField] GameObject damageCollider;
    [SerializeField] GameObject player;
    PlayerControllerX playerController;
    SpriteRenderer enemyDirection;
    Rigidbody2D rb;
    BossKillColliderDetect killColDetect;
    Animator enemyAnimator;

    [Header("Enemy Stats")]
    [SerializeField] float speed;
    [SerializeField] int startingPoint;
    [SerializeField] Transform[] points;
    bool enemyDead;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerControllerX>();
        rb = GetComponent<Rigidbody2D>();
        killColDetect = killColliderRef.GetComponent<BossKillColliderDetect>();
        enemyDirection = enemySprite.GetComponent<SpriteRenderer>();
        enemyAnimator = enemySprite.GetComponent<Animator>();
        transform.position = points[startingPoint].position;
        enemyDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!killColDetect.enemyDying) { EnemyMov(); }
        else if (killColDetect.enemyDying && !enemyDead) { StartCoroutine(Death()); }
    }

    void EnemyMov()
    {
        if (!killColDetect.enemyDying && !enemyDead)
        {
            if (points[i].position.x < transform.position.x)
            {
                enemyDirection.flipX = true;
            }
            else if (points[i].position.x > transform.position.x)
            {
                enemyDirection.flipX = false;
            }

            if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
            {
                i++;

                if (i == points.Length)
                {
                    i = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    IEnumerator Death()
    {
        rb.isKinematic = true;
        killColliderRef.SetActive(false);
        damageCollider.SetActive(false);
        enemyDead = true;
        enemyAnimator.SetTrigger("death");
        AudioManager.Instance.PlaySFX(2);

        yield return null;
    }


}
