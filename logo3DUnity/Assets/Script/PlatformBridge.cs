using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlatformBridge
{
    //[DllImport("__Internal")]
    //public static extern bool GetIsPC();


    //[DllImport("__Internal")]
    //public static extern void PlayVideo(string path);

    //[DllImport("__Internal")]
    //public static extern void ShowStartToFullScreen();


    [DllImport("__Internal")]
    public static extern void UnityMessage(string msg);
}
