using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//defines a generic object that can be positioned on a tile

public class MapObject : MonoBehaviour
{
    public Vector2Int TilePosition;
    public bool Passable;       //passable = you can enter in it, like a door
}
