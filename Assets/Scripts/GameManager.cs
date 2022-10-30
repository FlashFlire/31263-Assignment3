using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    public static int score = 0;

    public static float ghostScareTime = 0f;
    // how long ghosts have left in their scared state, in seconds

    public static int ghostsDefeated = 0;
    // how many ghosts are currently defeated (mainly for music purposes)

    public static bool gamePlaying = false;
    // determines if ghosts, cherry and pacstudent can move

    public static int ghostState = 0;
    // 0: normal
    // 1: scared
    // 2: recovering

}
