using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using TMPro;

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
    [HideInInspector] public bool verticalMovement = false;

    bool aiming;

    Rigidbody2D rb;

    // Spawnpoint for our heroine
    public GameObject spawnPoint;
    public GameObject checkPoint;

    SeedType selectedGrenade;

    [HideInInspector] public Seed currentThrowable = null;

    bool facingRight = true;

    float angle = 0;

    float fireCooldownEndTime = 0;

    const float fireCooldown = 0.2f;

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
            facingRight = false;
        }
        else if (directionalInput.x > 0.0f)
        {
            facingRight = true;
        }

        if (facingRight)
        {
            playerRenderer.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            playerRenderer.transform.localScale = new Vector3(-1, 1, 1);
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

        fireHeld = fireHeldNow;

        // Throw indicator movement
        aiming = false;

        float mult = facingRight ? 1 : -1;
        if (Input.GetKey(KeyCode.Q))
        {
            float newAngle = angle + mult * aimSensitivity * Time.deltaTime;

            if (directionalInput.x == 0.0f && (newAngle < -90 || newAngle > 90))
            {
                facingRight = !facingRight;
            }

            angle = Mathf.Clamp(newAngle, -90, 90);
            aiming = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            float newAngle = angle - mult * aimSensitivity * Time.deltaTime;

            if (directionalInput.x == 0.0f && (newAngle < -90 || newAngle > 90))
            {
                facingRight = !facingRight;
            }

            angle = Mathf.Clamp(newAngle, -90, 90);
            aiming = true;
        }

        Vector3 rot = throwRotator.eulerAngles;
        rot.z = facingRight ? angle : 180 - angle;
        throwRotator.eulerAngles = rot;
    }

    void OnGrenadeChanged(int grenadeType)
    {
        if (selectedGrenade == (SeedType)grenadeType)
        {
            return;
        }

        selectedGrenade = (SeedType)grenadeType;
        if (currentThrowable != null)
        {
            Destroy(currentThrowable.gameObject);
            currentThrowable = null;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.gameplayUI.HighlightSelectedGrenade((int)selectedGrenade);
        }
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

        if (fireDown && currentThrowable == null && Time.time > fireCooldownEndTime)
        {
            // Prepare throw
            currentThrowable = Instantiate(seedPrefabList[(int)selectedGrenade], throwPosition).GetComponent<Seed>();
            currentThrowable.sender = this;
            currentThrowable.rb2D.simulated = false;
            currentThrowable.GetComponent<Collider2D>().enabled = false;
        }

        if (fireUp && currentThrowable != null && currentThrowable.rb2D != null)
        {
            // Execute throw
            currentThrowable.rb2D.simulated = true;
            currentThrowable.GetComponent<Collider2D>().enabled = true;
            currentThrowable.transform.SetParent(null);
            currentThrowable.rb2D.AddForce(throwForce * (throwIndicator.transform.position - currentThrowable.transform.position).normalized, ForceMode2D.Impulse);
            currentThrowable = null;

            fireCooldownEndTime = Time.time + fireCooldown;
        }

        throwIndicator.sprite = aiming || fireHeld ? throwIndicatorSprite : null;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gameplayUI.DisplayAimInfo(aiming || fireHeld);
        }

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
