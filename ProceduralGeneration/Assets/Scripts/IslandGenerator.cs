using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator {
	
    public static float[,] GenerateIsland(int size)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                //transform into range -1 to 1, so middle of map is 0
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;
                                            
                //this would make a circular island (not working) 
                /*float distance_x = Mathf.Abs(i - size * 0.5f);
                float distance_y = Mathf.Abs(j + size * 0.5f);
                float distance = Mathf.Sqrt(distance_x * distance_x + distance_y * distance_y); // circular mask

                float max_width = size * 0.5f - 80.0f;
                float delta = distance / max_width;
                float gradient = delta * delta;

                map[i, j] = Mathf.Max(-1.0f, 1.0f - gradient);*/

                //find out which value is closer to 1 (edge of map)
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                float a = 3f;
                float b = 2f;

                //use our sigmoid type algorithm to determine proper adjusted value
                map[i, j] = Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));

            }
        }
        return map;
    }
}
