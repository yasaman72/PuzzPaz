using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tile
{
    public int TileType;
    public int myIndex;
    public bool isChecked;
    public int upperSimilarTiles;
    public int belowSimilarTiles;
    public GameObject gameObject;

    //public void onClickOnTile()
    //{
    //    //Debug.Log("Clcike on tile with type " + TileType);
    //    this.gameObject.transform.parent.GetComponent<GameBoardManager>().CheckClickedTile(TileType,myIndex);
    //}
}
