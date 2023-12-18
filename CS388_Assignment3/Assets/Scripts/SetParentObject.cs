using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentObject : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
            return;

        collision.gameObject.transform.parent = gameObject.transform;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
            return;

        collision.gameObject.transform.parent = null;
    }
}
