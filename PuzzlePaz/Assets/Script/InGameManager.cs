using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameManager : MonoBehaviour {

    public GameObject[] activateThese;
    public GameObject[] deactiveThese;
    public GameData gameData;
    public int initialCoin, initialGem;
    public PersianText coinAmountTxt, gemAmountTxt;

    private void Start()
    {
        //setuping player initial currencies
        if (!PlayerPrefs.HasKey("alreadyPlyaed"))
        {
            PlayerPrefs.SetInt("alreadyPlyaed", 1);
            PlayerPrefs.SetInt("playerCoins", initialCoin);
            PlayerPrefs.SetInt("playerGems", initialGem);
        }

        coinAmountTxt._rawText = PlayerPrefs.GetInt("playerCoins").ToString();
        coinAmountTxt.enabled = false;
        coinAmountTxt.enabled = true;
        gemAmountTxt._rawText = PlayerPrefs.GetInt("playerGems").ToString();
        gemAmountTxt.enabled = false;
        gemAmountTxt.enabled = true;
             


        foreach (GameObject activateObj in activateThese)
        {
            activateObj.SetActive(true);
        }

        foreach (GameObject deactivateObj in deactiveThese)
        {
            deactivateObj.SetActive(false);
        }

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
}
