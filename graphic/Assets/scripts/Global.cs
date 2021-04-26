using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    /// <summary>
    /// the possible "ruler" of each territory
    /// </summary>
    public enum Control
    {
        Free,
        player1,
        player2,
        player3,
        player4,
        player5,
        player6
    }
    /// <summary>
    /// from the enum to int
    /// </summary>
    /// <param name="c"> the "ruler"</param>
    /// <returns>get int that represent which player </returns>
    public static int indexOfControl(Control c)
    {
        switch (c)
        {
            case Control.player1:
                return 0;
            case Control.player2:
                return 1;
            case Control.player3:
                return 2;
            case Control.player4:
                return 3;
            case Control.player5:
                return 4;
            case Control.player6:
                return 5;

        }
        return -1;
    }


    public static Color FreeColor = new Color(0.5f, 0.5f, 0.5f, 1f);//gray
    public static Color player1 = Color.red;
    public static Color player2 = new Color(0.4170969f, 0.5514073f, 0.8584906f);//blue
    public static Color player3 = Color.green;
    public static Color player4 = new Color(0.1f, 0.5f, 0.2f);//dark green
    public static Color player5 = Color.yellow;
    public static Color player6 = new Color(0.5f, 0.1f, 0.95f);//bordu
    //colors of each "ruler"
    public static Color[] colors = { FreeColor, player1, player2, player3, player4, player5, player6 };

    public const int MAX_INDEX_OF_TER = 42;
    //the main data structure 
    public static Graph map = new Graph();
}
