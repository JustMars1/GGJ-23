using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraSizer : MonoBehaviour
{
    SpriteRenderer camArea;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;

        camArea = GetComponent<SpriteRenderer>();
        Vector3 pos = cam.transform.position;
        pos.x = camArea.transform.position.x;
        pos.y = camArea.transform.position.y;
        cam.transform.position = pos;
    }

    void Update()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        
        float targetRatio = camArea.bounds.size.x / camArea.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = camArea.bounds.size.y / 2;
        }
        else
        {
            float scale = targetRatio / screenRatio;
            cam.orthographicSize = (camArea.bounds.size.y / 2) * scale;
        }
    }
}
