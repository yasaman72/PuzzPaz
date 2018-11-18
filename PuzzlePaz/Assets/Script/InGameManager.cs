﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameManager : MonoBehaviour
{

    public GameObject[] activateThese;
    public GameObject[] deactiveThese;
    public GameData gameData;
    public int initialCoin, initialGem;
    public PersianText coinAmountTxt, gemAmountTxt;
    public OfflineRewardManager offlineRewardManager;
    [Space]
    public Image[] heartSprites;
    public Sprite fullHeart, EmptyHeart;
    public int maxHeart;


    private void Start()
    {

        //setuping player initial currencies
        if (!PlayerPrefs.HasKey("alreadyPlyaed"))
        {
            PlayerPrefs.SetInt("alreadyPlyaed", 1);
            PlayerPrefs.SetInt("playerCoins", initialCoin);
            PlayerPrefs.SetInt("playerGems", initialGem);
            PlayerPrefs.SetInt("ActiveHearts", maxHeart);
        }

        coinAmountTxt._rawText = PlayerPrefs.GetInt("playerCoins").ToString();
        coinAmountTxt.enabled = false;
        coinAmountTxt.enabled = true;
        gemAmountTxt._rawText = PlayerPrefs.GetInt("playerGems").ToString();
        gemAmountTxt.enabled = false;
        gemAmountTxt.enabled = true;

        //adding more hearts based on time passed should be here
        ChangeHeartAmount(0);

        foreach (GameObject activateObj in activateThese)
        {
            activateObj.SetActive(true);
        }

        foreach (GameObject deactivateObj in deactiveThese)
        {
            deactivateObj.SetActive(false);
        }

    }

    public int GetMaxHeart()
    {
        return maxHeart;
    }

    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        gameData.SaveGame();
    }
    //private void OnApplicationPause(bool pause)
    //{
    //    gameData.SaveGame();
    //}

    public void ClearPlayerData()
    {
        PlayerPrefs.DeleteAll();
        gameData.ResetJSON();
        ResetCurrentScene();
    }

    public void AddCurrency(int addedCoinAmount, int addedGemAmount)
    {
        PlayerPrefs.SetInt("playerCoins", PlayerPrefs.GetInt("playerCoins") + addedCoinAmount);
        PlayerPrefs.SetInt("playerGems", PlayerPrefs.GetInt("playerGems") + addedGemAmount);

        coinAmountTxt._rawText = PlayerPrefs.GetInt("playerCoins").ToString();
        coinAmountTxt.enabled = false;
        coinAmountTxt.enabled = true;
        gemAmountTxt._rawText = PlayerPrefs.GetInt("playerGems").ToString();
        gemAmountTxt.enabled = false;
        gemAmountTxt.enabled = true;
    }

    public void ChangeHeartAmount(int heartAmount)
    {
        int currentHeart = PlayerPrefs.GetInt("ActiveHearts");
        Debug.Log("player hearts: " + PlayerPrefs.GetInt("ActiveHearts"));


        if (currentHeart + heartAmount >= maxHeart)
        {
            PlayerPrefs.SetInt("ActiveHearts", maxHeart);
        }
        else
        {
            PlayerPrefs.SetInt("ActiveHearts", PlayerPrefs.GetInt("ActiveHearts") + heartAmount);
        }
        Debug.Log("PLayer hearts: " + PlayerPrefs.GetInt("ActiveHearts"));

        for (int i = 0; i < heartSprites.Length; i++)
        {
            heartSprites[i].sprite = EmptyHeart;
            if (i < PlayerPrefs.GetInt("ActiveHearts"))
                heartSprites[i].sprite = fullHeart;
        }
    }

    public void CheckIfNeedHeart()
    {
        if (PlayerPrefs.GetInt("ActiveHearts") == maxHeart-1)
        {
            Debug.Log("Want some heart?");
            offlineRewardManager.StartCoroutine("RewardTimer", 0);
        }
    }
}
