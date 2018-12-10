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
}
