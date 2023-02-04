using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5.0f;

    Vector2 directionalInput;
    bool jump;
    bool fireHeld;
    bool fireUp;
    bool fireDown;
    int selectedGranede = 0;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        directionalInput.x = Input.GetAxis("Horizontal");
        directionalInput.y = Input.GetAxis("Vertical");
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
                OnGranedeChanged(i - 1);
            }
        }

        fireHeld |= fireHeldNow;
    }

    void OnGranedeChanged(int granedeType) 
    {
        selectedGranede = granedeType;

        // Change granede sprite etc.
    }

    void FixedUpdate()
    {
        Vector2 vel = rb.velocity;
        vel.x = directionalInput.x * speed;
        rb.velocity = vel;

        if (jump) 
        {
            // Try jump
        }

        if (fireDown) 
        {
            // Prepare throw
        }

        if (fireUp) 
        {
            // Execute throw
        }

        fireHeld = false;
        fireDown = false;
        fireUp = false;
    }
}
