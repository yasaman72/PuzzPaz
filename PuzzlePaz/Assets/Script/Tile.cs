using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int TileType;
    public int myIndex;
    public bool isChecked = false;
    public int upperSimilarTiles;
    public int belowSimilarTiles;

    public void onClickOnTile()
    {
        //Debug.Log("Clcike on tile with type " + TileType);
        gameObject.transform.parent.GetComponent<GameBoardManager>().CheckClickedTile(TileType,myIndex);
    }
}
