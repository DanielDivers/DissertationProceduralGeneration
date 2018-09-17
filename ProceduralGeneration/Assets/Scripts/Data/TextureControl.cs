using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class TextureControl : ScriptableObject
{
    public bool update;
    public Region[] regions;

    float minHeight, maxHeight;
    public event System.Action ValuesUpdatedAction;

    //we want to update the values on the mesh, when there are changes
    protected virtual void OnValidate()
    {
        if (update)
        {
            UnityEditor.EditorApplication.update += ValuesUpdated;
        }
    }

    public void ValuesUpdated()
    {
        //need to subtract it or the application runs incredibly slowly
        UnityEditor.EditorApplication.update -= ValuesUpdated;

        if (ValuesUpdatedAction != null)
        {
            ValuesUpdatedAction();
        }
    }

    public void UpdateTextures(float min, float max, Material mat)
    {
        minHeight = min;
        maxHeight = max;

        Texture2D[] textures = regions.Select(x => x.texture).ToArray();
        Texture2DArray textureArray = new Texture2DArray(512, 512, textures.Length, TextureFormat.RGB565, true);

        for (int i = 0; i < textures.Length; i++)
        {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        textureArray.Apply();
        
        //set the material information from the regions in editor
        mat.SetTexture("textures", textureArray);
        mat.SetFloat("minHeight", minHeight);
        mat.SetFloat("maxHeight", maxHeight);
        mat.SetInt("regionCount", regions.Length);
        mat.SetFloatArray("heights", regions.Select(x => x.startHeight).ToArray());
        mat.SetFloatArray("blends", regions.Select(x => x.blendStrength).ToArray());
        mat.SetFloatArray("textureScales", regions.Select(x => x.textureScale).ToArray());
    }

    //our class for editing variables in the editor
    [System.Serializable]
    public class Region
    {
        [Range(0, 1)]
        public float startHeight, blendStrength;
        public float textureScale;

        public Texture2D texture;        
    }
}
