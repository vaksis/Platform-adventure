/*
 * Playercontroller.cs 
 * 
 * player controller controls all the interactive things that player can witness
 * 
 * The script also features all the basic mechanics for the player
 * like movement, animation and all the stuff that player can interact
 * 
 * Date 4.1.2022
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //=========ANIMATION===========
    Animator m_Animator;

    //=========MOVEMENT===========
    public float speed;
    public float jumpforce;
    public float moveInput;
    private Rigidbody2D rb;

    private bool facingRight = true;


    // onko hahmo maassa?
    private bool isGrounded;
    // tarkistus onko maa jalkojen kohdalla
    public Transform groundCheck;
    // tarkistettavan alueen säde
    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    private int extraJumpValue = 2;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();

        //=========ANIMATION===========
        m_Animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // jos ollaan maassa niin nollataan hypyt:
        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            m_Animator.SetBool("Jump", true);
            rb.velocity = Vector2.up * jumpforce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpforce;
        }

    }

    void FixedUpdate()
    {
        //=========MOVEMENT===========
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // oikea nuoli = 1, vasen nuoli = -1
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (!facingRight && moveInput > 0)
        {
            // jos ei katsota oikeaan ja painettu oikealle
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            // tai jos katsotaan oikealle ja painettu vasemmalle
            Flip();
        }

        if (rb.transform.position.y < -10)
        {
            SceneManager.LoadScene("GameOver");
        }

        //=========ANIMATION===========
        if (moveInput < 0)
        {
            m_Animator.SetBool("Run_Left", true);
        }

        if (moveInput > 0)
        {
            m_Animator.SetBool("Run_Right", true);
        }

        if (moveInput == 0)
        {
            m_Animator.SetBool("Run_Left", false);
            m_Animator.SetBool("Run_Right", false);
        }

        //=========MOVEMENT===========
        if (isGrounded == true && rb.velocity.y == 0)
        {
            m_Animator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            m_Animator.Play("Base Layer.Jump", -1, 0f);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    //===========On collision enter 2D===========
    void OnTriggerEnter2D(Collider2D col)
    {
        //Loads new level when player collides with object tag "House"
        if (col.gameObject.tag == "House")
        {
            SceneManager.LoadScene("lvlClear");
        }

        if(col.gameObject.tag == "Spike")
        {
            SceneManager.LoadScene("GameOver");
        }

        if(col.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
