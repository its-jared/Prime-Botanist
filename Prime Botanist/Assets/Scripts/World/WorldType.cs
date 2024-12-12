using System;
using UnityEngine;

[CreateAssetMenu(fileName = "World Type Preset")]
public class WorldType : ScriptableObject
{
    public string presetName;

    [Header("Colors")]
    public Color deepGroundColor;
    public Color shallowGroundColor;

    [Header("Generation")]
    public bool flat;
    public float waterLevel;
    public float noiseZoom;
    public float noiseScale;

    [Header("Rock Generation")]
    public bool generateRocks;
    public GameObject rockPrefab;
    public float rockThreshold;
    public int allowedNumberOfRocks;
}