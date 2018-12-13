using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level", menuName = "cooking/level"), System.Serializable]
public class Level : ScriptableObject
{
    public enum difficulty
    {
        easy,
        mediem,
        hard
    };

    public string levelName;
    public int levelIndex;
    public int moves;
    public difficulty difficultyLevel;
    public Order[] ordersList;

    public int GetLevelGoals(int goalIndex)
    {
        int maxPossibleCoinReward = 0;
        
        foreach(Order order in ordersList)
        {
            foreach(Dish dish in order.dishes)
            {
                maxPossibleCoinReward += dish.rewardAmount;
            }
        }

        switch (goalIndex)
        {
            case 0:
                return (Mathf.CeilToInt((maxPossibleCoinReward) * 0.4f));
            case 1:
                return (Mathf.CeilToInt((maxPossibleCoinReward) * 0.6f));
            case 2:
                return (Mathf.CeilToInt((maxPossibleCoinReward) * 0.85f));
            default:
                Debug.Log("Couldn't find level goals amount.");
                return 0;
        }

    }
}
