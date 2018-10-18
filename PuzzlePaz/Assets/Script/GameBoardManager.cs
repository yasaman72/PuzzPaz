using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{

    public Sprite[] tilesImage;
    public GameObject tilePrefab;
    public GameObject tileHolderGameObject;
    public int boardColCount;
    public int tileListSize;
    public List<GameObject> tileList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < tileListSize; i++)
        {
            tileList.Add(new GameObject());
            tileList[i] = Instantiate(tilePrefab, tileHolderGameObject.transform);
            tileList[i].GetComponent<Tile>().myIndex = i;
            tileList[i].GetComponent<Tile>().TileType = Random.Range(0, tilesImage.Length);
            //tileList[i].GetComponent<Tile>().TileType = tileList[i].GetComponent<Tile>().TileType;
            tileList[i].transform.GetChild(0).GetComponent<Image>().sprite = tilesImage[tileList[i].GetComponent<Tile>().TileType];
            //tileList[i].GetComponent<Button>().onClick.AddListener(delegate { onClickOnTile(tileList[i]); });
        }
    }

    int adjacentSimilarTiles = 0;
    int serachingType;
    List<GameObject> similarAdjacentTiles = new List<GameObject>();

    public void CheckClickedTile(int type, int index)
    {
        similarAdjacentTiles = new List<GameObject>();
        serachingType = type;
        //checks if adjacent tiles have the same type
        CheckAdjacentTile(index);

        Debug.Log("Type: " + type + " index: " + index + " Adjacent Similar Tiles: " + adjacentSimilarTiles);

        if (similarAdjacentTiles.Count >= 2)
        {
            //add the selected tile itself to the list of objects to change
            similarAdjacentTiles.Add(tileList[index]);

            //changing the type and sprite of matching tiles
            foreach (GameObject adjacent in similarAdjacentTiles)
            {
                adjacent.GetComponent<Tile>().TileType = Random.Range(0, tilesImage.Length);
                adjacent.transform.GetChild(0).GetComponent<Image>().sprite = tilesImage[adjacent.GetComponent<Tile>().TileType];
            }
        }
    }

    private void CheckAdjacentTile(int index)
    {
        checkRightTile(index);
        checkLeftTile(index);
        checkTopTile(index);
        checkDownTile(index);
    }

    private void checkRightTile(int index)
    {
        for (int i = 1;
                index + i < tileListSize && tileList[index + i].GetComponent<Tile>().TileType == serachingType;
                i++)
        {
            adjacentSimilarTiles++;
            similarAdjacentTiles.Add(tileList[index + i]);
            checkTopTile(index + i);
            checkDownTile(index + i);
        }
    }

    private void checkLeftTile(int index)
    {
        for (int i = -1;
                index + i > tileListSize && tileList[index + i].GetComponent<Tile>().TileType == serachingType;
                i--)
        {
            adjacentSimilarTiles++;
            similarAdjacentTiles.Add(tileList[index + i]);
            checkTopTile(index + i);
            checkDownTile(index + i);
        }
    }

    private void checkDownTile(int index)
    {
        for (int i = boardColCount;
                index + i < tileListSize && tileList[index + i].GetComponent<Tile>().TileType == serachingType;
                i += boardColCount)
        {
            adjacentSimilarTiles++;
            similarAdjacentTiles.Add(tileList[index + i]);
            checkRightTile(index + i);
            checkLeftTile(index + i);
        }
    }

    private void checkTopTile(int index)
    {
        for (int i = -boardColCount;
      index + i > tileListSize && tileList[index + i].GetComponent<Tile>().TileType == serachingType;
      i -= boardColCount)
        {
            adjacentSimilarTiles++;
            similarAdjacentTiles.Add(tileList[index + i]);
            checkRightTile(index + i);
            checkLeftTile(index + i);
        }
    }
}
