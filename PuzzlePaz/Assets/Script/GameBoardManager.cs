using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{

    public Ingredient[] ingredients;
    public GameObject tilePrefab;
    public Transform tileHolderGameObject;
    public int boardColCount;
    public int tileListSize;
    public Text boardText;
    [Space]
    public LevelManager levelManager;
    public GameLogicManager gameLogicManager;
    public OrderHandler orderHandler;
    public IngredientsFly ingredientsFly;

    [Header("Movement section")]
    public GameObject blockingObj;

    public List<Tile> tileList = new List<Tile>();

    private IEnumerator coroutine;
    private int thisGameMoves;

    void Start()
    {
        boardText.text = "";

        for (int i = 0; i < tileListSize; i++)
        {
            tileList.Add(new Tile());
            tileList[i].gameObject = Instantiate(tilePrefab, tileHolderGameObject);
            tileList[i].myIndex = i;
            tileList[i].isChecked = false;
            tileList[i].TileType = Random.Range(0, ingredients.Length);
            tileList[i].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ingredients[tileList[i].TileType].sprite;
        }
        gameLogicManager.DestroyedConstructor(ingredients.Length);

        if (CheckForDeadend())
        {
            ShuffleTiles();
        }
    }

    public void NewBoard()
    {
        ShuffleTiles();
        thisGameMoves = 0;
        blockingObj.SetActive(false);
    }

    public void CheckClickedTile(int index)
    {
        List<Tile> SimilarTilesList = new List<Tile>();

        //add the selected tile itself to the list of objects to change
        SimilarTilesList.Add(tileList[index]);
        tileList[index].isChecked = true;

        int type = tileList[index].TileType;

        //checks if adjacent tiles have the same type
        SimilarTilesList.AddRange(ListOfSimilarAdjacentTilesObj(index, type));

        tileList[index].isChecked = false;

        //Debug.Log("Selected Tiles indexes: ");
        //foreach (GameObject simGameObject in SimilarTilesList)
        //{
        //    Debug.Log((simGameObject.GetComponent<Tile>().myIndex));
        //}

        //check if there are 3 adjacent similar tiles
        if (SimilarTilesList.Count >= 2)
        {
            thisGameMoves++;
            levelManager.MadeAMove(thisGameMoves);

            blockingObj.SetActive(true);
            StartCoroutine("DisableBlocker");
            //Debug.Log("Type: " + type + " index: " + index + " Similar Tiles: " + SimilarTilesList.Count);

            //sorting game object in the list based on their index
            SimilarTilesList.Sort(SortDesByIndex);
            //Debug.Log("Sorted Selected Tiles indexes: ");
            //foreach (GameObject simGameObject in SimilarTilesList)
            //{
            //    Debug.Log((simGameObject.GetComponent<Tile>().myIndex));
            //}


            gameLogicManager.CountDestroyedTilesByType(SimilarTilesList.Count, ingredients[type]);
            bool wasSomthingFromOrder = orderHandler.ChangeRequirementsAmount(SimilarTilesList.Count, ingredients[type]);


            //changing the type and sprite of matching tiles
            foreach (Tile adjacent in SimilarTilesList)
            {
                //playing flying animation
                if (wasSomthingFromOrder)
                    ingredientsFly.MoveTileToTarget(adjacent.myIndex, type);

                //check how many similar tiles are upon this one to set the right animation in the next step
                int upperSimilarTiles = 0;
                for (int i = adjacent.myIndex - boardColCount;
                    i >= 0 && adjacent.TileType == tileList[i].TileType;
                    i -= boardColCount)
                {
                    upperSimilarTiles++;
                }
                adjacent.upperSimilarTiles = upperSimilarTiles;

                //check how many similar tiles are down this one to set the right animation in the next step
                int belowSimilarTiles = 0;
                for (int i = adjacent.myIndex + boardColCount;
                    i < tileListSize && adjacent.TileType == tileList[i].TileType;
                    i += boardColCount)
                {
                    belowSimilarTiles++;
                }
                adjacent.belowSimilarTiles = belowSimilarTiles;

                //Debug.Log("Index: " + adjacent.myIndex + " | upper similar tiles amount: " + upperSimilarTiles
                //    + " | down similar tiles amount: " + belowSimilarTiles);
            }

            SimilarTilesList.Sort(SortAscByIndex);
            foreach (Tile adjacent in SimilarTilesList)
            {
                ChangeTiles(adjacent);
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

    static int SortAscByIndex(Tile p1, Tile p2)
    {
        return (p1.myIndex).CompareTo(p2.myIndex);
    }

    static int SortDesByIndex(Tile p1, Tile p2)
    {
        return (p2.myIndex).CompareTo(p1.myIndex);
    }

    private void ChangeTiles(Tile objectToChange)
    {
        List<Tile> upperTiles = new List<Tile>();
        int tileIndex = objectToChange.myIndex;

        //find all tile above the breaking tile
        for (int i = tileIndex; i >= 0; i -= boardColCount)
        {
            upperTiles.Add(tileList[i]);
        }

        //move the information of each tile to its below tile
        for (int i = tileIndex; upperTiles.Count > 1; i -= boardColCount)
        {
            tileList[i].TileType = tileList[i - boardColCount].TileType;

            //falling animation handler
            if (objectToChange.belowSimilarTiles == 0)
            {
                tileList[i].gameObject.GetComponent<Animator>().SetFloat("Blend", objectToChange.upperSimilarTiles / 5f);
                tileList[i].gameObject.GetComponent<Animator>().SetTrigger("Fall");
            }

            tileList[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ingredients[tileList[i].TileType].sprite;

            upperTiles.RemoveAt(0);
        }

        //setting type and sprite for the upper most tile in the list/ Insert a new tile from top
        upperTiles[0].TileType = Random.Range(0, ingredients.Length);
        upperTiles[0].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ingredients[upperTiles[0].TileType].sprite;
        //falling animation handler
        if (objectToChange.belowSimilarTiles == 0)
        {
            upperTiles[0].gameObject.GetComponent<Animator>().SetFloat("Blend", objectToChange.upperSimilarTiles / 5f);
            upperTiles[0].gameObject.GetComponent<Animator>().SetTrigger("Fall");
        }
        upperTiles.RemoveAt(0);
    }

    IEnumerator DisableBlocker()
    {
        //waiting time depends on the length of the animation
        yield return new WaitForSeconds(0.5f);
        blockingObj.SetActive(false);
        yield return null;
    }

    private List<Tile> ListOfSimilarAdjacentTilesObj(int index, int type)
    {
        List<Tile> adjacentGameObject = new List<Tile>();

        adjacentGameObject.AddRange(checkLeftTile(index, type));
        adjacentGameObject.AddRange(checkRightTile(index, type));
        adjacentGameObject.AddRange(checkTopTile(index, type));
        adjacentGameObject.AddRange(checkDownTile(index, type));

        foreach (Tile adjGameObject in adjacentGameObject)
        {
            adjGameObject.isChecked = false;
        }

        return adjacentGameObject;

    }

    private List<Tile> checkRightTile(int index, int type)
    {
        List<Tile> ObjectsFound = new List<Tile>();

        for (int i = 1;
                index + i < tileListSize && tileList[index + i].TileType == type;
                i++)
        {
            //check if we reached the boarder of the board
            if ((index + i) % boardColCount == 0)
            {
                return ObjectsFound;
            }

            if (tileList[index + i].isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].isChecked = true;

                ObjectsFound.AddRange(checkTopTile(index + i, type));
                ObjectsFound.AddRange(checkDownTile(index + i, type));

            }
        }
        return ObjectsFound;
    }

    private List<Tile> checkLeftTile(int index, int type)
    {
        List<Tile> ObjectsFound = new List<Tile>();

        for (int i = -1;
                index + i >= 0 && tileList[index + i].TileType == type;
                i--)
        {
            //check if we reached the boarder of the board
            if ((index + i + 1) % boardColCount == 0)
            {
                return ObjectsFound;
            }

            if (tileList[index + i].isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].isChecked = true;

                ObjectsFound.AddRange(checkTopTile(index + i, type));
                ObjectsFound.AddRange(checkDownTile(index + i, type));

            }
        }

        return ObjectsFound;
    }

    private List<Tile> checkDownTile(int index, int type)
    {
        List<Tile> ObjectsFound = new List<Tile>();

        for (int i = boardColCount;
                index + i < tileListSize && tileList[index + i].TileType == type;
                i += boardColCount)
        {

            if (tileList[index + i].isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].isChecked = true;

                ObjectsFound.AddRange(checkRightTile(index + i, type));
                ObjectsFound.AddRange(checkLeftTile(index + i, type));
            }
        }
        return ObjectsFound;
    }

    private List<Tile> checkTopTile(int index, int type)
    {
        List<Tile> ObjectsFound = new List<Tile>();

        for (int i = -boardColCount;
                 index + i >= 0 && tileList[index + i].TileType == type;
                 i -= boardColCount)
        {
            if (tileList[index + i].isChecked == false)
            {
                ObjectsFound.Add(tileList[index + i]);
                tileList[index + i].isChecked = true;

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

        foreach (Tile tileObj in tileList)
        {
            if (ListOfSimilarAdjacentTilesObj(tileObj.myIndex, tileObj.TileType).Count >= 1)
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
        foreach (Tile tileObj in tileList)
        {
            //find a random index for a random space in the board
            randomIndex = Random.Range(0, emptySpaces.Count);

            //give the new random index to the game object
            tileObj.myIndex = emptySpaces[randomIndex];
            tileObj.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ingredients[tileObj.TileType].sprite;

            //remove the index from indexes list
            emptySpaces.RemoveAt(randomIndex);
        }

        //changing the actual place of tiles
        tileList.Sort(SortAscByIndex);
        int j = 0;
        foreach (Tile tileObj in tileList)
        {
            tileObj.gameObject.transform.SetSiblingIndex(j);
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

