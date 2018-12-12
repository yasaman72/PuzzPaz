using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoLifeManager : MonoBehaviour
{
    public GameObject shopPopup;
    public InGameManager inGameManager;
    public int requiredGem;


    public void onGemUse()
    {
        if (PlayerPrefs.GetInt("playerGems") >= requiredGem)
        {
            PlayerPrefs.SetInt("playerGems", PlayerPrefs.GetInt("playerGems") - requiredGem);
            inGameManager.ChangeHeartAmount(inGameManager.maxHeart);
        }
        else
        {
            shopPopup.SetActive(true);
        }
    }

    public void onAdWatch()
    {
        inGameManager.ChangeHeartAmount(1);
    }
}
