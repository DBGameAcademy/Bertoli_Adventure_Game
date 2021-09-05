using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tile Prototype", menuName = "Create Tile Prototype")]
public class TilePrototype : ScriptableObject
{
    public enum eTileID
    {
        Empty,
        Door,
        FloorUp,
        FloorDown
    }
    public eTileID TileType;
    public GameObject PrefabObject;
}
