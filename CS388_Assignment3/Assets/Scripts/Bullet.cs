using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rigidbody;
    private float timer;

    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5.0f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().GetHurtByDamage(10, rigidbody.velocity);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }
}
