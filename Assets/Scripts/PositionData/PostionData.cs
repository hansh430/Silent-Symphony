using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionData", menuName = "SpawnPostions", order = 1)]
public class PostionData : ScriptableObject
{
    public List<Vector3> SpawnPointPostions = new List<Vector3>();
}
