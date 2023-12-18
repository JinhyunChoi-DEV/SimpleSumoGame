using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject towerObject;
    public GameObject startPoint;
    public Bullet bullet;
    public bool longTime;

    private float timer = 0.0f;
    private float randomTargetTime;

    void Start()
    {
        bullet.gameObject.SetActive(false);
        randomTargetTime = -1.0f;
        timer = 0.0f;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        if (randomTargetTime < 0)
        {
            if (longTime)
                randomTargetTime = Random.Range(10.0f, 25.0f);
            else
                randomTargetTime = Random.Range(8.0f, 15.0f);
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= randomTargetTime)
            {
                timer = 0.0f;
                randomTargetTime = -1;
                var newBullet = Instantiate(bullet, startPoint.transform.position, Quaternion.identity);
                newBullet.gameObject.SetActive(true);
                var dir = towerObject.transform.forward;
                newBullet.GetComponent<Rigidbody>().AddForce(dir * 5000, ForceMode.Acceleration);
            }
        }
    }
}
