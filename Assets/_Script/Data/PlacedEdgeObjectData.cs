using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Data saved when we place an Edge object on our grid
/// </summary>
public class PlacedEdgeObjectData
{
    public IEnumerable<Edge> PositionsOccupied { get; private set; }
    public int gameObjectIndex, structureID;
    public Vector3Int origin;
    public Quaternion rot;
    public PlacedEdgeObjectData(int gameObjectIndex, int structureID, IEnumerable<Edge> positionsOccupied, Vector3Int origin, Quaternion rot)
    {
        this.gameObjectIndex = gameObjectIndex;
        this.PositionsOccupied = positionsOccupied;
        this.structureID = structureID;
        this.origin = origin;
        this.rot = rot;
    }

}
