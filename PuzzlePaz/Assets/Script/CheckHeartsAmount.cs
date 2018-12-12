using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeartsAmount : MonoBehaviour {

    public GameObject notEnoughHeartPopup;

    public void onHeartClick()
    {
        if(PlayerPrefs.GetInt("ActiveHearts") == 0)
        {
            notEnoughHeartPopup.SetActive(true);
        }
        else
        {
            Debug.Log("Has enough hearts.");
        }
    }
}
