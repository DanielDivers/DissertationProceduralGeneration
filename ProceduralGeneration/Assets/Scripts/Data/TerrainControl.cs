using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainControl : ScriptableObject
{
    public event System.Action ValuesUpdatedAction;
    public bool update;

    public int mapSize = 255;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool useIsland;

    public float[,] IslandMap;
    public float[,] mapData;

    [Range(1, 6)]
    public int octaves;

    //we dont want these values to be changed, they look the best at the set value
    float persistance = 0.424f;
    float lacunarity = 2.0f;

    public float noiseScale;
    public int seed;

    public float minHeight
    {
        get
        {
            return meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }
    public float maxHeight
    {
        get
        {
            return meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }
    public float GetPersistance()
    {
        return persistance;
    }

    public float GetLacunarity()
    {
        return lacunarity;
    }

    protected void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        //make sure the scale can never be <= 0
        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        if (update)
        {
            UnityEditor.EditorApplication.update += ValuesUpdated;
        }
    }

    public void ValuesUpdated()
    {
        UnityEditor.EditorApplication.update -= ValuesUpdated;

        if (ValuesUpdatedAction != null)
        {
            ValuesUpdatedAction();
        }
    }
}
