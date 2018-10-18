using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int TileType;
    public int myIndex;

    public void onClickOnTile()
    {
        Debug.Log("Clcike on tile with index " + TileType);
        gameObject.transform.parent.GetComponent<GameBoardManager>().CheckClickedTile(TileType,myIndex);
    }
}
