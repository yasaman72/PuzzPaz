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
            inGameManager.AddCurrency(-requiredCoin, 0);
            inGameManager.ChangeHeartAmount(inGameManager.maxHeart);
        }
        else
        {
            inGameManager.ShowMessageBox("سکه های شما کافی نمی باشد. می توانید از فروشگاه سکه تهیه کنید.");
            shopPopup.SetActive(true);
        }
    }

    public void onAdWatch()
    {
        adsManager.ShowTapsellVideoAd("5c1113d6a4973c000144cbc2");
    }
}
