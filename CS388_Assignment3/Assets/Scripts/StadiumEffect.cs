using UnityEngine;

public class StadiumEffect : MonoBehaviour
{
    public Material baseM;
    public Material iceM;
    public PhysicMaterial material;
    public MeshRenderer render;
    public MeshCollider collider;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        timer += Time.deltaTime;
        if (timer >= 10.0f)
        {
            timer = 0.0f;

            var value = Random.Range(0.0f, 1.0f);

            if (value > 0.5f)
            {
                render.material = baseM;
                collider.material = null;
            }
            else
            {
                render.material = iceM;
                collider.material = material;
            }
        }
    }
}
