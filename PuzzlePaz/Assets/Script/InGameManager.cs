using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;

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
    public GameObject notEnoughHeartPopup;


    public GameObject crossSign;
    [Space]
    public GameObject gainedPopup;
    public Image gainedCurrencyImage;
    public PersianText gainedAmountText;
    public Sprite[] currenciesImages;
    [Space]
    public GameObject messagePopup;
    public PersianText messageTextObject;

    private void Start()
    {
        //GameAnalytics.Initialize();      
        
        //setuping player initial currencies
        if (!PlayerPrefs.HasKey("alreadyPlyaed"))
        {
            Debug.Log("first time playing");
            PlayerPrefs.SetInt("alreadyPlyaed", 1);

            PlayerPrefs.SetInt("playerCoins", initialCoin);
            //PlayerPrefs.SetInt("playerGems", initialGem);
            PlayerPrefs.SetInt("ActiveHearts", maxHeart);
            PlayerPrefs.SetInt("soundState", 1);
            //setting player's dimention
            //GameAnalytics.SetCustomDimension01("MainGroup");
            PlayerPrefs.Save();
        }

        //set currencies UI
        coinAmountTxt._rawText = PlayerPrefs.GetInt("playerCoins").ToString();
        coinAmountTxt.enabled = false;
        coinAmountTxt.enabled = true;
        //gemAmountTxt._rawText = PlayerPrefs.GetInt("playerGems").ToString();
        //gemAmountTxt.enabled = false;
        //gemAmountTxt.enabled = true;

        //change hearts visual
        ChangeHeartAmount(0);

        //change sound state
        if (PlayerPrefs.GetInt("soundState") == 1)
        {
            AudioListener.pause = false;
            crossSign.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("soundState") == 0)
        {
            AudioListener.pause = true;
            crossSign.SetActive(true);
        }

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
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        gameData.SaveGame();
    }

    public void ClearPlayerData()
    {
        PlayerPrefs.DeleteAll();
        //gameData.ResetJSON();
        ResetCurrentScene();
    }

    public void AddCurrency(int addedCoinAmount, int addedGemAmount)
    {
        PlayerPrefs.SetInt("playerCoins", PlayerPrefs.GetInt("playerCoins") + addedCoinAmount);
        //PlayerPrefs.SetInt("playerGems", PlayerPrefs.GetInt("playerGems") + addedGemAmount);

        coinAmountTxt._rawText = PlayerPrefs.GetInt("playerCoins").ToString();
        coinAmountTxt.enabled = false;
        coinAmountTxt.enabled = true;
        //gemAmountTxt._rawText = PlayerPrefs.GetInt("playerGems").ToString();
        //gemAmountTxt.enabled = false;
        //gemAmountTxt.enabled = true;
        PlayerPrefs.Save();
    }

    public void ShowCurrencyPopup(int amount, int type)
    {
        gainedPopup.SetActive(true);
        gainedAmountText.text = amount.ToString();
        gainedCurrencyImage.sprite = currenciesImages[type];
    }

    public void ShowMessageBox(string messageString)
    {
        messagePopup.SetActive(true);
        messageTextObject._rawText = messageString;
        messageTextObject.enabled = false;
        messageTextObject.enabled = true;
    }

    public void ChangeHeartAmount(int heartAmount)
    {
        int currentHeart = PlayerPrefs.GetInt("ActiveHearts");
        Debug.Log("player hearts: " + PlayerPrefs.GetInt("ActiveHearts"));


        if (currentHeart + heartAmount >= maxHeart)
        {
            PlayerPrefs.SetInt("ActiveHearts", maxHeart);
            offlineRewardManager.stopGivingHeart();
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
        PlayerPrefs.Save();
    }

    public void CheckIfNeedHeart()
    {
        if (PlayerPrefs.GetInt("ActiveHearts") == maxHeart - 1)
        {
            Debug.Log("Want some heart?");
            offlineRewardManager.StartCoroutine("RewardTimer", 0);
        }
    }
}
