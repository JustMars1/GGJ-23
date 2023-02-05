using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoiint : MonoBehaviour
{
    Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayAnimation", true);
        }
    }
}
