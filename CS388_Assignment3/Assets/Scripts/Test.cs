using nn.hid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    //public GameObject[] temp; 

    private NpadId[] npadIds = { NpadId.Handheld, NpadId.No1, NpadId.No2, NpadId.No3, NpadId.No4 };
    private NpadState npadState = new NpadState();
    private long[] preButtons;

    void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedIdType(npadIds);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);

        preButtons = new long[npadIds.Length];
    }

    void Update()
    {
        NpadButton onButtons = 0;

        for (int i = 0; i < npadIds.Length; i++)
        {
            NpadId npadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(npadId);
            if (npadStyle == NpadStyle.None)
            {
                //temp[i].gameObject.SetActive(false);
                continue;
            }

            Npad.GetState(ref npadState, npadId, npadStyle);

            if (npadState.GetButton(NpadButton.L) && npadState.GetButton(NpadButton.R))
            {
                SceneManager.LoadScene("Playground");
            }

            onButtons |= ((NpadButton)preButtons[i] ^ npadState.buttons) & npadState.buttons;
        }
    }
}
