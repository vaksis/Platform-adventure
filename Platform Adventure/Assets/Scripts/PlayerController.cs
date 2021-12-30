using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
