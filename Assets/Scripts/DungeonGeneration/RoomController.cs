using UnityEngine;

public class RoomController : MonoBehaviour {
    [SerializeField] private GameObject doorNorth, doorSouth, doorEast, doorWest;
    
    private RoomData m_roomData;

    public void setRoomData(RoomData t_roomData) {
        m_roomData = t_roomData;
    }

    public void setUpRoom() {
        transform.position = m_roomData.position;
        if (m_roomData.isConnectedEastSide) {
            doorEast.SetActive(false);
        }
        if (m_roomData.isConnectedNorthSide) {
            doorNorth.SetActive(false);
        }
        if (m_roomData.isConnectedSouthSide) {
            doorSouth.SetActive(false);
        }
        if (m_roomData.isConnectedWestSide) {
            doorWest.SetActive(false);
        }
    }
}
