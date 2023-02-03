using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    PlatformSeed, VineSeed, BrigdeSeed
}

public class Seed : MonoBehaviour
{
    public float timer = 3f;
    public SeedType seedType;
    public GameObject platform;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            switch (seedType)
            {
                case SeedType.PlatformSeed:
                    // Makes a platform
                    break;
                case SeedType.VineSeed:
                    // Makes ladder vine
                    break;
                case SeedType.BrigdeSeed:
                    // Makes a connection between two bridge seeds
                    break;
                default:
                    break;
            }
        }
    }

    void platfrom()
    {
        Instantiate(platform, transform);
    }

    void ladder()
    {

    }

    void bridge()
    {

    }
}
