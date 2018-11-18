using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineRewardManager : MonoBehaviour
{
    [Tooltip("don't change this manualy!")]
    public int RewardIntervalMinute;
    public InGameManager inGameManager;

    public Text heartTimerTxt;

    public int secondsPassedSinceGameStarted;
    private int minutesToNextReward;

    System.DateTime startTime;
    System.DateTime exitTime;

    int minutesPassed;

    private void Start()
    {
        startTime = System.DateTime.Now;
        //Debug.Log("Application started at " + startTime);

        if (PlayerPrefs.HasKey("exitTime"))
        {
            minutesPassed = ((DateTimeToUnixTimestamp(startTime) - PlayerPrefs.GetInt("exitTime")));
            Debug.Log("Application minutes passed: " + minutesPassed);

            CalculateOfflineRewards();
        }
        else
        {
            Debug.Log("no data on exit time");
        }


    }

    private void OnApplicationQuit()
    {
        exitTime = System.DateTime.Now;
        PlayerPrefs.SetInt("exitTime", DateTimeToUnixTimestamp(exitTime));
        //Debug.Log("Application ended at " + DateTimeToUnixTimestamp(exitTime) + " minutes long time.");
        PlayerPrefs.SetInt("rewardSecondsAlreadyPassed", ((RewardIntervalMinute - minutesToNextReward) * 60) + secondsPassedSinceGameStarted);
    }

    public static int DateTimeToUnixTimestamp(System.DateTime dateTime)
    {
        return (int)(dateTime - new System.DateTime(1970, 1, 1)).TotalMinutes;
    }

    private void CalculateOfflineRewards()
    {
        int allSecondsPassed = minutesPassed * 60 + PlayerPrefs.GetInt("rewardSecondsAlreadyPassed");
        minutesPassed += Mathf.FloorToInt(PlayerPrefs.GetInt("rewardSecondsAlreadyPassed") / 60);

        if (minutesPassed >= RewardIntervalMinute)
        {
            inGameManager.ChangeHeartAmount(Mathf.FloorToInt(minutesPassed / RewardIntervalMinute));
            Debug.Log("hearts to add to player: " + Mathf.FloorToInt(minutesPassed / RewardIntervalMinute));
        }
        else
        {
            Debug.Log("Not enough time passed.");
        }       

        if (PlayerPrefs.GetInt("ActiveHearts") < inGameManager.GetMaxHeart())
            StartCoroutine("RewardTimer", allSecondsPassed % (RewardIntervalMinute*60));
    }


    public IEnumerator RewardTimer(int secondsPassedForNextReward)
    {
        minutesToNextReward = RewardIntervalMinute - Mathf.FloorToInt(secondsPassedForNextReward / 60);
        secondsPassedSinceGameStarted = secondsPassedForNextReward % 60;
        secondsPassedSinceGameStarted = 60 - secondsPassedSinceGameStarted;

        Debug.Log("minutes to next reward: " + minutesToNextReward + " seconds to next reward: " + secondsPassedSinceGameStarted);
        while (true)
        {
            secondsPassedSinceGameStarted++;
            if (secondsPassedSinceGameStarted >= 60)
            {
                minutesToNextReward--;
                secondsPassedSinceGameStarted = 0;

                if (minutesToNextReward <= 0)
                {
                    Debug.Log("Time for a reward!");
                    inGameManager.ChangeHeartAmount(1);

                    if (PlayerPrefs.GetInt("ActiveHearts") >= inGameManager.GetMaxHeart())
                    {
                        heartTimerTxt.text = "";
                        Debug.Log("Has enough hearts!");
                        StopCoroutine("RewardTimer");
                        yield return null;
                    }

                    minutesToNextReward = RewardIntervalMinute;
                }
            }
            heartTimerTxt.text = minutesToNextReward.ToString() + " : " + (59 - secondsPassedSinceGameStarted).ToString();

            yield return new WaitForSeconds(1);
        }
    }
}
