using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Camera playerCamera;
    public SlapHitbox slap;
    public Slider gaugeSlider;
    public Slider hpSlider;
    public GameObject diePanel;
    public GameObject playablePanel;
    public GameObject warningPanel;
    public GameObject stunPanel;
   
    public bool CanMove;
    public bool Alive { get; private set; }
    public int idx { get; private set; }

    private float HP;
    public bool IsBot => idx == -1;
    private int stickClickCount;
    private int gauge;
    private float timer = 0.0f;
    private float warningdamageTimer = 0.0f;
    private float stunTimer = 0.0f;
    private bool isStun = false;
    private bool isAlreadySlap = false;
    private float slapTimer = 0.0f;


    public void SetIdx(int idx)
    {
        this.idx = idx;
    }

    public void GetHurt(int gauge, Vector3 dir)
    {
        float power;
        if (gauge == 1)
            power = 1;
        else
            power = gauge / 25.0f;

        isStun = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(-dir * 1500 * power, ForceMode.Acceleration);

        HP -= power;
    }

    public void GetHurtByDamage(int damage, Vector3 velocity)
    {
        isStun = true;
        rigidbody.velocity = Vector3.zero;
        var dir = velocity.normalized;
        rigidbody.AddForce(dir * 2500, ForceMode.Acceleration);

        HP -= damage;
    }

    public void Die()
    {
        HP = 0;
        Alive = false;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        diePanel.SetActive(true);
        playablePanel.SetActive(false);
        warningPanel.SetActive(false);
        stunPanel.SetActive(false);
    }

    private void Start()
    {
        HP = 100;
        Alive = true;
        gauge = 1;
        gaugeSlider.minValue = 1;
        gaugeSlider.maxValue = 100;
        gaugeSlider.value = gauge;
        hpSlider.minValue = 0;
        hpSlider.maxValue = 100;
        hpSlider.value = HP;
        CanMove = true;
        diePanel.SetActive(false);
        warningPanel.SetActive(false);
        stunPanel.SetActive(false);
        playablePanel.SetActive(true);

        SetCameraView();
    }

    void Update()
    {
        if (GameManager.Instance.IsFinishedGame)
        {
            playerCamera.enabled = false;
        }

        if (!Alive)
            return;

        if (warningPanel.activeSelf)
        {
            warningdamageTimer += Time.deltaTime;
            if (warningdamageTimer >= 3.0f)
            {
                HP -= 5;
                warningdamageTimer = 0.0f;
            }
        }
        else
        {
            warningdamageTimer = 0.0f;
        }

        if (isStun)
        {
            stunPanel.SetActive(true);
            stunTimer += Time.deltaTime;

            if (stunTimer >= 2.5f)
            {
                stunPanel.SetActive(false);
                isStun = false;
                stunTimer = 0.0f;
            }
        }


        if (IsBot)
        {
        }
        else
        {
            var input = InputManager.Instance.GetInput(idx);

            if (isStun && input.IsUseAxis)
            {
                isStun = false;
                stunTimer = 0.0f;
                stunPanel.SetActive(false);
            }

            if (CanMove && !isStun)
            {
                Slap(input);
                GaugeUpdate(input);
            }

            if(isAlreadySlap)
            {
                slapTimer += Time.deltaTime;
                if(slapTimer > 0.5f)
                {
                    slapTimer = 0.0f;
                    isAlreadySlap = false;
                }
            }
        }

        if (HP <= 0)
            Die();

        UpdateHP();
    }

    void FixedUpdate()
    {
        if (!Alive)
            return;

        if (IsBot)
        {
        }
        else
        {
            var input = InputManager.Instance.GetInput(idx);

            if (CanMove && !isStun)
                Move(input);
        }
    }

    private void Move(InputData data)
    {
        var move = data.Movement;
        var dir = transform.forward * move.z + transform.right * move.x;
        dir = dir.normalized;

        if (move.x == 0 && move.y == 0 && move.z < 0)
        {
            //DO NOT ROTATE
            rigidbody.velocity = dir * 8f;
        }
        else if (move != Vector3.zero)
        {
            var newRotation = Quaternion.LookRotation(dir);
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, newRotation, Time.fixedDeltaTime);
            rigidbody.velocity = dir * 8f;
        }
    }

    private void Slap(InputData data)
    {
        if (!data.Slap)
            return;

        if (isAlreadySlap)
            return;

        isAlreadySlap = true;
        slap.Active(gauge);
        gauge = 1;
    }

    private void GaugeUpdate(InputData data)
    {
        bool addGauge = false;
        if ((data.LBStick || data.RBStick))
        {
            stickClickCount++;

            if (gauge < 10)
            {
                if (stickClickCount > 1)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }
            else if (gauge < 30)
            {
                if (stickClickCount > 3)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }
            else if (gauge < 50)
            {
                if (stickClickCount > 5)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }
            else if (gauge < 70)
            {
                if (stickClickCount > 6)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }
            else if (gauge < 90)
            {
                if (stickClickCount > 7)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }
            else
            {
                if (stickClickCount > 8)
                {
                    addGauge = true;
                    stickClickCount = 0;
                }
            }

            if (addGauge)
                gauge += 5;

            if (gauge > 100)
                gauge = 100;

            timer = 0.0f;
        }
        else
        {

            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                gauge = 1;
                timer = 0.0f;
            }
        }

        gaugeSlider.value = gauge;
    }

    private void UpdateHP()
    {
        if (HP > 0)
            hpSlider.value = HP;

        if (HP < 0)
        {
            HP = 0;
            hpSlider.value = 0;
        }

    }

    private void SetCameraView()
    {
        if (IsBot)
            playerCamera.enabled = false;

        if ((InputManager.Instance.CurrentPlayer == 1 ) && !IsBot)
        {
            playerCamera.rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            if (idx == 0)
            {
                if (InputManager.Instance.CurrentPlayer == 2)
                {
                    playerCamera.rect = new Rect(0, 0, 0.5f, 1);
                }
                else
                {
                    playerCamera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
                }
            }
            else if (idx == 1)
            {
                if (InputManager.Instance.CurrentPlayer == 2)
                {
                    playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
                }
                else
                {
                    playerCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
            else if (idx == 2)
            {
                playerCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
            }
            else
            {
                playerCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }
        }
    }
}
