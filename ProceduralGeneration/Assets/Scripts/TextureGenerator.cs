using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{ 
    public static Texture2D heightMapTexture(float[,] heightMap)
    {
        //only need size since width and height are the same
        int size = heightMap.GetLength(0);
      
        Texture2D texture = new Texture2D(size, size);
        Color[] colours = new Color[size * size];

        //give each position a colour in the gradient of black to white
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                colours[x * size + y] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        texture.SetPixels(colours);
        texture.Apply();

        return texture;
    }
}
