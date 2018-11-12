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
    public GameData gameData;
    
    private List<GameObject> levelNodes = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < levelsLine.positionCount; i++)
        {
            GameObject newLevel = Instantiate(levelNodeObj, levelsContainer);

            newLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(levelsLine.GetPosition(i).x, levelsLine.GetPosition(i).y);
            newLevel.transform.GetChild(0).GetComponent<Image>().color = future;
            newLevel.transform.GetChild(1).GetComponent<Text>().text = (i+1).ToString();
            newLevel.transform.GetChild(2).gameObject.SetActive(false);

            int j = i;
            newLevel.GetComponent<Button>().onClick.AddListener(() => levelManager.SetLevel(j));

            levelNodes.Add(newLevel);
        }

        gameData.SetInitialLevel(levelNodes);
    }

    public void SetUpLevelStars(int levelIndex, int stars)
    {
        levelNodes[levelIndex].transform.GetChild(0).GetComponent<Image>().color = passed;
        if(levelNodes[levelIndex + 1] != null)
        levelNodes[levelIndex+1].transform.GetChild(0).GetComponent<Image>().color = current;

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
