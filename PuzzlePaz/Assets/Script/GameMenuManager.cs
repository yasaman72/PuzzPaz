using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{

    public int levelsCount;
    public Color passed, current, future;
    public LineRenderer levelsLine;
    public GameObject levelNodeObj;
    public Transform levelsContainer;
    public LevelManager levelManager;
    
    private List<GameObject> levelNodes = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < levelsCount; i++)
        {
            GameObject newLevel = Instantiate(levelNodeObj, levelsContainer);

            newLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(levelsLine.GetPosition(i).x, levelsLine.GetPosition(i).y);
            newLevel.transform.GetChild(0).GetComponent<Image>().color = future;
            newLevel.transform.GetChild(1).GetComponent<Text>().text = (i+1).ToString();
            newLevel.transform.GetChild(2).gameObject.SetActive(false);

            int j = i;
            newLevel.GetComponent<Button>().onClick.AddListener(() => ClickedOnNodeBtn(j));

            levelNodes.Add(newLevel);
        }
    }

    public void ClickedOnNodeBtn(int index)
    {
        Debug.Log(index.ToString());
        //levelManager.SetLevel(index);
    }

}
