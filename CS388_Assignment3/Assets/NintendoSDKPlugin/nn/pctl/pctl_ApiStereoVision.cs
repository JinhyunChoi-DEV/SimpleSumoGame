﻿/*--------------------------------------------------------------------------------*
  Copyright (C)Nintendo All rights reserved.

  These coded instructions, statements, and computer programs contain proprietary
  information of Nintendo and/or its licensed developers and are protected by
  national and international copyright laws. They may not be disclosed to third
  parties or copied or duplicated in any form, in whole or in part, without the
  prior written consent of Nintendo.

  The content herein is highly confidential and should be handled accordingly.
 *--------------------------------------------------------------------------------*/

#if UNITY_SWITCH_VRKIT && (UNITY_SWITCH || UNITY_EDITOR || NN_PLUGIN_ENABLE)
using System.Runtime.InteropServices;

namespace nn.pctl
{
    public static class ParentalControl
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public static bool CheckStereoVisionPermission(bool isShowUi) { return false; }
#else
        [DllImport(Nn.DllName,
        CallingConvention = CallingConvention.Cdecl,
        EntryPoint = "nn_pctl_CheckStereoVisionPermission")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CheckStereoVisionPermission(bool isShowUi);
#endif
    }
}

#endif