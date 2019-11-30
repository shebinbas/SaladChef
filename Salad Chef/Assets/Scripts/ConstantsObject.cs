using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constants", menuName = "ScriptableObjects/ConstantsObject", order = 1)]
public class ConstantsObject : ScriptableObject
{
    public int Player1Id;
    public int Player2Id;
    public int DefaultSpeed;
    public int MaximumPickingVegetabels;
    public float PlayerTime;
    public float CustomerTime;
    public int CustomerExitTime;
    public Vector3 VegetableSpawnPoint;
    public int StartIndex;
    public int TotalPlayers;
    public int DefaultScore;
    public int DefaultPenality;
    public GameObject SaladObject;
    public GameObject HappyObject;
    public GameObject AngryObject;
}
