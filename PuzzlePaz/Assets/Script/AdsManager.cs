using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;

public class AdsManager : MonoBehaviour
{
    public InGameManager inGameManager;
    public GameObject loadingPopup;

    private void Start()
    {
        string tapsellAppKey = "nbftmjanbkossihlboslklodjmmeskbbrrrheanmaafcnglnhesmottobmkctttesgdtrd";
        Tapsell.initialize(tapsellAppKey);
    }

    public void showAd(string zoneID)
    {
        ShowTapsellVideoAd(zoneID);
    }

    public void ShowTapsellVideoAd(string zoneID)
    {
        Tapsell.requestAd(zoneID, false,
            (TapsellAd result) => //onAdAvailable
            {
                Debug.Log("Action: onAdAvailable");
                TapsellAd ad = result; // store this to show the ad later

                TapsellShowOptions showOptions = new TapsellShowOptions();
                showOptions.backDisabled = false;
                showOptions.immersiveMode = false;
                showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
                showOptions.showDialog = true;
                Tapsell.showAd(ad, showOptions);

                loadingPopup.SetActive(true);

                Tapsell.setRewardListener((TapsellAdFinishedResult rewardResult) =>
                {
                    if (rewardResult.completed && rewardResult.rewarded)
                    {
                        //todo: give the reward
                        inGameManager.ChangeHeartAmount(1);
                        inGameManager.ShowCurrencyPopup(1, 1);
                        loadingPopup.SetActive(false);
                    }
                    else
                    {
                        //todo: error popup
                        inGameManager.ShowMessageBox("متاسفانه خطایی در نمایش تبلیغ پیش آمد.");
                        loadingPopup.SetActive(false);
                    }
                });
            },
            (string zoneId) => //onNoAdAvailable
            {
                Debug.Log("No Ad Available");

                //todo: error popup
                inGameManager.ShowMessageBox("متاسفانه خطایی در نمایش تبلیغ پیش آمد.");
                loadingPopup.SetActive(false);
            },
            (TapsellError error) => //onError
            {
                Debug.Log(error.error);
                //todo: error popup
                inGameManager.ShowMessageBox("متاسفانه خطایی در نمایش تبلیغ پیش آمد.");
                loadingPopup.SetActive(false);
            },
            (string zoneId) => //onNoNetwork
            {
                Debug.Log("No Network");
                //todo: error popup
                inGameManager.ShowMessageBox("متاسفانه خطایی در نمایش تبلیغ پیش آمد.");
                loadingPopup.SetActive(false);
            },
            (TapsellAd result) => //onExpiring
            {
                Debug.Log("Expiring");
                // this ad is expired, you must download a new ad for this zone
                inGameManager.ShowMessageBox("متاسفانه خطایی در نمایش تبلیغ پیش آمد.");
                loadingPopup.SetActive(false);
            });
    }
}
