using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameData : MonoBehaviour
{
    public List<LevelData> levelDatas;
    public LevelManager levelManager;
    public GameMenuManager gameMenuManager;

    private string dataFilePath = "/Data/PlayerData.json";

    private void Start()
    {
        levelDatas = new List<LevelData>();

        for (int i = 0; i < levelManager.levels.Length; i++)
        {
            LevelData myLevelData = new LevelData();
            myLevelData.LevelIndex = i;
            myLevelData.lvlState = -1;
            levelDatas.Add(myLevelData);
        }

        ////setting up the current active level
        //if (PlayerPrefs.HasKey("CurrentLvl"))
        //{
        //    levelDatas[PlayerPrefs.GetInt("CurrentLvl")].lvlState = 0;
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("CurrentLvl", 0);
        //}
        //int activeLevelIndex = PlayerPrefs.GetInt("CurrentLvl");

        ////set levels with smaller index's state to passed
        //for (int i = 0; i < activeLevelIndex; i++)
        //{
        //    levelDatas[i].lvlState = 1;
        //}

        if (PlayerPrefs.HasKey("SavedGame"))
        {
            if (PlayerPrefs.GetInt("SavedGame") == 1)
            {
                LoadGame();
            }
        }
        else
        {
            levelDatas[0].lvlState = 0;
        }

        gameMenuManager.MakeLevelNodes(levelManager.levels.Length);
    }

    //will setup the game based on player saved data
    public void SetInitialLevel(List<GameObject> levelNodes)
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            //check if player has not yet passed the level
            if (levelDatas[i].lvlState < 0)
            {
                levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.future;
            }
            else if (levelDatas[i].lvlState == 0)
            {
                levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.current;
                gameMenuManager.SetUpLevelStarsAndShape(i, levelDatas[i].starsAmount, true);

            }
            else
            {
                levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.passed;
                gameMenuManager.SetUpLevelStarsAndShape(i, levelDatas[i].starsAmount, true);

            }
        }
    }

    //will only be called when player passes a level
    public void SetUpNewPlayerLevelData(int levelIndex, int stars, int rewardAmount)
    {
        if (PlayerPrefs.HasKey("CurrentLvl"))
        {
            if (PlayerPrefs.GetInt("CurrentLvl") < levelIndex + 1)
            {
                PlayerPrefs.SetInt("CurrentLvl", levelIndex + 1);
                Debug.Log("Setuped a new current level: " + levelIndex);
                levelDatas[PlayerPrefs.GetInt("CurrentLvl")].lvlState = 0;
            }
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLvl", 1);
            Debug.Log("Setuped a new current level: " + levelIndex);
            levelDatas[PlayerPrefs.GetInt("CurrentLvl")].lvlState = 0;
        }

        levelDatas[levelIndex].lvlState = 1;
        levelDatas[levelIndex].score = rewardAmount;

        //check if the new star amount is bigger than the current one and then sets it
        if (levelDatas[levelIndex].starsAmount < stars)
            levelDatas[levelIndex].starsAmount = stars;

        gameMenuManager.SetUpLevelStarsAndShape(levelIndex, stars, false);
    }

    public void ResetJSON()
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            levelDatas[i].lvlState = -1;
            levelDatas[i].score = 0;
            levelDatas[i].starsAmount = 0;

            if (i == 0)
            {
                levelDatas[i].lvlState = 0;
            }
        }
        SaveGame();
    }

    public void SaveGame()
    {
        string filePath = Application.dataPath + dataFilePath;
        string json = "";

        //json += "[";
        for (int i = 0; i < levelDatas.Count; i++)
        {
            json += JsonUtility.ToJson(levelDatas[i], true);
            if (i < levelDatas.Count - 1)
                json += "/";
        }
        //json += "]";

        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, json);
            if (!PlayerPrefs.HasKey("SavedGame"))
            {
                PlayerPrefs.SetInt("SavedGame", 1);
            }
            //Debug.Log("Saved Data!!!");
        }
    }

    public void LoadGame()
    {
        string filePath = Application.dataPath + dataFilePath;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            string[] jsonChunks = json.Split('/');

            for (int i = 0; i < levelDatas.Count; i++)
            {
                levelDatas[i] = JsonUtility.FromJson<LevelData>(jsonChunks[i]);
            }
            //Debug.Log("Loaded Data!!");
        }
    }
}
