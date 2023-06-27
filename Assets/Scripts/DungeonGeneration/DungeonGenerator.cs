using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour {
    private int[,] m_grid;
    private List<RoomData> m_roomDatas = new List<RoomData>();
    private List<HallwayData> m_hallwayDatas = new List<HallwayData>();
    private int m_currentRooms;
    private GameObject spawnRoom, exitRoom;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject hallwayPrefab;
    [SerializeField, Range(2, 99)] private int maxRooms = 5;
    [SerializeField, Range(2, 99)] private int minRooms = 3;
    [SerializeField, Range(3, 99)] private int dungeonWidth = 3;
    [SerializeField, Range(3, 99)] private int dungeonHeight = 3;
    [SerializeField] private GameObject playerPrefab, finishPrefab;
    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

    private void Awake() {
        createDungeon();
        createRooms();
        createHallways();
        fillRoomData();
        fillHallwayData();
        spawnDungeonRoomsAndHallways();
        spawnPlayer();
        spawnFinish();
    }

    /// <summary>
    /// Create a grid with specified width and height, and minimum rooms and maximum rooms.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void createDungeon() {
        if (minRooms > maxRooms) {
            throw new Exception("Minimum value cannot be higher than maximum value");
        }

        m_grid = new int[dungeonWidth, dungeonHeight];
        while (minRooms > m_currentRooms) {
            m_currentRooms = 0;
            populateGrid();
        }
    }

    /// <summary>
    /// Assigns grid position value.
    /// 0: empty position.
    /// 1: room position.
    /// </summary>
    private void populateGrid() {
        for (int i = 0; i < m_grid.GetLength(0); i++) {
            for (int j = 0; j < m_grid.GetLength(1); j++) {
                if (m_currentRooms >= maxRooms) continue;
                m_grid[i, j] = roomValue();
                if (m_grid[i, j] == 1)
                    m_currentRooms++;
            }
        }
    }

    /// <summary>
    /// Creates the data that is going to be used to create a room.
    /// Only assigns position.
    /// </summary>
    private void createRooms() {
        for (int i = 0; i < m_grid.GetLength(0); i++) {
            for (int j = 0; j < m_grid.GetLength(1); j++) {
                //print(grid[i, j] + " " + i + "," + j);
                if (m_grid[i, j] != 1) continue;
                Vector3 spawnPosition = new Vector3(i * 20, 0, j * 20);
                //GameObject tempRoom = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
                RoomData roomData = new RoomData {
                    position = spawnPosition
                };
                m_roomDatas.Add(roomData);
            }
        }
    }

    /// <summary>
    /// Picks a random room and fills the path to that room
    /// with hallway.
    /// </summary>
    private void createHallways() {
        foreach (RoomData room in m_roomDatas) {
            List<RoomData> tempRooms = new List<RoomData>(m_roomDatas);
            tempRooms.Remove(room);
            int randomRoomNumber = Random.Range(0, tempRooms.Count);
            RoomData randomRoom = tempRooms[randomRoomNumber];
            Vector3 distance = randomRoom.position - room.position;
            if (distance.x != 0 && distance.z != 0) {
                createLHallway(room.position, distance);
            } else if (distance.x != 0) {
                createHorizontalHallway(room.position, distance);
            } else if (distance.z != 0) {
                createVerticalHallway(room.position, distance);
            }
        }
    }

    /// <summary>
    /// Picks a random value between 0 and 1.
    /// 0 is an empty room.
    /// 1 is where a room is going to spawn.
    /// </summary>
    /// <returns>random value between 0 and 1</returns>
    private int roomValue() {
        return Random.Range(0, 2);
        // 0 Empty
        // 1 Room
    }

    /// <summary>
    /// Spawns hallways horizontally until one reaches a room.
    /// </summary>
    /// <param name="t_initialPosition"></param>
    /// <param name="t_direction"></param>
    private void createHorizontalHallway(Vector3 t_initialPosition, Vector3 t_direction) {
        int roomsToCreate = (int)t_direction.x / 10;
        roomsToCreate = Mathf.Abs(roomsToCreate);
        switch (t_direction.x) {
            case > 0: {
                    for (int i = 0; i < roomsToCreate; i++) {
                        float xPos = t_initialPosition.x + i * 10;
                        Vector3 spawnPos = new Vector3(xPos, t_initialPosition.y, t_initialPosition.z);
                        spawnHallway(spawnPos);
                    } //Spawn hallways to the right

                    break;
                }
            case < 0: {
                    for (int i = 0; i < roomsToCreate; i++) {
                        float xPos = t_initialPosition.x + i * -10;
                        Vector3 spawnPos = new Vector3(xPos, t_initialPosition.y, t_initialPosition.z);
                        spawnHallway(spawnPos);
                    } //Spawn hallways to the left

                    break;
                }
        }
    }

    /// <summary>
    /// Spawns hallways vertically until one reaches a room.
    /// </summary>
    /// <param name="t_initialPosition"></param>
    /// <param name="t_direction"></param>
    private void createVerticalHallway(Vector3 t_initialPosition, Vector3 t_direction) {
        int roomsToCreate = (int)t_direction.z / 10;
        roomsToCreate = Mathf.Abs(roomsToCreate);
        switch (t_direction.z) {
            case > 0: {
                    for (int i = 0; i < roomsToCreate; i++) {
                        float zPos = t_initialPosition.z + i * 10;
                        Vector3 spawnPos = new Vector3(t_initialPosition.x, t_initialPosition.y, zPos);
                        spawnHallway(spawnPos);
                    } //Spawn hallways to the right

                    break;
                }
            case < 0: {
                    for (int i = 0; i < roomsToCreate; i++) {
                        float zPos = t_initialPosition.z + i * -10;
                        Vector3 spawnPos = new Vector3(t_initialPosition.x, t_initialPosition.y, zPos);
                        spawnHallway(spawnPos);
                    } //Spawn hallways to the left

                    break;
                }
        }
    }

    /// <summary>
    /// Spawns hallways in an L shape.
    /// Picks randomly L direction.
    /// </summary>
    /// <param name="t_initialPosition"></param>
    /// <param name="t_direction"></param>
    private void createLHallway(Vector3 t_initialPosition, Vector3 t_direction) {
        if (Random.Range(0f, 1f) < 0.5f) {
            Vector3 newInitialPos = new Vector3(t_initialPosition.x + t_direction.x, t_initialPosition.y,
                t_initialPosition.z);
            createHorizontalHallway(t_initialPosition, t_direction);
            createVerticalHallway(newInitialPos, t_direction);
        } //Spawn hallways horizontally and then vertically
        else {
            Vector3 newInitialPos = new Vector3(t_initialPosition.x, t_initialPosition.y,
                t_initialPosition.z + t_direction.z);
            createVerticalHallway(t_initialPosition, t_direction);
            createHorizontalHallway(newInitialPos, t_direction);
        } //Spawn hallways vertically and then horizontally
    }

    /// <summary>
    /// Creates the data that is going to be used to create a hallway.
    /// Only assigns position.
    /// </summary>
    private void spawnHallway(Vector3 t_spawnPos) {
        if (m_roomDatas.Any(t_roomData => t_roomData.position == t_spawnPos)) {
            return;
        }

        if (m_hallwayDatas.Any(t_hallwayData => t_hallwayData.position == t_spawnPos)) {
            return;
        }

        //GameObject tempHallway = Instantiate(hallway, spawnPos, quaternion.identity);
        HallwayData tempHallwayData = new HallwayData {
            position = t_spawnPos
        };
        m_hallwayDatas.Add(tempHallwayData);
    }

    /// <summary>
    /// Fills room data.
    /// </summary>
    private void fillRoomData() {
        foreach (RoomData roomData in m_roomDatas) {
            Vector3 roomPosition = roomData.position;
            foreach (HallwayData hallwayData in m_hallwayDatas) {
                Vector3 roomPositionOffset = new Vector3(roomPosition.x + 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == hallwayData.position) {
                    roomData.isConnectedEastSide = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x - 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == hallwayData.position) {
                    roomData.isConnectedWestSide = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z + 10);
                if (roomPositionOffset == hallwayData.position) {
                    roomData.isConnectedNorthSide = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z - 10);
                if (roomPositionOffset == hallwayData.position) {
                    roomData.isConnectedSouthSide = true;
                }
            }
        }
    }

    /// <summary>
    /// Fills hallway data.
    /// </summary>
    private void fillHallwayData() {
        foreach (HallwayData hallwayData in m_hallwayDatas) {
            Vector3 roomPosition = hallwayData.position;
            List<HallwayData> hallWayDataCopy = new List<HallwayData>(m_hallwayDatas);
            hallWayDataCopy.Remove(hallwayData);
            foreach (HallwayData tempHallwayData in hallWayDataCopy) {
                Vector3 roomPositionOffset = new Vector3(roomPosition.x + 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == tempHallwayData.position) {
                    hallwayData.isGoingEast = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x - 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == tempHallwayData.position) {
                    hallwayData.isGoingWest = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z + 10);
                if (roomPositionOffset == tempHallwayData.position) {
                    hallwayData.isGoingNorth = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z - 10);
                if (roomPositionOffset == tempHallwayData.position) {
                    hallwayData.isGoingSouth = true;
                }
            }

            foreach (RoomData roomData in m_roomDatas) {
                Vector3 roomPositionOffset = new Vector3(roomPosition.x + 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == roomData.position) {
                    hallwayData.isGoingEast = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x - 10, roomPosition.y, roomPosition.z);
                if (roomPositionOffset == roomData.position) {
                    hallwayData.isGoingWest = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z + 10);
                if (roomPositionOffset == roomData.position) {
                    hallwayData.isGoingNorth = true;
                }

                roomPositionOffset = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z - 10);
                if (roomPositionOffset == roomData.position) {
                    hallwayData.isGoingSouth = true;
                }
            }
        }
    }

    private void spawnDungeonRoomsAndHallways() {
        List<GameObject> dungeonRooms = new List<GameObject>();
        foreach (RoomData roomData in m_roomDatas) {
            GameObject tempRoom = Instantiate(roomPrefab, Vector3.zero, quaternion.identity);
            RoomController tempRoomController = tempRoom.GetComponent<RoomController>();
            tempRoomController.setRoomData(roomData);
            tempRoomController.setUpRoom();
            dungeonRooms.Add(tempRoom);
        }

        foreach (HallwayData hallwayData in m_hallwayDatas) {
            GameObject tempHallway = Instantiate(hallwayPrefab, Vector3.zero, quaternion.identity);
            HallwayController tempHallwayController = tempHallway.GetComponent<HallwayController>();
            tempHallwayController.setHallwayData(hallwayData);
            tempHallwayController.setUpHallway();
            dungeonRooms.Add(tempHallway);
        }

        spawnRoom = dungeonRooms[Random.Range(0, dungeonRooms.Count)];
        dungeonRooms.Remove(spawnRoom);
        exitRoom = dungeonRooms[Random.Range(0, dungeonRooms.Count)];
    }

    private void spawnPlayer() {
        Vector3 spawnPos = spawnRoom.transform.position + new Vector3(0, 1, 0);
        GameObject tempPlayer = Instantiate(playerPrefab, spawnPos, quaternion.identity);
        m_virtualCamera.Follow = tempPlayer.transform;
    }

    private void spawnFinish() {
        Vector3 spawnPos = exitRoom.transform.position + new Vector3(0, 1, 0);
        GameObject tempFinish = Instantiate(finishPrefab, spawnPos, Quaternion.identity);
    }
}