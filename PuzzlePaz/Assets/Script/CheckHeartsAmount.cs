using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeartsAmount : MonoBehaviour {

    public GameObject notEnoughHeartPopup;
    public InGameManager inGameManager;
    public NoLifeManager noLifeManager;

    public void onHeartClick()
    {
        if (PlayerPrefs.GetInt("ActiveHearts") == 0)
        {
            notEnoughHeartPopup.SetActive(true);
            noLifeManager.adButton.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("ActiveHearts") < inGameManager.maxHeart) 
        {
            notEnoughHeartPopup.SetActive(true);
            noLifeManager.adButton.SetActive(false);
        }
        else
        {
            Debug.Log("Has enough hearts.");
        }
    }
}
