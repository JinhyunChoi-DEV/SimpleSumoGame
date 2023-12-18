using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for only Trio because the camera can't clear frame buffers
public class EmptyPlayer : MonoBehaviour
{
    public Camera cam;

    public void Spawn()
    {
        cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }
}
