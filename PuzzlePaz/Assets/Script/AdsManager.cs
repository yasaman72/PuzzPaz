using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;

public class AdsManager : MonoBehaviour
{
    private void Start()
    {
        string tapsellAppKey = "";
        Tapsell.initialize(tapsellAppKey);
    }

    public void ShowTapsellVideoAd(string zoneID)
    {
        Tapsell.requestAd(zoneID, false,
            (TapsellAd result) => //onAdAvailable
            {
                Debug.Log("Action: onAdAvailable");
                TapsellAd ad = result; // store this to show the ad later
            },
            (string zoneId) => //onNoAdAvailable
            {
                Debug.Log("No Ad Available");
            },
            (TapsellError error) => //onError
            {
                Debug.Log(error.error);
            },
            (string zoneId) => //onNoNetwork
            {
                Debug.Log("No Network");
            },
            (TapsellAd result) => //onExpiring
            {
                Debug.Log("Expiring");
                // this ad is expired, you must download a new ad for this zone
            }
        );
    }
}
