using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueSceneGirl : MonoBehaviour
{
    public GameObject textBox;
    public Animator animator;
    public Animator rootAnimator;
    public float speed;
    public bool pushed;
    public float pushUp;
    public TextBoxManager tbManager;

    public Transform seedSpawner;
    public GameObject[] seeds;

    public GameObject rootSpawn;

    Rigidbody2D rb;
    bool treeReached = false;
    bool seeded = false;
    bool destroyed = false;

    List<GameObject> newSeedList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!treeReached)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position += new Vector3(-1 *speed * Time.deltaTime, 0, 0);
            animator.SetBool("Run", true);
        }

        if(tbManager.currentLine == 20 && !seeded)
        {
            Rigidbody2D seedRB = null;
            for(int i = 0; i < 3; i++)
            {
                GameObject seed = Instantiate(seeds[i], seedSpawner);
                newSeedList.Add(seed);
                seed.GetComponent<Seed>().enabled = false; 
                seedRB = seed.GetComponent<Rigidbody2D>();
                seedRB.AddForce(new Vector2(5, 2f * i), ForceMode2D.Impulse);
            }

            seeded = true;
        }

        if(tbManager.currentLine == 23 && !destroyed)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(newSeedList[i]);
            }

            destroyed = true;
        }


        if(!textBox.activeInHierarchy && treeReached)
        {
            animator.SetBool("Run", true);
            rootAnimator.SetBool("Grow", true);
            transform.localScale = new Vector3(1, 1, 1);
            if (!pushed)
            {
                rb.AddForce(transform.up * pushUp, ForceMode2D.Impulse);
                pushed = true;
            }
            transform.position += new Vector3(speed * 3 * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tree"))
        {
            treeReached = true;
            animator.SetBool("Run", false);
            tbManager.EnableTextBox();
        }
    }
}
