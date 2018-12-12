using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAudioMuter : MonoBehaviour
{

    public GameObject crossSign;

    public void ChangeAudioState()
    {
        if (PlayerPrefs.GetInt("soundState") == 1)
        {
            AudioListener.pause = true;
            crossSign.SetActive(true);

            PlayerPrefs.SetInt("soundState", 0);
        }
        else if (PlayerPrefs.GetInt("soundState") == 0)
        {
            AudioListener.pause = false;
            crossSign.SetActive(false);
            PlayerPrefs.SetInt("soundState", 1);
        }
    }

}
