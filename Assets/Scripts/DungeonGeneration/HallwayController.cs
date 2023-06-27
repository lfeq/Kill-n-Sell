using System;
using UnityEngine;

public class HallwayController : MonoBehaviour {
    [SerializeField] private GameObject northHallway, southHallway, eastHallway, westHallway;
    [SerializeField] private GameObject northDoor, southDoor, eastDoor, westDoor;

    private HallwayData m_hallwayData;

    public void setHallwayData(HallwayData t_hallwayData) {
        m_hallwayData = t_hallwayData;
    }

    public void setUpHallway() {
        transform.position = m_hallwayData.position;
        if (m_hallwayData.isGoingEast) {
            eastDoor.SetActive(false);
            eastHallway.SetActive(true);
        }

        if (m_hallwayData.isGoingNorth) {
            northDoor.SetActive(false);
            northHallway.SetActive(true);
        }

        if (m_hallwayData.isGoingSouth) {
            southDoor.SetActive(false);
            southHallway.SetActive(true);
        }

        if (m_hallwayData.isGoingWest) {
            westDoor.SetActive(false);
            westHallway.SetActive(true);
        }
    }
}