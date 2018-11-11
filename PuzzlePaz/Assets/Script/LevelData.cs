using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData {

    public int LevelIndex;
    public int score;
    public int starsAmount;
    // -1 -> not reached yet | 0 -> currently in | 1 -> passed
    public int lvlState;
}
