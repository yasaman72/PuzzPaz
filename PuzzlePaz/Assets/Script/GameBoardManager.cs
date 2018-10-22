using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{

    public Ingredient[] ingredients;
    public GameObject tilePrefab;
    public GameObject tileHolderGameObject;
    public int boardColCount;
    public int tileListSize;
    public float fallingSpeed;
    public Text boardText;
    [Space]
    public GameLogicManager gameLogicManager;
    public DishHandler dishHandler;

    [Header("Movement section")]
    public GameObject blockingObj;

    public List<GameObject> tileList = new List<GameObject>();

    void Start()
    {
        boardText.text = "";

        for (int i = 0; i < tileListSize; i++)
        {
            tileList.Add(new GameObject());
            tileList[i] = Instantiate(tilePrefab, tileHolderGameObject.transform);
            tileList[i].GetComponent<Tile>().myIndex = i;
            tileList[i].GetComponent<Tile>().TileType = Random.Range(0, ingredients.Length);
            //tileList[i].GetComponent<Tile>().TileType = tileList[i].GetComponent<Tile>().TileType;
            tileList[i].transform.GetChild(0).GetComponent<Image>().sprite = ingredients[tileList[i].GetComponent<Tile>().TileType].sprite;
            //tileList[i].GetComponent<Button>().onClick.AddListener(delegate { onClickOnTile(tileList[i]); });
        }
        gameLogicManager.DestroyedConstructor(ingredients.Length);

        if (CheckForDeadend())
        {
            ShuffleTiles();
        }

        blockingObj.SetActive(false);
    }

    public void CheckClickedTile(int type, int index)
    {
        List<GameObject> SimilarTilesList = new List<GameObject>();

        //add the selected tile itself to the list of objects to change
        SimilarTilesList.Add(tileList[index]);
        tileList[index].GetComponent<Tile>().isChecked = true;

        //checks if adjacent tiles have the same type
        SimilarTilesList.AddRange(ListOfSimilarAdjacentTilesObj(index, type));

        tileList[index].GetComponent<Tile>().isChecked = false;

        //Debug.Log("Selected Tiles indexes: ");
        //foreach (GameObject simGameObject in SimilarTilesList)
        //{
        //    Debug.Log((simGameObject.GetComponent<Tile>().myIndex));
        //}

        //check if there are 3 adjacent similar tiles
        if (SimilarTilesList.Count >= 3)
        {
            //sorting game object in the list based on their index
            SimilarTilesList.Sort(SortByIndex);

            //Debug.Log("Type: " + type + " index: " + index + " Similar Tiles: " + SimilarTilesList.Count);

            gameLogicManager.CountDestroyedTilesByType(SimilarTilesList.Count, ingredients[type]);
            dishHandler.ChangeRequirementsAmount(SimilarTilesList.Count, ingredients[type]);

            //Debug.Log("Sorted Selected Tiles indexes: ");
            //foreach (GameObject simGameObject in SimilarTilesList)
            //{
            //    Debug.Log((simGameObject.GetComponent<Tile>().myIndex));
            //}

            //changing the type and sprite of matching tiles
            foreach (GameObject adjacent in SimilarTilesList)
            {
                MoveTiles(adjacent);
                //adjacent.GetComponent<Animator>().SetTrigger("Destroy");
            }

            if (CheckForDeadend())
            {
                string message = string.Format("<color=red><b>-------------GOING TO SHUFFLE AFTER SOME SECONDS-------------</b></color>");
                Debug.Log(message);
                Invoke("ShuffleTiles", 100 * Time.deltaTime);

                //ShuffleTiles();
            }
        }
    }

    static int SortByIndex(GameObject p1, GameObject p2)
    {
        return (p1.GetComponent<Tile>().myIndex).CompareTo(p2.GetComponent<Tile>().myIndex);
    }

    private void MoveTiles(GameObject objectToChange)
    {
        List<GameObject> upperTiles = new List<GameObject>();
        int tileIndex = objectToChange.GetComponent<Tile>().myIndex;

        //find all tile above the breaking tile
        for (int i = tileIndex; i >= 0; i -= boardColCount)
        {
            upperTiles.Add(tileList[i]);
        }

        //move the information of each tile to its below tile
        for (int i = tileIndex; upperTiles.Count > 1; i -= boardColCount)
        {
            tileList[i].GetComponent<Tile>().TileType = tileList[i - boardColCount].GetComponent<Tile>().TileType;

            /////////////////////moving tiles animation///////////////////////////
            //tileHolderGameObject.GetComponent<GridLayoutGroup>().enabled = false;
            //tileList[i].transform.GetChild(0).gameObject.SetActive(false);
            //Vector3 ObjUpStartPos = tileList[i - boardColCount].transform.position;

            //while (Vector2.Distance(tileList[i - boardColCount].transform.position, tileList[i].transform.position) > 0)
            //{
            //    tileList[i - boardColCount].transform.position =
            //        Vector2.MoveTowards(tileList[i - boardColCount].transform.position,
            //        tileList[i].transform.position,
            //        fallingSpeed);
            //    StartCoroutine(WaitForSomeSeconds(0.1f));
            //}
            //tileList[i - boardColCount].transform.position = ObjUpStartPos;

            //tileList[i].transform.GetChild(0).gameObject.SetActive(true);
            //tileHolderGameObject.GetComponent<GridLayoutGroup>().enabled = true;

            /////////////////////End of moving tiles animation/////////////////////

            tileList[i].transform.GetChild(0).GetComponent<Image>().sprite = ingredients[tileList[i].GetComponent<Tile>().TileType].sprite;


            upperTiles.RemoveAt(0);
        }

        //setting type and sprite for the upper most tile in the list/ Insert a new tile from top
        upperTiles[0].GetComponent<Tile>().TileType = Random.Range(0, ingredients.Length);
        upperTiles[0].transform.GetChild(0).GetComponent<Image>().sprite = ingredients[upperTiles[0].GetComponent<Tile>().TileType].sprite;
        upperTiles.RemoveAt(0);
    }

    IEnumerator WaitForSomeSeconds(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        yield return null;
    }

    private List<GameObject> ListOfSimilarAdjacentTilesObj(int index, int type)
    {
        List<GameObject> adjacentGameObject = new List<GameObject>();

        adjacentGameObject.AddRange(checkLeftTile(index, type));
        adjacentGameObject.AddRange(checkRightTile(index, type));
        adjacentGameObject.AddRange(checkTopTile(index, type));
        adjacentGameObject.AddRange(checkDownTile(index, type));

        foreach (GameObject adjGameObject in adjacentGameObject)
        {
            adjGameObject.GetComponent<Tile>().isChecked = false;
        }

        return adjacentGameObject;

    }

    private List<GameObject> checkRightTile(int index, int type)
    {
        List<GameObject> ObjectsFound = new List<GameObject>();

        for (int i = 1;
                index + i < tileListSize && tileList[index + i].GetComponent<Tile>().TileType == type;
                i++)
        {
            //check if we reached the boarder of the board
            if ((index + i) % boardColCount == 0)
            {
                return ObjectsFound;
            }

            if (tileList[index + i].GetComponent<Tile>().isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].GetComponent<Tile>().isChecked = true;

                ObjectsFound.AddRange(checkTopTile(index + i, type));
                ObjectsFound.AddRange(checkDownTile(index + i, type));

            }
        }
        return ObjectsFound;
    }

    private List<GameObject> checkLeftTile(int index, int type)
    {
        List<GameObject> ObjectsFound = new List<GameObject>();

        for (int i = -1;
                index + i >= 0 && tileList[index + i].GetComponent<Tile>().TileType == type;
                i--)
        {
            //check if we reached the boarder of the board
            if ((index + i + 1) % boardColCount == 0)
            {
                return ObjectsFound;
            }

            if (tileList[index + i].GetComponent<Tile>().isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].GetComponent<Tile>().isChecked = true;

                ObjectsFound.AddRange(checkTopTile(index + i, type));
                ObjectsFound.AddRange(checkDownTile(index + i, type));

            }
        }

        return ObjectsFound;
    }

    private List<GameObject> checkDownTile(int index, int type)
    {
        List<GameObject> ObjectsFound = new List<GameObject>();

        for (int i = boardColCount;
                index + i < tileListSize && tileList[index + i].GetComponent<Tile>().TileType == type;
                i += boardColCount)
        {

            if (tileList[index + i].GetComponent<Tile>().isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].GetComponent<Tile>().isChecked = true;

                ObjectsFound.AddRange(checkRightTile(index + i, type));
                ObjectsFound.AddRange(checkLeftTile(index + i, type));
            }
        }
        return ObjectsFound;
    }

    private List<GameObject> checkTopTile(int index, int type)
    {
        List<GameObject> ObjectsFound = new List<GameObject>();

        for (int i = -boardColCount;
                 index + i >= 0 && tileList[index + i].GetComponent<Tile>().TileType == type;
                 i -= boardColCount)
        {
            if (tileList[index + i].GetComponent<Tile>().isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].GetComponent<Tile>().isChecked = true;

                ObjectsFound.AddRange(checkRightTile(index + i, type));
                ObjectsFound.AddRange(checkLeftTile(index + i, type));
            }
        }

        return ObjectsFound;
    }

    //returns true if there is a deadend
    private bool CheckForDeadend()
    {
        //Debug.Log("Check For Deadend!");

        foreach (GameObject tileObj in tileList)
        {
            if (ListOfSimilarAdjacentTilesObj(tileObj.GetComponent<Tile>().myIndex, tileObj.GetComponent<Tile>().TileType).Count >= 2)
            {
                //Debug.Log("Found possible move!");
                return false;
            }
        }
        boardText.text = "DEADEND! Board will shuffle!";
        Debug.Log("It's a DEADEND!");
        return true;
    }

    public void ShuffleTiles()
    {
        boardText.text = "";
        Debug.Log("Shuffling!");

        //create a list of index of spaces that have not been changed
        List<int> emptySpaces = new List<int>();
        for (int i = 0; i < tileListSize; i++)
        {
            emptySpaces.Add(i);
        }

        int randomIndex;
        foreach (GameObject tileObj in tileList)
        {
            //find a random index for a random space in the board
            randomIndex = Random.Range(0, emptySpaces.Count);

            //give the new random index to the game object
            tileObj.GetComponent<Tile>().myIndex = emptySpaces[randomIndex];
            tileObj.transform.GetChild(0).GetComponent<Image>().sprite = ingredients[tileObj.GetComponent<Tile>().TileType].sprite;

            //remove the index from indexes list
            emptySpaces.RemoveAt(randomIndex);
        }

        //changing the actual place of tiles
        tileList.Sort(SortByIndex);
        int j = 0;
        foreach (GameObject tileObj in tileList)
        {
            tileObj.transform.SetSiblingIndex(j);
            j++;
        }

        //check if the new board still has deadend
        if (CheckForDeadend())
        {
            ShuffleTiles();
        }
        else
        {
            //////////////////////////////play animation of changing place!!///////////////////////////////////////////
            //remember! You are losing the initial index of your game object! Do something for that!
        }
    }
}
