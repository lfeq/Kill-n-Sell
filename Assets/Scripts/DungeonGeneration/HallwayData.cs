using System;
using UnityEngine;

[System.Serializable]
public class HallwayData {
    public Vector3 position;
    public bool isGoingEast;
    public bool isGoingSouth;
    public bool isGoingWest;
    public bool isGoingNorth;
}

public enum HallwayDirection {
    IsGoingEast,
    IsGoingSouth,
    IsGoingWest,
    IsGoingNorth
}