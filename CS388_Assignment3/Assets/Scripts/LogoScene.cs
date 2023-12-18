using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nn.hid;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    private NpadId[] npadIds = { NpadId.Handheld, NpadId.No1, NpadId.No2, NpadId.No3, NpadId.No4 };
    private NpadState npadState = new NpadState();
    private long[] preButtons;

    // Start is called before the first frame update
    void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedIdType(npadIds);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);

        preButtons = new long[npadIds.Length];
    }

    // Update is called once per frame

    void Update()
    {
        NpadButton onButtons = 0;

        for (int i = 0; i < npadIds.Length; i++)
        {
            NpadId npadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(npadId);
            if (npadStyle == NpadStyle.None) { continue; }

            Npad.GetState(ref npadState, npadId, npadStyle);

            if (npadState.GetButton(NpadButton.L) && npadState.GetButton(NpadButton.R))
            {
                SceneManager.LoadScene("Main");
            }

            onButtons |= ((NpadButton)preButtons[i] ^ npadState.buttons) & npadState.buttons;
        }

    }
}
