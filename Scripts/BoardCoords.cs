using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Checkers;
public class BoardCoords
{
    public int GetRightBoardCoords(float number)
    {

        if (number < 0.0)
        {

            if ((int)number == 0) return 4;
            return ((int)number + 4);
        }
        else if (number > 0.0)
        {

            if ((int)number == 0) return 5;
            return ((int)number + 5);
        }
        return -1;

    }
    public bool Check(int row , int col, Mode mode)
    {
        if(mode == Mode.INTERNATIONAL) {
            if (row < 10 && row >= 0 && col < 10 && col >= 0)
                return true;
        }
        else {
            if (row < 8 && row >= 0 && col < 8 && col >= 0)
                return true;
        }
        return false;
    }
    public bool Check(Vector2 vector)
    {
        int row = GetRightBoardCoords(vector.x);
        int col = GetRightBoardCoords(vector.y);
        if (row < 10 && row >= 0 && col < 10 && col >= 0)
            return true;
        return false;
    }
}
