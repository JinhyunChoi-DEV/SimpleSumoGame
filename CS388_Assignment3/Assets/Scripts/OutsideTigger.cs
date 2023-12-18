using UnityEngine;

public class OutsideTigger : MonoBehaviour
{
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Player>().Die();
        }
    }
}
