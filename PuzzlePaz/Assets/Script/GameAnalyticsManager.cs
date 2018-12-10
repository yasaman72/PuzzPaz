using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameAnalyticsManager : MonoBehaviour
{

    public static GameAnalyticsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <param name="progresionStatus">0: undefined, 1: Start, 2: Complete, 3: Fail</param>
    /// <param name="worldIndex">"world" + world index</param>
    /// <param name="levelIndex">"level" + level index</param>
    public void SendProgressionEvent(int progresionStatus, string worldIndex, string levelIndex)
    {
        GameAnalytics.NewProgressionEvent((GAProgressionStatus)progresionStatus, worldIndex, levelIndex);
    }

    /// <param name="flowType">0: undefined ,1: source, 2: sink</param>
    public void SendResourceEvent(int flowType, string currency, int rewardAmount, string itemType, string itemId)
    {
        GameAnalytics.NewResourceEvent((GAResourceFlowType)flowType, currency, rewardAmount, itemType, itemId);
    }

    /// <param name="eventName">[category]:[sub_category]:[outcome]</param>
    public void SendDesignEvents(string eventName, float eventValue)
    {
        GameAnalytics.NewDesignEvent(eventName, eventValue);
    }
    /// <param name="eventName">The event string can have 1 to 5 parts. [category]:[sub_category]:[outcome]</param>
    public void SendDesignEvents(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }

    /// <param name="errorSeverity">Undefined = 0,Debug = 1,Info = 2,Warning = 3,Error = 4,Critical = 5</param>
    public void SendErrorEvent(int errorSeverity, string errorMessage)
    {
        GameAnalytics.NewErrorEvent((GAErrorSeverity)errorSeverity, errorMessage);
    }

    public void SettingPlayerDimention(string customDimention)
    {
        GameAnalytics.SetCustomDimension01(customDimention);
    }
}
