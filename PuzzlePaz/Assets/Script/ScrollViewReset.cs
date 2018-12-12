using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewReset : MonoBehaviour {

    public GameObject P1Content;

    //private RectTransform P1ContentRect;
    //private Vector2 StartingP1;

    public static bool IsStudioBusy;

    void Awake()
    {
        //P1ContentRect = P1Content.GetComponent<RectTransform>();
        //StartingP1 = P1Content.GetComponent<RectTransform>().anchoredPosition;
    }

    void OnEnable()
    {
        Invoke("ResetScrollbar", 0.01f);
    }

    public void ResetScrollbar()
    {
        P1Content.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }
}
