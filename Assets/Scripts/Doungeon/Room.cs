using UnityEngine;

public class Room : MonoBehaviour
{
    public Tile[,] Tiles;
    public Vector2Int RoomPosition; //room position in the floor
    
    /// <summary>
    /// getter che ritorna le dimensioni della room
    /// </summary>
    public Vector2Int Size { get { return new Vector2Int(Tiles.GetLength(0), Tiles.GetLength(1)); } }

    //references to all the doors of the room
    public Door UpDoor, DownDoor, LeftDoor, RightDoor;
}