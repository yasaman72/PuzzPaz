using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoLifeManager : MonoBehaviour
{
    public GameObject shopPopup;
    public InGameManager inGameManager;
    public GameObject adButton;
    public AdsManager adsManager;
    public int requiredCoin;

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
        AdsManager.ShowTapsellVideoAd("5c1113d6a4973c000144cbc2");
    }
}
