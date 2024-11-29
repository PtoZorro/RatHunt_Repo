using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [Header("Variables de referencia")]
    [SerializeField] GameObject normalSpriteRef;
    [SerializeField] GameObject climbRSpriteRef;
    [SerializeField] GameObject climbLSpriteRef;
    [SerializeField] GameObject groundCheckRef;
    [SerializeField] GameObject climbRCheckRef;
    [SerializeField] GameObject climbLCheckRef;
    Rigidbody2D playerRb;
    Collider2D playerCol;
    Animator normalAnimPlayer;
    Animator climbRAnimPlayer;
    Animator climbLAnimPlayer;
    SpriteRenderer normalSprite;
    SpriteRenderer climbRSprite;
    SpriteRenderer climbLSprite;
    GroundCheckDetect groundCheck;
    RClimbCheckDetect climbCheckR;
    LClimbCheckDetect climbCheckL;

    private float horizontalInput;
    private float verticalInput;
    bool jumpClimbedDone;

    [Header("PlayerStats")]
    public float speed;
    public float jumpForce;
    public bool killJump;
    public bool damaged;
    bool justDying;
    bool enemyJumpRunning;

    // Start is called before the first frame update
    void Start()
    {
        //Primero se referencian objetos, luego a componentes
        normalSprite = normalSpriteRef.GetComponent<SpriteRenderer>();
        climbRSprite = climbRSpriteRef.GetComponent<SpriteRenderer>();
        climbLSprite = climbLSpriteRef.GetComponent<SpriteRenderer>();
        normalAnimPlayer = normalSpriteRef.GetComponent<Animator>();
        climbRAnimPlayer = climbRSpriteRef.GetComponent<Animator>();
        climbLAnimPlayer = climbLSpriteRef.GetComponent<Animator>();
        groundCheck = groundCheckRef.GetComponent<GroundCheckDetect>();
        climbCheckR = climbRCheckRef.GetComponent<RClimbCheckDetect>();
        climbCheckL = climbLCheckRef.GetComponent<LClimbCheckDetect>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();

        killJump = false;
        damaged = false;
        justDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.playerDead)
        {
            Movement();
            Jump();
        }
        Dying();
    }

    void Movement()
    {
        if (!enemyJumpRunning) { groundCheckRef.SetActive(!climbCheckL.isClimbingL && !climbCheckR.isClimbingR); }

        if (!climbCheckR.isClimbingR && !climbCheckL.isClimbingL)
        {
            normalSpriteRef.SetActive(true);
            climbRSpriteRef.SetActive(false);
            climbLSpriteRef.SetActive(false);
            if (!jumpClimbedDone) { climbRCheckRef.SetActive(true); climbLCheckRef.SetActive(true); }
            horizontalInput = Input.GetAxis("Horizontal");
            playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);

            if (horizontalInput > 0)
            {
                normalSprite.flipX = false;
                normalAnimPlayer.SetBool("isWalking", true);
            }
            if (horizontalInput < 0)
            {
                normalSprite.flipX = true;
                normalAnimPlayer.SetBool("isWalking", true);
            }
            if (horizontalInput == 0)
            {
                normalAnimPlayer.SetBool("isWalking", false);
            }
        }
        else if (climbCheckR.isClimbingR || climbCheckL.isClimbingL)
        {
            GameObject tempGameObject = null;
            Animator tempAnimPlayer = null;
            SpriteRenderer tempSprite = null;

            //Carga El Sprite adecuado en variables temporales según si escalamos por la derecha o la izquierda
            if (climbCheckR.isClimbingR) 
            {
                climbLCheckRef.SetActive(false);
                tempGameObject = climbRSpriteRef;
                tempSprite = climbRSprite;
                tempAnimPlayer = climbRAnimPlayer;
            }
            else if (climbCheckL.isClimbingL) 
            {
                climbRCheckRef.SetActive(false);
                tempGameObject = climbLSpriteRef;
                tempSprite = climbLSprite;
                tempAnimPlayer = climbLAnimPlayer;
            }

            tempGameObject.SetActive(true);
            normalSpriteRef.SetActive(false);
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            if (climbCheckR.isClimbingR)
            {
                if (horizontalInput > 0)
                {
                    horizontalInput = 0;
                }
                playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
            }
            else if (climbCheckL.isClimbingL)
            {
                if (horizontalInput < 0)
                {
                    horizontalInput = 0;
                }
                playerRb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
            }

            if (verticalInput > 0)
            {
                tempSprite.flipY = false;
                tempAnimPlayer.SetBool("isWalking", true);
            }
            if (verticalInput < 0)
            {
                tempSprite.flipY = true;
                tempAnimPlayer.SetBool("isWalking", true);
            }
            if (verticalInput == 0)
            {
                tempAnimPlayer.SetBool("isWalking", false);
            }
        }
    }

    void Jump()
    {
        //Cuando esté en el aire hace animación de Jump siempre
        normalAnimPlayer.SetBool("isJumping", !groundCheck.isGrounded); 

        if (Input.GetKeyDown(KeyCode.Space)) //|| killjump)
        {
            if ((groundCheck.isGrounded && !climbCheckR.isClimbingR && !climbCheckL.isClimbingL)) //|| killjump)
            {
                normalSpriteRef.SetActive(true);
                climbRSpriteRef.SetActive(false);
                climbLSpriteRef.SetActive(false);
                AudioManager.Instance.PlaySFX(0);
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                killJump = false;
            }
            else if (climbCheckR.isClimbingR || climbCheckL.isClimbingL)
            {
                StartCoroutine(JumpClimbed());
            }
        }

        if (killJump || damaged) { StartCoroutine(EnemyInteractJump()); }
    }

    IEnumerator JumpClimbed()
    {
        jumpClimbedDone = true;
        Vector2 tempVector2 = Vector2.zero;

        if (climbCheckR.isClimbingR)
        {
            tempVector2 = Vector2.left;
            climbRCheckRef.SetActive(false);
        }
        else if (climbCheckL.isClimbingL)
        {
            tempVector2 = Vector2.right;
            climbLCheckRef.SetActive(false);
        }

        normalSpriteRef.SetActive(true);
        climbRSpriteRef.SetActive(false);
        climbLSpriteRef.SetActive(false);

        AudioManager.Instance.PlaySFX(0);
        playerRb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        //playerRb.AddForce(tempVector2 * jumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.2f);

        climbRCheckRef.SetActive(true);
        climbLCheckRef.SetActive(true);
        jumpClimbedDone = false;
    }

    IEnumerator EnemyInteractJump()
    {
        enemyJumpRunning = true;
        if (damaged)
        {
            normalAnimPlayer.SetTrigger("hurt");
        }
        killJump = false;
        damaged = false;
        normalSpriteRef.SetActive(true);
        climbRSpriteRef.SetActive(false);
        climbLSpriteRef.SetActive(false);
        groundCheckRef.SetActive(false);
        AudioManager.Instance.PlaySFX(1);
        playerRb.AddForce(Vector2.up * 35, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.5f);
        groundCheckRef.SetActive(true);
        enemyJumpRunning = true;
        yield return new WaitForSeconds(.5f);
    }

    private void Dying()
    {
        if (GameManager.Instance.playerDead && !justDying)
        {
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        justDying = true;
        normalSpriteRef.SetActive(true);
        climbRSpriteRef.SetActive(false);
        climbLSpriteRef.SetActive(false);
        normalAnimPlayer.SetTrigger("dead");
        AudioManager.Instance.PlaySFX(1);
        GameManager.Instance.lives -= 1;

        yield return new WaitForSeconds(3f);

        if (GameManager.Instance.lives > 0 ) { GameManager.Instance.RestartLevel(); }

        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FinalPickUp"))
        {
            if (GameManager.Instance.points >= 6)
            {
                AudioManager.Instance.PlayMusic(0);
                GameManager.Instance.LoadScene(4);
            }
        }
    }
}
