using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools 
{
    #region ENUMS
    public enum FloorType
    {
        WALKABLE, NONWOKABLE, EMPTY ,START, FINISH, EVENT
    }

    public enum Directions {
        FORWORD,BACK,LEFT,RIGHT
    }
    #endregion

    #region FUNCTIONS
    public static Vector3 GetMoveDirectonBaseOnRotation(int _Rotation)
    {


         if( _Rotation > 0 && _Rotation < 90)
                return new Vector3(1, 0, 0);
        if (_Rotation > 90 && _Rotation < 180)
            return new Vector3(1, 0, 0);
        if (_Rotation < 0 && _Rotation > -90)
            return new Vector3(1, 0, 0);
        if (_Rotation > 0 && _Rotation < 90)
            return new Vector3(1, 0, 0);
        else return Vector3.zero;
       
    }

    public static Directions GetOpositeDirection(this Directions dir) {
        switch (dir)
        {
            case Directions.FORWORD:
                return Directions.BACK;
            case Directions.BACK:
                return Directions.FORWORD;
            case Directions.LEFT:
                return Directions.RIGHT;
            case Directions.RIGHT:
                return Directions.LEFT;
            default:
                return Directions.BACK;
        }
    }
    #endregion
    #region CLASSES

    #endregion
}
