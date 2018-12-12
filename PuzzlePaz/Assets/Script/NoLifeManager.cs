using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;
using UnityEngine.UI;

public class NoLifeManager : MonoBehaviour
{
    public GameObject shopPopup;
    public InGameManager inGameManager;
    public GameObject adButton;
    public AdsManager adsManager;
    public int requiredCoin;

    TapsellAd ad;

    public void onCoinUse()
    {
        if (PlayerPrefs.GetInt("playerCoins") >= requiredCoin)
        {
            PlayerPrefs.SetInt("playerCoins", PlayerPrefs.GetInt("playerCoins") - requiredCoin);
            inGameManager.ChangeHeartAmount(inGameManager.maxHeart);
        }
        else
        {
            shopPopup.SetActive(true);
        }
    }

    public void onAdWatch()
    {

        Tapsell.requestAd("5c1113d6a4973c000144cbc2", false,
    (TapsellAd result) =>
    {
        // onAdAvailable
        Debug.Log("Action: onAdAvailable");
        TapsellAd ad = result; // store this to show the ad later
    },

    (string zoneId) =>
    {
        // onNoAdAvailable
        Debug.Log("No Ad Available");
        adButton.GetComponent<Button>().enabled = false;
    },

    (TapsellError error) =>
    {
        // onError
        Debug.Log(error.error);
    },

    (string zoneId) =>
    {
        // onNoNetwork
        Debug.Log("No Network");
    },

    (TapsellAd result) =>
    {
        // onExpiring
        Debug.Log("Expiring");
        // this ad is expired, you must download a new ad for this zone
    }
    );

        TapsellShowOptions showOptions = new TapsellShowOptions();
        showOptions.backDisabled = false;
        showOptions.immersiveMode = false;
        showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
        showOptions.showDialog = true;
        Tapsell.showAd(ad, showOptions);


        Tapsell.setRewardListener((TapsellAdFinishedResult result) =>
        {
            // you may give rewards to user if result.completed and
            // result.rewarded are both true
            inGameManager.ChangeHeartAmount(1);
        }
        );

    }
}
