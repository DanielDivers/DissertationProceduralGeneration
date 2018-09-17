using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        GameObject TerrainMesh = GameObject.Find("Terrain");

        if(DrawDefaultInspector())
        {     
            mapGen.DrawMapInEditor();           
        }

        if(EditorApplication.isPlaying)
        {
            mapGen.DrawMapInEditor();
        }

        if(GUILayout.Button("Generate Map"))
        {        
            mapGen.DrawMapInEditor();

            //rebuild the collider so the map always has the correct one
            if (TerrainMesh.GetComponent<MeshCollider>() != null)
            {
                DestroyImmediate(TerrainMesh.GetComponent<MeshCollider>());
            }
            TerrainMesh.AddComponent<MeshCollider>();
        }

        if (GUILayout.Button("Generate Trees"))
        {
            //want to remove the mesh collider from the TerrainMesh so that we can place objects
            if(TerrainMesh.GetComponent<MeshCollider>() != null)
            {
                DestroyImmediate(TerrainMesh.GetComponent<MeshCollider>());
            }
            mapGen.objectPlacement.AddObjects(true, mapGen.terrainControl);
            TerrainMesh.AddComponent<MeshCollider>();
        }

        if (GUILayout.Button("Generate Buildings"))
        {
            //want to remove the mesh collider from the TerrainMesh so that we can place objects
            if (TerrainMesh.GetComponent<MeshCollider>() != null)
            {
                DestroyImmediate(TerrainMesh.GetComponent<MeshCollider>());
            }
            mapGen.buildingPlacement.AddObjects(true, mapGen.terrainControl);
            TerrainMesh.AddComponent<MeshCollider>();
        }
    }
}
