using UnityEngine;
using UnityEditor;

[HelpURL("http://acidgreengames.com/")]
public class GeneralCustomEditor : EditorWindow
{
    static int coinAdder;

    [MenuItem("Custom/Game Controller")]

    private static void ShowWindow()
    {
        GetWindow<GeneralCustomEditor>("Game Controller");
    }

    void OnGUI()
    {
        //coinAdder = EditorGUILayout.IntField("Coin Amount: ", coinAdder);
        ////GUILayout.Label("Delete All", EditorStyles.largeLabel);
        ////GUILayout.Label("Give Me Coins!", EditorStyles.largeLabel);
        //if(coinAdder <= 0)
        //{
        //    EditorGUILayout.HelpBox("Enter a number larger than 0.", MessageType.Warning);
        //}

        //if (GUILayout.Button("Give Me Coins!"))
        //{
        //    PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins") + coinAdder);
        //    Debug.Log("Added "+ coinAdder + " coins. Now you have "+ PlayerPrefs.GetInt("PlayerCoins") + " coins." );
        //}

        EditorGUILayout.Space();
        if (GUILayout.Button("Delete All"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted all");
        }
    }
}