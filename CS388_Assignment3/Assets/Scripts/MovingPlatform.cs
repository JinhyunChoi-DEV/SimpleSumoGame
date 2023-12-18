using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public MeshCollider collider;

    private Transform destination;
    private float timer;
    private bool needToMove = true;
    private float ghostDistance;

    void Start()
    {
        destination = end;

        ghostDistance = Vector3.Distance(start.position, end.position) / 3.0f;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        if (!needToMove)
        {
            timer += Time.deltaTime;
            if (timer < 5.0f)
            {
                needToMove = false;
            }
            else
            {
                timer = 0;
                needToMove = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        if (!needToMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.fixedDeltaTime * 10);

        var distance = Vector3.Distance(transform.position, destination.position);

        if (distance <= 0.05f)
        {
            if (destination == end)
                destination = start;
            else
                destination = end;

            collider.enabled = true;
            needToMove = false;
        }
        else if (destination == start && distance <= ghostDistance)
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
    }
}
