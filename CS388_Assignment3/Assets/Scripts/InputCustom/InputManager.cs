using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using nn.hid;
using nn.util;
using System;
using static Cinemachine.CinemachineBlendDefinition;
using System.Xml;

public class InputData
{
    public Vector3 Movement;

    public bool LBStick;
    public bool RBStick;
    public bool Slap;
    public bool IsUseAxis;

    public InputData()
    {
        Movement = Vector3.zero;

        LBStick = false;
        RBStick = false;
        Slap = false;
    }
}

public class InputManager : MonoBehaviour
{
    public int CurrentPlayer { get; private set; }

    private static InputManager instance;

    private List<InputData> playerInputData;

    #region Switch Input Data
    ControllerSupportResultInfo pOutValue;
    private NpadId[] npadIds = { NpadId.No1, NpadId.No2, NpadId.No3, NpadId.No4 };
    private NpadState npadState = new NpadState();
    private const int maxPlayer = 4;
    private ControllerSupportArg controllerSupportArg = new ControllerSupportArg();
    private nn.Result result = new nn.Result();
    private SixAxisSensorHandle[] handle = new SixAxisSensorHandle[2];
    private SixAxisSensorState state = new SixAxisSensorState();

    #endregion

    public static InputManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType(typeof(InputManager)) as InputManager;

            return instance;
        }
    }

    public InputData GetInput(int idx)
    {
        return playerInputData[idx];
    }

    public void SetPlayerCountForDebug(int count)
    {
        if (count <= 0)
            CurrentPlayer = 1;
        else if (count > maxPlayer)
            CurrentPlayer = maxPlayer;
        else
            CurrentPlayer = count;
    }

    public void RefreshToGetPlayers()
    {
        if (Application.platform != RuntimePlatform.Switch)
        {
            var controllers = Input.GetJoystickNames();
            CurrentPlayer = controllers.Length + 1; // 1 is Keyboard+Mouse user as a default

        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        playerInputData = new List<InputData>();

        for (int i = 0; i < maxPlayer; ++i)
        {
            playerInputData.Add(new InputData());
        }
        CurrentPlayer = 1;
    }

    private void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedIdType(npadIds);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);
        Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Switch)
            UpdateSwitchInput();
        else
            UpdateOtherInput();
    }

    private void UpdateSwitchInput()
    {
        for (int i = 0; i < CurrentPlayer; i++)
        {
            NpadId npadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(npadId);

            var input = playerInputData[i];

            for (int j = 0; j< 2; ++j)
            {
                SixAxisSensor.GetState(ref state, handle[j]);
                input.IsUseAxis = VectorMagnitude(state.acceleration) > 2.0f;
            }

            Npad.GetState(ref npadState, npadId, npadStyle);
            input.LBStick = npadState.GetButtonDown(NpadButton.L);
            input.RBStick = npadState.GetButtonDown(NpadButton.R);
            input.Slap = npadState.GetButtonDown(NpadButton.A);
            input.Movement = new Vector3(npadState.analogStickL.fx, 0, npadState.analogStickL.fy);
        }
    }

    private void UpdateOtherInput()
    {
        for (int i = 0; i < CurrentPlayer; ++i)
        {
            var inputData = playerInputData[i];
            var playerName = string.Format("Player{0}_", (i + 1));
            var vertical = Input.GetAxis(playerName + "Left_Vertical");
            var horizontal = Input.GetAxis(playerName + "Left_Horizontal");

           
            inputData.Slap = Input.GetButtonDown(playerName + "Slap");
            inputData.LBStick = Input.GetButtonDown(playerName + "LB_Stick");
            inputData.RBStick = Input.GetButtonDown(playerName + "RB_Stick");
            inputData.Movement = new Vector3(horizontal, 0, vertical);
        }
    }

    private float VectorMagnitude(Float3 value)
    {
        value.x+= 0.98f;
        value.y+= 0.98f;
        value.z += 0.98f;
        return Mathf.Sqrt(value.x * value.x + value.y * value.y + value.z * value.z);
    }

    public void ShowControllerSupport()
    {
        controllerSupportArg.SetDefault();
        controllerSupportArg.playerCountMax = (byte)(npadIds.Length);

        controllerSupportArg.enableIdentificationColor = true;
        nn.util.Color4u8 color = new nn.util.Color4u8();
        color.Set(255, 128, 128, 255);
        controllerSupportArg.identificationColor[0] = color;
        color.Set(128, 128, 255, 255);
        controllerSupportArg.identificationColor[1] = color;
        color.Set(128, 255, 128, 255);
        controllerSupportArg.identificationColor[2] = color;
        color.Set(224, 224, 128, 255);
        controllerSupportArg.identificationColor[3] = color;

        controllerSupportArg.enableExplainText = true;
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Red", NpadId.No1);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Blue", NpadId.No2);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Green", NpadId.No3);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Yellow", NpadId.No4);

        Debug.Log(controllerSupportArg);
        result = ControllerSupport.Show(ref pOutValue, controllerSupportArg);

        
        if (!result.IsSuccess()) { Debug.Log(result); }

        CurrentPlayer = Convert.ToInt32(pOutValue.playerCount);
        UpdateSixAxisSensor();
    }

    private void UpdateSixAxisSensor()
    {
        for(int i = 0; i < CurrentPlayer; ++i)
        {
            var nPadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(nPadId);

            SixAxisSensor.GetHandles(handle, 2, nPadId, npadStyle);
            SixAxisSensor.Start(handle[i]);
        }
    }
}
