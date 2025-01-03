﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "World Type Preset")]
public class WorldType : ScriptableObject
{
    public string presetName;

    public Gradient groundGradient;

    [Header("Generation")]
    public bool flat;
    public float waterLevel;
    public float noiseZoom;
    public float noiseScale;

    [Header("Soil")]
    public float defaultSoilHealth;
    public float defaultSoilWater;
    public float defaultSoilSunlight;

    [Header("Rock Generation")]
    public bool generateRocks;
    public GameObject rockPrefab;
    public float rockThreshold;
    public int allowedNumberOfRocks;

    [Header("Obelisk")]
    public bool spawnObelisk;
    public GameObject obeliskPrefab;
}