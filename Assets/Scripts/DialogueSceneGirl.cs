using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueSceneGirl : MonoBehaviour
{
    public GameObject textBox;
    public Animator animator;
    public float speed;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Run", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!textBox.activeInHierarchy)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            animator.SetBool("Run", true);
            transform.localScale = new Vector3(1, 1, 1);
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
