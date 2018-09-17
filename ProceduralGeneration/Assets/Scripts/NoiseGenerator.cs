using System.Collections;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] NoiseMap(int mapSize, TerrainControl terrainControl)
    {
        float[,] noiseMap = new float[mapSize, mapSize];

        float amplitude, frequency, height, targetX, targetY, noiseValue;
        
        //give values one that will be changed in if statement
        float maxHeight = 0f;
        float minHeight = 1f;
        
        //for every position
        for (int y = 0; y < mapSize; y++)
        {
            for(int x = 0; x < mapSize; x++)
            {
                //reset variables to default values
                amplitude = 1f;
                frequency = 1;
                height = 0;

                //do for every octave
                for (int i = 0; i < terrainControl.octaves; i++)
                {
                    targetX = x  / terrainControl.noiseScale * frequency;
                    targetY = y  / terrainControl.noiseScale * frequency;

                    noiseValue = Mathf.PerlinNoise(targetX + terrainControl.seed, targetY + terrainControl.seed);
                    height += noiseValue * amplitude;

                    amplitude *= terrainControl.GetPersistance();
                    frequency *= terrainControl.GetLacunarity();
                }

                if(height > maxHeight)
                {
                    maxHeight = height;
                }
                else if(height < minHeight)
                {
                    minHeight = height;
                }

                //assign this value so we can use it in our lerp function
                //noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, height);
                noiseMap[x, y] = height;
            }
        }

        //this has to be done seperately since the min and max height can change
        //doing noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, height);
        //in main loop causes aborations around edges
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
