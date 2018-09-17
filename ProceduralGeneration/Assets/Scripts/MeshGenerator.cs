using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public static MeshInfo CreateMesh(TerrainControl terrainControl)
    {
        //width and height are the same size since we use an assigned mapSize
        int size = terrainControl.mapData.GetLength(0);

        MeshInfo meshInfo = new MeshInfo(size);

        int numVertices = 0;
        int numTriangles = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                meshInfo.vertices[numVertices] = new Vector3(x, terrainControl.meshHeightCurve.Evaluate(terrainControl.mapData[x, y]) * terrainControl.meshHeightMultiplier, -y);
                meshInfo.uvs[numVertices] = new Vector2(x / size, y / size);

                if (x < size - 1 && y < size - 1)
                {
                    meshInfo.triangles[numTriangles] = numVertices;
                    numTriangles++;
                    meshInfo.triangles[numTriangles] = numVertices + size + 1;
                    numTriangles++;
                    meshInfo.triangles[numTriangles] = numVertices + size;
                    numTriangles++;
                    meshInfo.triangles[numTriangles] = numVertices + size + 1;
                    numTriangles++;
                    meshInfo.triangles[numTriangles] = numVertices;
                    numTriangles++;
                    meshInfo.triangles[numTriangles] = numVertices + 1;
                    numTriangles++;
                }
                numVertices++;
            }
        }

        return meshInfo;
    }
}

//container to hold our nessecary mesh information
public class MeshInfo
{
    public Vector3[] vertices;

    //unity mesh uvs is Vector 2 so we need the same here
    public Vector2[] uvs;
    public int[] triangles;

    public MeshInfo(int size)
    {
        vertices = new Vector3[size * size];
        uvs = new Vector2[size * size];
        triangles = new int[size * size * 6];
    }
}