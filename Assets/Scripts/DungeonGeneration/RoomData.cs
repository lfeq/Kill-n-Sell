using UnityEngine;

[System.Serializable]
public class RoomData {
    public Vector3 position;
    public bool isConnectedEastSide = false;
    public bool isConnectedWestSide = false;
    public bool isConnectedSouthSide = false;
    public bool isConnectedNorthSide = false;
}
