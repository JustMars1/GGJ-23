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

    public SpriteRenderer playerRenderer;

    public Animator animator;

    Vector2 directionalInput;
    bool jump;
    bool fireHeld;
    bool fireUp;
    bool fireDown;
    public bool verticalMovement = false;

    bool aiming;

    Rigidbody2D rb;

    // Spawnpoint for our heroine
    public GameObject spawnPoint;
    public GameObject checkPoint;

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

        if (directionalInput.x < 0.0f)
        {
            playerRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (directionalInput.x > 0.0f)
        {
            playerRenderer.transform.localScale = new Vector3(1, 1, 1);
        }

        if (directionalInput.x != 0.0f && isGrounded)
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
        aiming = false;
        if (Input.GetKey(KeyCode.Q))
        {
            throwRotator.transform.Rotate(Vector3.forward * aimSensitivity * Time.deltaTime);
            aiming = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            throwRotator.transform.Rotate(Vector3.back * aimSensitivity * Time.deltaTime);
            aiming = true;
        }
    }

    void OnGrenadeChanged(int grenadeType)
    {
        selectedGrenede = (SeedType)grenadeType;
        // Change granede sprite etc.
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(groundCheckPositionLeft.position, groundCheckRadius, groundCheckLayer) ||
            Physics2D.OverlapCircle(groundCheckPositionRight.position, groundCheckRadius, groundCheckLayer))
        {
            isGrounded = true;
            animator.SetBool("GoingDown", false);
        }
        else
        {
            isGrounded = false;
            if (rb.velocity.y > 0)
            {
                animator.SetBool("GoingUp", true);
            }
            if (rb.velocity.y < 0)
            {
                animator.SetBool("GoingUp", false);
                animator.SetBool("GoingDown", true);
            }
        }

        Vector2 vel = rb.velocity;
        vel.x = directionalInput.x * speed;
        rb.velocity = vel;

        if (verticalMovement)
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
            currentThrowable = Instantiate(seedPrefabList[(int)selectedGrenede], throwPosition).GetComponent<Rigidbody2D>();
            currentThrowable.simulated = false;
            currentThrowable.GetComponent<Collider2D>().enabled = false;
        }

        if (fireUp && currentThrowable != null)
        {
            // Execute throw
            currentThrowable.simulated = true;
            currentThrowable.GetComponent<Collider2D>().enabled = true;
            currentThrowable.transform.SetParent(null);
            currentThrowable.AddForce(throwForce * (throwIndicator.transform.position - currentThrowable.transform.position).normalized, ForceMode2D.Impulse);
            currentThrowable = null;
        }

        throwIndicator.sprite = aiming || fireHeld ? throwIndicatorSprite : null;

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

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            spawnPoint.transform.position = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vine"))
        {
            verticalMovement = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deathline"))
        {
            gameObject.transform.position = spawnPoint.transform.position;
        }
    }
}
