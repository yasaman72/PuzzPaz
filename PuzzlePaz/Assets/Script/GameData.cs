using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameData : MonoBehaviour
{
    public LevelDatas levelDatas;
    public LevelManager levelManager;
    public GameMenuManager gameMenuManager;

    private string dataFilePath = "/Data/PlayerData.json";

    private void Start()
    {
        for (int i = 0; i < levelManager.levels.Length; i++)
        {
            LevelData myLevelData = new LevelData();
            myLevelData.LevelIndex = i;
            myLevelData.lvlState = -1;
            levelDatas.Datas.Add(myLevelData);
        }

        //setting up the current active level
        if (PlayerPrefs.HasKey("CurrentLvl"))
        {
            levelDatas.Datas[PlayerPrefs.GetInt("CurrentLvl")].lvlState = 0;
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLvl", 0);
        }
        int activeLevelIndex = PlayerPrefs.GetInt("CurrentLvl");

        //set levels with smaller index's state to passed
        for (int i = 0; i < activeLevelIndex; i++)
        {
            levelDatas.Datas[i].lvlState = 1;
        }

    }

    //will setup the game based on player saved data
    public void SetInitialLevel(List<GameObject> levelNodes)
    {
        for (int i = 0; i < levelDatas.Datas.Count; i++)
        {
            //check if player has not yet passed the level
            if (levelDatas.Datas[i].lvlState < 0)
            {
                levelNodes[i].GetComponent<Image>().color = gameMenuManager.future;
            }
            else if (levelDatas.Datas[i].lvlState == 0)
            {
                levelNodes[i].GetComponent<Image>().color = gameMenuManager.current;
            }
            else
            {
                levelNodes[i].GetComponent<Image>().color = gameMenuManager.passed;
            }
        }
    }

    //will only be called when player passes a level
    public void SetUpNewPlayerLevelData(int levelIndex, int stars, int rewardAmount)
    {
        if (PlayerPrefs.HasKey("CurrentLvl"))
        {
            if (PlayerPrefs.GetInt("CurrentLvl") < levelIndex)
            {
                PlayerPrefs.SetInt("CurrentLvl", levelIndex);
                Debug.Log("Setuped a new current level: " + levelIndex);
            }
        }

        levelDatas.Datas[levelIndex].lvlState = 1;
        levelDatas.Datas[levelIndex + 1].lvlState = 0;
        levelDatas.Datas[levelIndex].score = rewardAmount;

        //check if the new star amount is bigger than the current one and then sets it
        if (levelDatas.Datas[levelIndex].starsAmount < stars)
            levelDatas.Datas[levelIndex].starsAmount = stars;

        gameMenuManager.SetUpLevelStars(levelIndex, stars);
    }


    public void SaveGame()
    {
        string filePath = Application.dataPath + dataFilePath;
        string json = "";

        json += "[";
        for (int i = 0; i < levelDatas.Datas.Count; i++)
        {
            json += JsonUtility.ToJson(levelDatas.Datas[i]);
            json += ",";
        }
        json += "]";
        Debug.Log(json);

        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, json);
        }
    }

    public void LoadGame()
    {
        string filePath = Application.dataPath + dataFilePath;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            levelDatas = new LevelDatas();
            levelDatas = JsonUtility.FromJson<LevelDatas>(json);
        }

    }
}
