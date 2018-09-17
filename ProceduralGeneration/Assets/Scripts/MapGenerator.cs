using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Renderer textureRenderer;
    public MeshRenderer meshRenderer;

    public TerrainControl terrainControl;
    public TextureControl textureControl;

    public Material terrainMaterial;

    public ObjectPlacement objectPlacement;
    public BuildingPlacement buildingPlacement;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;

        //divide the width and height my map size so we can get a reasonably sized texture to view
        textureRenderer.transform.localScale = new Vector3(texture.width / terrainControl.mapSize * 10, 1, texture.height / terrainControl.mapSize * 10);
    }
    public void DrawMesh(MeshInfo meshInfo)
    {
        Mesh mesh = new Mesh();

        mesh.vertices = meshInfo.vertices;
        mesh.triangles = meshInfo.triangles;
        mesh.uv = meshInfo.uvs;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    void ValuesUpdatedAction()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
        textureControl.UpdateTextures(terrainControl.minHeight, terrainControl.maxHeight, terrainMaterial);
    }

    public void DrawMapInEditor()
    {
        terrainControl.mapData = GenerateMapData();
        DrawTexture(TextureGenerator.heightMapTexture(terrainControl.mapData));
        DrawMesh(MeshGenerator.CreateMesh(terrainControl));
    }

    float[,] GenerateMapData()
    {
        float[,] noiseMap = NoiseGenerator.NoiseMap(terrainControl.mapSize, terrainControl);
      
        //if were using the island map 
        if(terrainControl.useIsland)
        {        
            terrainControl.IslandMap = IslandGenerator.GenerateIsland(terrainControl.mapSize);           
        }

        for(int y = 0; y < terrainControl.mapSize; y ++)
        {
            for(int x = 0; x < terrainControl.mapSize; x ++)
            {
                //if were using the IslandMap the apply it
                if(terrainControl.useIsland)
                {
                    //combine both maps, and clamp value between 0-1
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - terrainControl.IslandMap[x, y]);
                }
            }
        }

        textureControl.UpdateTextures(terrainControl.minHeight, terrainControl.maxHeight, terrainMaterial);

        return noiseMap;
    }

    void OnValidate()
    {
        //used for updating values in relation to system.action variable
        if(terrainControl != null)
        {
            terrainControl.ValuesUpdatedAction -= ValuesUpdatedAction;
            terrainControl.ValuesUpdatedAction += ValuesUpdatedAction;
        }
        if (textureControl != null)
        {
           textureControl.ValuesUpdatedAction -= ValuesUpdatedAction;
           textureControl.ValuesUpdatedAction += ValuesUpdatedAction;
        }
    }
}