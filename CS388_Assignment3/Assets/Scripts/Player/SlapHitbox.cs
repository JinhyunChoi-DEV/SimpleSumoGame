using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapHitbox : MonoBehaviour
{
    public GameObject missSlap;
    public List<GameObject> collidingObjects { get; private set; }
    public Player player;

    public void Active(int gaugePower)
    {
        if (collidingObjects.Count > 0)
        {
            foreach (var target in collidingObjects)
            {
                target.GetComponent<Player>().GetHurt(gaugePower, -player.gameObject.transform.forward);
            }
        }
        else
        {
            StartCoroutine(MissedSlap());
        }
    }

    void Awake()
    {
        collidingObjects = new List<GameObject>();
    }

    void Start()
    {
        missSlap.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider == player.gameObject)
            return;

        if (collider.gameObject.tag == "Player")
        {
            collidingObjects.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        collidingObjects.Remove(collider.gameObject);
    }

    IEnumerator MissedSlap()
    {
        missSlap.SetActive(true);
        player.CanMove = false;
        yield return new WaitForSeconds(2.0f);
        missSlap.SetActive(false);
        player.CanMove = true;
    }
}
