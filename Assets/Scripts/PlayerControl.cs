using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float throwForce = 5.0f;

    public bool isGrounded = true;
    public Transform groundCheckPositionLeft;
    public Transform groundCheckPositionRight;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    public Transform throwPosition;
    public GameObject[] seedList;
    public GameObject throwRotator;
    public GameObject throwIndicator;
    public Sprite throwIndicatorSprite;

    Vector2 directionalInput;
    bool jump;
    bool fireHeld;
    bool fireUp;
    bool fireDown;
    public bool verticalMovement = false;
    GameObject selectedGrenade;
    GameObject throwable;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        selectedGrenade = seedList[0];
    }

    void Update()
    {
        directionalInput.x = Input.GetAxisRaw("Horizontal");
        directionalInput.y = Input.GetAxisRaw("Vertical");
        jump |= Input.GetButtonDown("Jump");

        bool fireHeldNow = Input.GetButton("Fire1");

        if (fireHeld != fireHeldNow)
        {
            fireDown |= fireHeldNow;
            fireUp |= !fireHeldNow;
        }

        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetKeyDown(i.ToString())) 
            {
                OnGrenadeChanged(i - 1);
            }
        }

        fireHeld |= fireHeldNow;

        // Throw indicator movement
        if(Input.GetKey(KeyCode.Q))
        {
            throwRotator.transform.Rotate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            throwRotator.transform.Rotate(0, 0, -1);
        }

        if (fireDown)
        {
            // Prepare throw
            throwPosition.transform.GetComponent<SpriteRenderer>().sprite = selectedGrenade.transform.GetComponent<SpriteRenderer>().sprite;
            throwIndicator.transform.GetComponent<SpriteRenderer>().sprite = throwIndicatorSprite;
        }

        if (fireUp)
        {
            // Execute throw
            throwPosition.transform.GetComponent<SpriteRenderer>().sprite = null;
            throwIndicator.transform.GetComponent<SpriteRenderer>().sprite = null;
            throwable = Instantiate(selectedGrenade, throwPosition.position, Quaternion.identity);
            Rigidbody2D rbThrowable = throwable.transform.GetComponent<Rigidbody2D>();
            rbThrowable.AddForce(throwForce * (throwIndicator.transform.position - throwable.transform.position), ForceMode2D.Impulse);
        }

        fireHeld = false;
        fireDown = false;
        fireUp = false;
    }

    void OnGrenadeChanged(int grenadeType) 
    {
        selectedGrenade = seedList[grenadeType];

        // Change granede sprite etc.
    }

    void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(groundCheckPositionLeft.position, groundCheckRadius, groundCheckLayer) ||
            Physics2D.OverlapCircle(groundCheckPositionRight.position, groundCheckRadius, groundCheckLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        Vector2 vel = rb.velocity;
        vel.x = directionalInput.x * speed;
        rb.velocity = vel;

        if(verticalMovement)
        {
            vel.y = directionalInput.y * speed;
            rb.velocity = vel;
        }

        if (jump && isGrounded) 
        {
            // Try jump
            vel.y = jumpForce;
            rb.velocity = vel;
        }

        /*if (fireDown) 
        {
            // Prepare throw
            throwPosition.transform.GetComponent<SpriteRenderer>().sprite = selectedGrenade.transform.GetComponent<SpriteRenderer>().sprite;
            throwIndicator.transform.GetComponent<SpriteRenderer>().sprite = throwIndicatorSprite;
        }

        if (fireUp) 
        {
            // Execute throw
            throwPosition.transform.GetComponent<SpriteRenderer>().sprite = null;
            throwIndicator.transform.GetComponent<SpriteRenderer>().sprite = null;
            throwable = Instantiate(selectedGrenade, throwPosition.position, Quaternion.identity);
            Rigidbody2D rbThrowable = throwable.transform.GetComponent<Rigidbody2D>();
            rbThrowable.AddForce(throwForce * (throwIndicator.transform.position - throwable.transform.position), ForceMode2D.Impulse);
        }

        fireHeld = false;
        fireDown = false;
        fireUp = false;*/
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vine")
        {
            verticalMovement = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vine")
        {
            verticalMovement = false;
        }
    }
}
