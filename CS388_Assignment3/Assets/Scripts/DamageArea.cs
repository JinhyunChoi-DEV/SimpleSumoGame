using UnityEngine;

public class DamageArea : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Player>().warningPanel.SetActive(true);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (!collider.GetComponent<Player>().warningPanel.activeSelf)
                collider.GetComponent<Player>().warningPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Player>().warningPanel.SetActive(false);
        }
    }


}
