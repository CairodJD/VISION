using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hidden Planet", menuName = "ScriptableObjects/Hidden Planet", order = 1)]
public class PlanetName : ScriptableObject {
    public string hiddenName;
    [Tooltip("Planet biome percent config in this order (Base/Eau/Gaz/Terre)")]
    public float[] config;

}

