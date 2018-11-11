using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClickedOn : MonoBehaviour {

	public void ClickedOnTile()
    {
        gameObject.transform.parent.GetComponent<TilesClickHelper>().gameBoardManager.CheckClickedTile(gameObject.transform.GetSiblingIndex());
    }
}