using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object che definisce una tile generica, e tutti i possibili tipi di tile

[CreateAssetMenu(fileName ="New Tile Prototype", menuName = "Create Tile Prototype")]
public class TilePrototype : ScriptableObject
{
    public enum eTileID
    {
        Empty,
        Door,
        FloorUp,
        FloorDown,
        Null
    }

    public eTileID TileType;
    public GameObject PrefabObject;
}
