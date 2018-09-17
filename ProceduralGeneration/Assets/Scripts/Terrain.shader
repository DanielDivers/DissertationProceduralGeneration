Shader "Custom/Terrain" {
	
	Properties{
		textures("Terrain Texture Array", 2DArray) = "white" {}
	}

	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		//Thanks to https://www.youtube.com/watch?v=XjH-UoyaTgs for the tutorial on getting the 
		//custom terrain shader working, and having it update with changed inputs

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		const static int maxRegions = 8;

		float minHeight, maxHeight;
		float heights[maxRegions];
		float blends[maxRegions];
		float textureScales[maxRegions];

		int regionCount;

		UNITY_DECLARE_TEX2DARRAY(textures);

		//we want to acces the world pos and worldNormal from the Input struct from surface shader 
		//"Must be named in accordance with documentation... https://docs.unity3d.com/Manual/SL-SurfaceShaders.html"
		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};

		//called for every pixel that is visible
		void surf (Input input, inout SurfaceOutputStandard surfaceOutput)
		{
			float3 blendAxis = abs(input.worldNormal);

			//*5 because we have scaled our terrain mesh
			float height = saturate((input.worldPos.y - (minHeight * 5)) / ((maxHeight * 5) - (minHeight * 5)));

			for (int i = 0; i < regionCount; i++)
			{
				float drawStrength = ((height - heights[i]) + (blends[i] / 2)) / (blends[i]);
				
				//saturate to clamp value between 0 and 1
				drawStrength = saturate(drawStrength);

				//get a scaled position so we can apply correctly
				float3 pos = input.worldPos / textureScales[i];

				//we want to make sure that if drawStrength == 0, that Albedo is set to its previous value rather than black
				surfaceOutput.Albedo = surfaceOutput.Albedo * (1 - drawStrength) + UNITY_SAMPLE_TEX2DARRAY(textures, float3(pos.x, pos.z, i)) * drawStrength;
			}

		}
		ENDCG
	}
	FallBack "Diffuse"
}
