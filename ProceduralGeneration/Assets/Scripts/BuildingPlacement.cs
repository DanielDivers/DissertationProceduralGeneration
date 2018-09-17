using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingPlacement : MonoBehaviour {

    public Transform ObjTrans;
    public GameObject Obj;

    public int numberOfObjects; // number of objects to place
    private int currentObjects; // number of placed objects

    List<GameObject> objectList = new List<GameObject>();
    public float beginheight, endheight;

    public void AddObjects(bool redo, TerrainControl terrainControl)
    {
        //we want to delete the previously placed trees when generating new ones
        if (redo == true)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                DestroyImmediate(objectList[i]);
                //DestroyObject(objectList[i]);
            }
            objectList.Clear();
            redo = false;
            currentObjects = 0;
        }

        //make sure beginheight is always less than end height so it doesnt crash
        if (beginheight >= endheight)
        {
            beginheight = endheight - 0.1f;
        }

        while (currentObjects < numberOfObjects)
        {
            // generate random x position
            int posx = Random.Range(0, 0 + terrainControl.mapSize);
            // generate random z position
            int posz = Random.Range(0, 0 + terrainControl.mapSize);

            float height = terrainControl.mapData[posx, posz];

            // get the terrain height at the random position
            float posy = terrainControl.meshHeightCurve.Evaluate(height) * terrainControl.meshHeightMultiplier;

            posx *= 5; posy *= 5; posz *= 5;

            Collider[] intersecting = Physics.OverlapSphere(new Vector3(posx, posy, -posz), 1.5f);

            //want to check whether or not there is already a game object placed at that location
            if (intersecting.Length == 0)
            {
                if (posy / 5 >= beginheight && posy / 5 <= endheight && currentObjects < numberOfObjects)
                {
                    // create new gameObject on random position
                    GameObject newObject = (GameObject)Instantiate(Obj, new Vector3(posx, posy + 1, -posz), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                    newObject.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                    objectList.Add(newObject);
                    currentObjects += 1;
                } 
            }
        }

        //add a mesh collider to each object, since these prefabs are several parts we need to assign the mesh collider to the Box001 mesh
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Box001")
            {
                gameObj.AddComponent(typeof(MeshCollider));
            }
        }

        //set all objects parent to mesh "easier for looking at at moving all at the same time"
        for (int i = 0; i < objectList.Count; i++)
        {        
            //GameObject.Find("Box001").AddComponent(typeof(MeshCollider));
            objectList[i].transform.SetParent(GameObject.FindWithTag("TerrainMesh").transform);  
        }
    }
}