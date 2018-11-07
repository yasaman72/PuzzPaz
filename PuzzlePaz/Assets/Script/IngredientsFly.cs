using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsFly : MonoBehaviour
{

    public GameBoardManager gameBoardManager;
    [Space]
    public GameObject fakeTileObj;
    public int boardSize;
    public Transform target;
    public float initiaWaitTime;
    public float flyingSpeed;
    public List<FakeTile> fakeTiles;

    private IEnumerator goToTargetCo;

    private void Start()
    {
        for (int i = 0; i < boardSize; i++)
        {
            fakeTiles.Add(new FakeTile());
            fakeTiles[i].fakeTileObj = Instantiate(fakeTileObj, gameObject.transform);
            fakeTiles[i].tileImageObj = fakeTiles[i].fakeTileObj.transform.GetChild(0).gameObject;
            fakeTiles[i].tileImage = fakeTiles[i].tileImageObj.GetComponent<Image>();
            fakeTiles[i].initialImgPos = fakeTiles[i].tileImageObj.GetComponent<RectTransform>().localPosition;
            fakeTiles[i].tileImage.enabled = false;
        }
    }

    public void MoveTileToTarget(int index, int type)
    {
        fakeTiles[index].tileImage.enabled = true;
        fakeTiles[index].tileImage.sprite = gameBoardManager.ingredients[type].sprite;

        goToTargetCo = GoToTarget(index);
        StartCoroutine(goToTargetCo);
    }

    bool firstTimeInCoR = true;
    IEnumerator GoToTarget(int index)
    {
        if (firstTimeInCoR)
        {
            yield return new WaitForSeconds(initiaWaitTime);
            yield return null;
            firstTimeInCoR = false;
        }

        while (Vector2.Distance(fakeTiles[index].tileImageObj.transform.position, target.position) >= 0.1f)
        {
            fakeTiles[index].tileImageObj.transform.position = Vector2.MoveTowards(
                fakeTiles[index].tileImageObj.transform.position,
                target.position,
                flyingSpeed);
            yield return new WaitForSeconds(0.005f);
        }
        firstTimeInCoR = true;

        //disabling the image
        fakeTiles[index].tileImage.enabled = false;

        //going back to the initial position
        fakeTiles[index].tileImageObj.GetComponent<RectTransform>().localPosition = fakeTiles[index].initialImgPos;
        yield return null;
    }

    [System.Serializable]
    public class FakeTile
    {
        public GameObject fakeTileObj;
        public GameObject tileImageObj;
        public Image tileImage;
        public Vector3 initialImgPos;
    }
}
