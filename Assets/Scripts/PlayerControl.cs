using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float throwForce = 5.0f;
    public float aimSensitivity = 180.0f;

    public bool isGrounded = true;
    public Transform groundCheckPositionLeft;
    public Transform groundCheckPositionRight;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    public GameObject[] seedPrefabList;
    public Transform throwRotator;
    public Transform throwPosition;
    public SpriteRenderer throwIndicator;
    public Sprite throwIndicatorSprite;

    public Animator animator;

    Vector2 directionalInput;
    bool jump;
    bool fireHeld;
    bool fireUp;
    bool fireDown;
    public bool verticalMovement = false;

    Rigidbody2D rb;

    SeedType selectedGrenede;

    Rigidbody2D currentThrowable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        directionalInput.x = Input.GetAxisRaw("Horizontal");
        directionalInput.y = Input.GetAxisRaw("Vertical");
        jump |= Input.GetButtonDown("Jump");
        bool fireHeldNow = Input.GetButton("Fire1");

        if(directionalInput.x < 0.0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(directionalInput.x > 0.0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        if(directionalInput.x != 0.0f && isGrounded)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

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
            throwRotator.transform.Rotate(Vector3.forward * aimSensitivity * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            throwRotator.transform.Rotate(Vector3.back * aimSensitivity * Time.deltaTime);
        }
    }

    void OnGrenadeChanged(int grenadeType) 
    {
        selectedGrenede = (SeedType)grenadeType;
        // Change granede sprite etc.
    }

    void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(groundCheckPositionLeft.position, groundCheckRadius, groundCheckLayer) ||
            Physics2D.OverlapCircle(groundCheckPositionRight.position, groundCheckRadius, groundCheckLayer))
        {
            isGrounded = true;
            animator.SetBool("GoingDown", false);
        }
        else
        {
            isGrounded = false;
            if(rb.velocity.y > 0)
            {
                animator.SetBool("GoingUp", true);
            }
            if(rb.velocity.y < 0)
            {
                animator.SetBool("GoingUp", false);
                animator.SetBool("GoingDown", true);
            }
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

        if (fireDown && currentThrowable == null) 
        {
            // Prepare throw
            throwIndicator.sprite = throwIndicatorSprite;
            currentThrowable = Instantiate(seedPrefabList[(int)selectedGrenede], throwPosition).GetComponent<Rigidbody2D>();
            currentThrowable.simulated = false;
            currentThrowable.GetComponent<Collider2D>().enabled = false;
        }

        if (fireUp && currentThrowable != null) 
        {
            // Execute throw
            throwIndicator.sprite = null;

            currentThrowable.simulated = true;
            currentThrowable.GetComponent<Collider2D>().enabled = true;
            currentThrowable.transform.SetParent(null);
            currentThrowable.AddForce(throwForce * (throwIndicator.transform.position - currentThrowable.transform.position).normalized, ForceMode2D.Impulse);
            currentThrowable = null;
        }

        fireHeld = false;
        fireDown = false;
        fireUp = false;
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vine"))
        {
            verticalMovement = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vine"))
        {
            verticalMovement = false;
        }
    }

}
