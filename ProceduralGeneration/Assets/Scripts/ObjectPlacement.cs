using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPlacement : MonoBehaviour
{
    public Transform ObjTrans;
    public GameObject[] Obj;

    public int numberOfObjects; // number of objects to place
    private int currentObjects; // number of placed objects

    List<GameObject> objectList = new List<GameObject>();
    int randScale;

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
        if(beginheight >= endheight)
        {
            beginheight = endheight - 0.1f;
        }

        //loops through every vertice on the map 
        /*for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                rand = Random.Range(0, 1000);
                float height = heightmap[x, y];

                if (rand > 980)
                {
                    if (height >= darkGrass && height <= stone)
                    {
                        ObjTrans.SetPositionAndRotation(new Vector3(x, heightCurve.Evaluate(height) * heightMultiplier, -y), new Quaternion(0, 0, 0, 0));
                        objectList.Add(Instantiate(Obj, ObjTrans.transform.position, ObjTrans.transform.rotation));
                    }
                }
                if (rand > 995)
                {
                    if (height >= grass && height <= darkGrass)
                    {
                        ObjTrans.SetPositionAndRotation(new Vector3(x, heightCurve.Evaluate(height) * heightMultiplier, -y), new Quaternion(0, 0, 0, 0));
                        objectList.Add(Instantiate(Obj, ObjTrans.transform.position, ObjTrans.transform.rotation));
                    }
                }
            }    
        }*/

        while (currentObjects < numberOfObjects)
        {
            // generate random x position
            int posx = Random.Range(0, 0 + terrainControl.mapSize);
            // generate random z position
            int posz = Random.Range(0, 0 + terrainControl.mapSize);

            int objType = Random.Range(0, Obj.Count());

            float height = terrainControl.mapData[posx, posz];
         
            // get the terrain height at the random position
            float posy = terrainControl.meshHeightCurve.Evaluate(height) * terrainControl.meshHeightMultiplier;

            randScale = Random.Range(3, 5);

            posx *= 5; posy *= 5; posz *= 5;

            Collider[] intersecting = Physics.OverlapSphere(new Vector3(posx, posy, -posz), 0.5f);

          
            //want to check whether or not there is already a game object placed at that location
            if (intersecting.Length == 0)
            {
                if (posy/5 >= beginheight && posy/5 <= endheight && currentObjects < numberOfObjects)
                {
                    // create new gameObject on random position
                    GameObject newObject = (GameObject)Instantiate(Obj[objType], new Vector3(posx , posy, -posz), Quaternion.identity);
                    newObject.AddComponent(typeof(MeshCollider));
                    newObject.transform.localScale = new Vector3(randScale/2, randScale/2, randScale / 2);
                    objectList.Add(newObject);
                    currentObjects += 1;
                }
            }  
        }

        //set all objects parent to mesh "easier for looking at at moving all at the same time"
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].transform.SetParent(GameObject.FindWithTag("TerrainMesh").transform);
        }
    }
}