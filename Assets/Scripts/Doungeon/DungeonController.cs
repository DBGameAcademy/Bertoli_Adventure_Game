using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoSingleton<DungeonController>
{
    Dungeon currentDungeon;
    public Dungeon CurrentDungeon { get { return currentDungeon; } }

    public TileSet TileSet;

    public int NoOfFloors;
    public Vector2Int RoomsPerFloor;        
    public Vector2Int RoomSize;

    public Floor CurrentFloor;
    private Vector2Int roomPosition;
    public Room CurrentRoom;

    public int MobDensity;

    public Tile GetTile(Vector2Int _position)
    {
        return CurrentRoom.Tiles[_position.x, _position.y];
    }
    //==============================================================================
    public void CreateNewDungeon()
    {
        currentDungeon = new Dungeon();

        // CRETING TILES OF EACH FLOOR
        for (int i = 0; i < NoOfFloors; i++)
        {
            Debug.Log("Creating floor " + i);
            // creation of a floor object
            GameObject floorObj = new GameObject("Floor " + i);//name of the object
            floorObj.transform.SetParent(transform);
            Floor floor = floorObj.AddComponent<Floor>();
            currentDungeon.floors.Add(floor);

            // initialization fo the rooms of the current floor
            floor.Rooms = new Room[RoomsPerFloor.x, RoomsPerFloor.y];

            // population of each room
            for (int x = 0; x < RoomsPerFloor.x; x++)
            {
                for (int y = 0; y < RoomsPerFloor.y; y++)
                {
                    //creation of room GameObject
                    GameObject roomObj = new GameObject("Room " + x + ", " + y);
                    roomObj.transform.SetParent(floorObj.transform);
                    //making it a Room
                    Room room = roomObj.AddComponent<Room>();
                    floor.Rooms[x, y] = room;

                    room.RoomPosition = new Vector2Int(x, y);
                    room.Tiles = new Tile[RoomSize.x, RoomSize.y];

                    // looping throug the tiles of the room
                    for (int tilex = 0; tilex < RoomSize.x; tilex++)
                    {
                        for (int tiley = 0; tiley < RoomSize.y; tiley++)
                        {
                            //getting an empty tile
                            GameObject tilePrototype = TileSet.GetTilePrototype(TilePrototype.eTileID.Empty).PrefabObject;
                            //instantiating an positioning it in the world
                            Vector3 tilePosition = new Vector3(tilex, 0, tiley);
                            GameObject tileObj = GameObject.Instantiate(tilePrototype, tilePosition, Quaternion.identity);
                            tileObj.transform.SetParent(roomObj.transform);
                            room.Tiles[tilex, tiley] = tileObj.AddComponent<Tile>();
                            room.Tiles[tilex, tiley].Position = new Vector2Int(tilex, tiley);
                        }
                    }
                    roomObj.SetActive(false);
                }
            }
        }

        // ADDING FLOOR DORS
        foreach (Floor floor in currentDungeon.floors)
        {
            for (int x = 0; x < floor.Rooms.GetLength(0); x++)
            {
                for (int y = 0; y < floor.Rooms.GetLength(1); y++)
                {
                    Room room = floor.Rooms[x, y];
                    if (room != null)
                    {
                        Room neighbour = null;

                        // UP TO DOWN
                        if (room.UpDoor == null && RoomHasNeighbour(floor, room, Vector2Int.up, out neighbour))
                        {
                            Vector2Int tilePosition = new Vector2Int(room.Size.x / 2, room.Size.y - 1);
                            room.UpDoor = AddDoor(floor, room, tilePosition);
                            if (neighbour.DownDoor == null)
                            {
                                Vector2Int neighbourTilePosition = new Vector2Int(room.Size.x / 2, 0);
                                neighbour.DownDoor = AddDoor(floor, neighbour, neighbourTilePosition);
                            }

                            room.UpDoor.TargetDoor = neighbour.DownDoor;
                            neighbour.DownDoor.TargetDoor = room.UpDoor;
                        }


                        // DOWN TO UP
                        if (room.DownDoor == null && RoomHasNeighbour(floor, room, Vector2Int.down, out neighbour))
                        {
                            Vector2Int tilePosition = new Vector2Int(room.Size.x / 2, 0);
                            room.DownDoor = AddDoor(floor, room, tilePosition);
                            if (neighbour.UpDoor == null)
                            {
                                Vector2Int neighbourTilePosition = new Vector2Int(room.Size.x / 2, room.Size.y - 1);
                                neighbour.UpDoor = AddDoor(floor, neighbour, neighbourTilePosition);
                            }

                            room.DownDoor.TargetDoor = neighbour.UpDoor;
                            neighbour.UpDoor.TargetDoor = room.DownDoor;
                        }

                        // LEFT TO RIGHT
                        if (room.LeftDoor == null && RoomHasNeighbour(floor, room, Vector2Int.left, out neighbour))
                        {
                            Vector2Int tilePosition = new Vector2Int(0, room.Size.y / 2);
                            room.LeftDoor = AddDoor(floor, room, tilePosition);

                            if (neighbour.RightDoor == null)
                            {
                                Vector2Int neighbourTilePosition = new Vector2Int(room.Size.x - 1, room.Size.y / 2);
                                neighbour.UpDoor = AddDoor(floor, neighbour, neighbourTilePosition);
                            }

                            room.LeftDoor.TargetDoor = neighbour.RightDoor;
                            neighbour.RightDoor.TargetDoor = room.LeftDoor;
                        }

                        // RIGHT TO LEFT
                        if (room.RightDoor == null && RoomHasNeighbour(floor, room, Vector2Int.right, out neighbour))
                        {
                            Vector2Int tilePosition = new Vector2Int(room.Size.x - 1, room.Size.y / 2);
                            room.RightDoor = AddDoor(floor, room, tilePosition);

                            if (neighbour.LeftDoor == null)
                            {
                                Vector2Int neighbourTilePosition = new Vector2Int(0, room.Size.y / 2);
                                neighbour.LeftDoor = AddDoor(floor, neighbour, neighbourTilePosition);
                            }

                            room.RightDoor.TargetDoor = neighbour.LeftDoor;
                            neighbour.LeftDoor.TargetDoor = room.RightDoor;
                        }
                    }
                }
            }
        }

        // PLACING FLOOR UP DOOR

        for (int i =0; i < currentDungeon.floors.Count; i++)
        {
            bool placedFloorUp = false;
            do
            {
                if (TryGetRandomTile(currentDungeon.floors[i], out Tile tile, out Room room))
                {
                    //se siamo al primo livello
                    if (i == 0)
                    {
                        //imposto la posizione iniziale del personaggio nella room trovata
                        roomPosition = room.RoomPosition;
                        GameController.Instance.Player.SetPosition(tile.Position);
                        room.gameObject.SetActive(true);
                        CurrentRoom = room;
                        CurrentFloor = currentDungeon.floors[0];
                    }

                    //extracting a floorUpPrototype from TileSet
                    TilePrototype doorPrototype = TileSet.GetTilePrototype(TilePrototype.eTileID.FloorUp);
                    GameObject doorPrefab = doorPrototype.PrefabObject;
                    GameObject newDoorObj = GameObject.Instantiate(doorPrefab, tile.transform.position, Quaternion.identity);
                    newDoorObj.transform.SetParent(tile.transform);
                    Door door = newDoorObj.GetComponent<Door>();
                    door.Passable = true;
                    tile.MapObjects.Add(door);
                    currentDungeon.floors[i].FloorUpDoor = door;

                    if (i > 0)
                    {
                        door.TargetDoor = currentDungeon.floors[i - 1].FloorDownDoor;
                    }
                    else
                    {
                        currentDungeon.DungeonExitDoor = door;
                    }
                    placedFloorUp = true;
                }
            }
            while (!placedFloorUp);

            // PLACING FLOOR DOWN DOOR
            
            if (i < currentDungeon.floors.Count - 1)
            {
                bool placedFloorDown = false;
                do
                {
                    if (TryGetRandomTile(currentDungeon.floors[i], out Tile tile, out Room room))
                    {

                        //extracting a floorUpPrototype from TileSet
                        TilePrototype doorPrototype = TileSet.GetTilePrototype(TilePrototype.eTileID.FloorDown);

                        GameObject doorPrefab = doorPrototype.PrefabObject;
                        GameObject newDoorObj = GameObject.Instantiate(doorPrefab, tile.transform.position, Quaternion.identity);
                        newDoorObj.transform.SetParent(tile.transform);
                        Door door = newDoorObj.GetComponent<Door>();
                        door.Passable = true;
                        tile.MapObjects.Add(door);
                        currentDungeon.floors[i].FloorDownDoor = door;
                        door.TargetDoor = currentDungeon.floors[i + 1].FloorUpDoor;
                        placedFloorDown = true;
                    }

                } while (!placedFloorDown);
            }


            int spawnAttempts = 0;
            do
            {
                if (TryGetRandomTile(currentDungeon.floors[i], out Tile tile, out Room room))
                {
                    Monster monster = MonsterController.Instance.AddMonster(MonsterPrototype.eMonsterID.Slime, 1);
                    tile.MapObjects.Add(monster);
                    monster.SetPosition(tile.Position);
                    monster.gameObject.transform.SetParent(room.transform);
                }

                spawnAttempts++;

            } while (spawnAttempts < MobDensity);
        }
    }

    //==============================================================================
    /// <summary>
    /// returns a random tile in a random room in the given floor. returns true if the tile is found
    /// </summary>
    /// <param name="_floor">floor where you want to find the random tile</param>
    /// <param name="_tile">returned tile</param>
    /// <param name="_room">returned room</param>
    /// <returns>ritorna true se e' stato possibile trovare una tile </returns>
    bool TryGetRandomTile(Floor _floor, out Tile _tile, out Room _room)
    {
        Vector2Int pos = Vector2Int.zero;
        _tile = null;
        
        //finding random room
        _room = _floor.Rooms[Random.Range(0, _floor.Rooms.GetLength(0)), Random.Range(0, _floor.Rooms.GetLength(1))];
        if (_room == null)
        {
            return false;
        }

        //finding random tile
        pos = new Vector2Int(Random.Range(0, _room.Size.x), Random.Range(0, _room.Size.y));
        _tile = _room.Tiles[pos.x, pos.y];
        if (!_tile.IsPassable())
        {
            return false;
        }

        return true;
    }

    //==============================================================================
    /// <summary>
    /// tells if the room has a neighbour in a certain direction
    /// </summary>
    /// <param name="_floor"></param>
    /// <param name="_checkRoom">room you want to check</param>
    /// <param name="_direction">direction you want to check</param>
    /// <param name="_neighbour"></param>
    /// <returns></returns>
    bool RoomHasNeighbour(Floor _floor, Room _checkRoom, Vector2Int _direction, out Room _neighbour)
    {
        Vector2Int testPos = _checkRoom.RoomPosition + _direction;
        _neighbour = null;

        //  controls
        if (testPos.x < 0
            || testPos.y < 0
            || testPos.x >= RoomsPerFloor.x
            || testPos.y >= RoomsPerFloor.y)
        {
            return false;
        }

        if (_floor.Rooms[testPos.x, testPos.y] == null)
        {
            return false;
        }
        // fiding neighbour
        _neighbour = _floor.Rooms[testPos.x, testPos.y];
        return true;
    }

    //==============================================================================
    /// <summary>
    /// adds a door in the specified point, and returns it
    /// </summary>
    /// <param name="_floor"></param>
    /// <param name="_room"></param>
    /// <param name="_tilePosition"></param>
    /// <returns></returns>
    Door AddDoor(Floor _floor, Room _room, Vector2Int _tilePosition)
    {
        TilePrototype doorPrototype = TileSet.GetTilePrototype(TilePrototype.eTileID.Door);
        GameObject doorPrefab = doorPrototype.PrefabObject;
        Tile tile = _room.Tiles[_tilePosition.x, _tilePosition.y];
        GameObject newDoorObj = GameObject.Instantiate(doorPrefab, tile.transform.position, Quaternion.identity);
        Door door = newDoorObj.GetComponent<Door>();
        door.Floor = _floor;
        door.Room = _room;
        tile.MapObjects.Add(door);
        door.TilePosition = _tilePosition;
        newDoorObj.transform.SetParent(tile.gameObject.transform);
        door.Passable = true;
        return door;
    }

    //==============================================================================

    public void ExitDungeon()
    {
        CurrentRoom.gameObject.SetActive(false);
    }

    //==============================================================================
    public void EnterDungeon()
    {
        CurrentRoom.gameObject.SetActive(true);
    }
}
