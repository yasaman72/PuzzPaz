using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public int levelsCount;
    public Color passed, current, future;
    public Sprite gainedStar, notGainedStar;
    public LineRenderer levelsLine;
    public GameObject levelNodeObj;
    public Transform levelsContainer;
    public LevelManager levelManager;
    public InGameManager inGameManager;
    public GameData gameData;

    private int myLevelCount;

    private List<GameObject> levelNodes;

    public void MakeLevelNodes(int levelCount)
    {
        levelNodes = new List<GameObject>();
        myLevelCount = levelCount;

        for (int i = 0; i < levelCount; i++)
        {
            GameObject newLevel = Instantiate(levelNodeObj, levelsContainer);

            newLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(levelsLine.GetPosition(i).x, levelsLine.GetPosition(i).y);
            newLevel.transform.GetChild(1).GetComponent<Image>().color = future;
            newLevel.transform.GetChild(1).transform.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            newLevel.transform.GetChild(2).gameObject.SetActive(false);

            int j = i;
            newLevel.GetComponent<Button>().onClick.AddListener(() => levelManager.SetLevel(j));

            levelNodes.Add(newLevel);
        }

        gameData.SetInitialLevel(levelNodes);
    }

    public void SetUpLevelStarsAndShape(int levelIndex, int stars, bool initial)
    {
        if (!initial)
        {
            levelNodes[levelIndex].transform.GetChild(0).GetComponent<Image>().color = passed;
            levelNodes[levelIndex].transform.GetChild(1).gameObject.SetActive(false);

            if (levelIndex + 1 == PlayerPrefs.GetInt("CurrentLvl"))
            {
                levelNodes[levelIndex + 1].transform.GetChild(0).GetComponent<Image>().color = current;
                levelNodes[levelIndex + 1].transform.GetChild(1).gameObject.SetActive(true);
                levelNodes[levelIndex + 1].transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        //set the stars
        GameObject starsHolder = levelNodes[levelIndex].transform.GetChild(2).gameObject;
        starsHolder.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            if (i < stars)
                starsHolder.transform.GetChild(i).GetComponent<Image>().sprite = gainedStar;
            else
                starsHolder.transform.GetChild(i).GetComponent<Image>().sprite = notGainedStar;
        }
    }

}
