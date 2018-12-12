using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowAndRate : MonoBehaviour
{

    public void OpenUrlWithUrl(string URL)
    {
        Application.OpenURL(URL);
    }

    //public void RateUs_btnOnClick()
    //{
    //    #if UNITY_ANDROID && !UNITY_EDITOR
    //        if (PlayerPrefs.GetString("TargetMarket") == "CafeBazaar")
    //        {
    //            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    //            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

    //            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

    //            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_EDIT"));
    //            intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse",
    //                "bazaar://details?id=com.acidgreengames.bingo"));
    //            intentObject.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");

    //            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
    //            currentActivity.Call("startActivity", intentObject);   
    //        }
    //        else if (PlayerPrefs.GetString("TargetMarket") == "Myket")
    //        {
    //            Application.OpenURL("myket://comment?id=com.acidgreengames.bingo");
    //        }
    //        else if (PlayerPrefs.GetString("TargetMarket") == "IranApps")
    //        {
    //            Application.OpenURL("iranapps://app/com.acidgreengames.bingo?a=comment&r=5");
    //        }
    //    #elif UNITY_IOS && !UNITY_EDITOR
    //        //Application.OpenURL("itms-apps://itunes.apple.com/app/");
    //        Application.OpenURL("https://bit.ly/2PlFbxq");
    //    #else
    //        print("Opened Rate page!");
    //    #endif
    //}

}
