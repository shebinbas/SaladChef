using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vegetables", menuName = "ScriptableObjects/VegetablesScriptableObject", order = 1)]
public class VegetableObjects : ScriptableObject
{
    public List<GameObject> vegetables;
}
