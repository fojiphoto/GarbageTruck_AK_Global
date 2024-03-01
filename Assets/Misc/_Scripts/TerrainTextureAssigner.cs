using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureAssigner : MonoBehaviour {

	//Skyboxes
	public Material skyboxSnow;
	public Material skyboxMountain;

	public GameObject snow;

	public GameObject directionalLight; //Mode1
	public GameObject directionalLightSnow;//Mode2

	// Grass-ATLAS 
	public Material Grass_alpha_blend1Material;   //Grass_alpha_blend 1
	public Texture Grass_AtlasTextureGreen;
	public Texture Grass_AtlasTextureSnow;

	// Grass-ATLAS 2
	public Material Grass_alpha_blend2Material;  //Grass_alpha_blend 1
	public Texture Grass_Atlas2TextureGreen;
	public Texture Grass_Atlas2TextureSnow;

	// Trees 
	public Material treeMaterial; //Trees_atlas1_leafs (drag this one material)
	public Texture snowTreeTexture;
	public Texture greenTreeTexture;

	// Rocks
	public Material rockMaterial,rockMaterial2,rockMaterial3,rockMaterial4,rockMaterial5,rockMaterial6,rockMaterial7,rockMaterial8,rockMaterial9;
	public Texture rockTextureMode1;
	public Texture rockTextureMode2,rockTextureMode12,rockTextureMode13,rockTextureMode14,rockTextureMode15,rockTextureMode16,rockTextureMode17,rockTextureMode18,rockTextureMode19;
//	public Texture [] rock;
//	public Material[] Rock;
	//TERRAIN

	public Texture2D[] TerrainTexturesMode1;
	public Texture2D[] TerrainTexturesMode2; //snow
	public Texture2D[] TerrainTexturesMode3;
	public TerrainData terraindata,terraindata2;
	public Terrain terrain,terrain2;

	//Road Textures
	public Material easyRoadMaterial;
	public Texture2D asphaltRoadTexture;
	public Texture2D snowDirtRoad;
	public Texture2D tread_diff;

 void Start(){
		
		int mode = PlayerPrefs.GetInt ("mode");

		if (mode == 1) {
			Mode1();
		}
		if (mode == 2) {
			Mode2();
		}
		if (mode==3) {
			Mode3 ();
		}
		//Mode3 ();
	}

	public void Mode1()
	{
		//ROAD
		easyRoadMaterial.mainTexture = asphaltRoadTexture;
		easyRoadMaterial.color = new Color32 (255,255,255,255);
		easyRoadMaterial.mainTextureScale = new Vector2 (1f,1f);

		//SKYBOX
		RenderSettings.skybox = skyboxSnow;

		// SNOW
		snow.SetActive (false);

		//FOG 
		RenderSettings.fog = false;

		//GRASS 1
		Grass_alpha_blend1Material.SetTexture("_MainTex", Grass_AtlasTextureGreen);

		//GRASS 2
		Grass_alpha_blend2Material.SetTexture("_MainTex",  Grass_Atlas2TextureGreen);

		// DIRECTIONAL lIGHT
		directionalLight.SetActive (true);

		// ROCK MATERIALS
		rockMaterial.mainTexture = rockTextureMode1;    // adjusting rock texture
		rockMaterial2.mainTexture = rockTextureMode12;
		rockMaterial3.mainTexture = rockTextureMode13;
		rockMaterial4.mainTexture = rockTextureMode14;
		rockMaterial5.mainTexture = rockTextureMode15;
		rockMaterial6.mainTexture = rockTextureMode16;
		rockMaterial7.mainTexture = rockTextureMode17;
		rockMaterial8.mainTexture = rockTextureMode18;
		rockMaterial9.mainTexture = rockTextureMode19;
		// TREE MATERIALS
		treeMaterial.SetTexture ("_MainTex", greenTreeTexture);   // assigning green tree texture
		treeMaterial.SetColor("_TintColor", new Color32(255,240,176,116));   // adjusting tint color of gree tree texture


		// HANDLING TEXTURES
		SplatPrototype[] tex = new SplatPrototype [TerrainTexturesMode1.Length];
		for (int i = 0; i < TerrainTexturesMode1.Length; i++) 
		{
			tex [i] = new SplatPrototype ();
			tex [i].texture = TerrainTexturesMode1 [i];    //Sets the texture

			if (i == 0) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Ground 9texture
			}
			if (i == 1) {
				tex [i].tileSize = new Vector2 (5, 5);    //Sets the size of the Ground 7 texture
			}
			if (i == 2) {
				tex [i].tileSize = new Vector2 (10, 5);    //Sets the size of the Ground 4 texture
			}
			if (i == 3) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Dirt 05 Seamless texture
			}
			if (i==4) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the WATER texture    
			}
		}
	
		terraindata.splatPrototypes = tex;
		terraindata2.splatPrototypes = tex;
		//terrain = Terrain.CreateTerrainGameObject (terraindata).GetComponent<Terrain> ();

		//this.gameObject.GetComponent<TerrainTextureAssigner> ().enabled = false;

	}

	public void Mode2()  // SNOW MODE
	{
		// DIRECTIONAL lIGHT
		directionalLight.SetActive (false);
		directionalLightSnow.SetActive (true);
		
		//ROAD
		easyRoadMaterial.mainTexture = snowDirtRoad;
		easyRoadMaterial.color = new Color32 (225,225,225,255);
		easyRoadMaterial.mainTextureScale = new Vector2 (1f,0.25f);
		
		//SKYBOX
		RenderSettings.skybox = skyboxSnow;

		// SNOW
		snow.SetActive (true);

		//FOG 
		RenderSettings.fog = true;

		//GRASS 1
		Grass_alpha_blend1Material.SetTexture("_MainTex", Grass_AtlasTextureSnow);

		//GRASS 2
		Grass_alpha_blend2Material.SetTexture("_MainTex",  Grass_Atlas2TextureSnow);

		//SNOW  texture on rocks

		rockMaterial.mainTexture = rockTextureMode2;
		rockMaterial2.mainTexture = rockTextureMode2;
		rockMaterial3.mainTexture = rockTextureMode2;
		rockMaterial4.mainTexture = rockTextureMode2;
		rockMaterial5.mainTexture = rockTextureMode2;
		rockMaterial6.mainTexture = rockTextureMode2;
		rockMaterial7.mainTexture = rockTextureMode2;
		rockMaterial8.mainTexture = rockTextureMode2;
		rockMaterial9.mainTexture = rockTextureMode2;
		// SNOW Texture on trees
		treeMaterial.SetTexture ("_MainTex", snowTreeTexture);  // asiigning snow texture to trees
		//treeMaterial.SetColor("_TintColor", new Color32(255,255,255,116));  //snow   
		treeMaterial.SetColor("_TintColor", new Color32(248,248,248,116));
	

		// Handling Terrain Textures
		SplatPrototype[] tex = new SplatPrototype [TerrainTexturesMode2.Length];
		for (int i = 0; i < TerrainTexturesMode2.Length; i++) {
			tex [i] = new SplatPrototype ();
			tex [i].texture = TerrainTexturesMode2 [i];    //Sets the texture

			if (i == 0) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Ground 9texture
			}
			if (i == 1) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Ground 7 texture
			}
			if (i == 2) {
				tex [i].tileSize = new Vector2 (10, 5);    //Sets the size of the Ground 4 texture
			}
			if (i == 3) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Dirt 05 Seamless texture
			}
			if (i==4) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the WATER texture    
			}
		}

		terraindata.splatPrototypes = tex;
		terraindata2.splatPrototypes = tex;
		//terrain = Terrain.CreateTerrainGameObject (terraindata).GetComponent<Terrain> ();


	//	this.gameObject.GetComponent<TerrainTextureAssigner> ().enabled = false;
	}

	public void Mode3()
	{
		// DIRECTIONAL lIGHT
		directionalLight.SetActive (true);
		directionalLightSnow.SetActive (false);

		//ROAD
		easyRoadMaterial.mainTexture = tread_diff;
		easyRoadMaterial.color = new Color32 (191,191,191,255);
		easyRoadMaterial.mainTextureScale = new Vector2 (1f,1f);

		//SKYBOX
		RenderSettings.skybox = skyboxMountain;

		// SNOW
		snow.SetActive (false);

		//FOG 
		RenderSettings.fog = false;

		//GRASS 1
		Grass_alpha_blend1Material.SetTexture("_MainTex", Grass_AtlasTextureGreen);

		//GRASS 2
		Grass_alpha_blend2Material.SetTexture("_MainTex",  Grass_Atlas2TextureGreen);

		// DIRECTIONAL lIGHT
		directionalLight.SetActive (true);
		directionalLightSnow.SetActive (false);

		// ROCK MATERIALS
		rockMaterial.mainTexture = rockTextureMode1;    // adjusting rock texture
		rockMaterial2.mainTexture = rockTextureMode12;
		rockMaterial3.mainTexture = rockTextureMode13;
		rockMaterial4.mainTexture = rockTextureMode14;
		rockMaterial5.mainTexture = rockTextureMode15;
		rockMaterial6.mainTexture = rockTextureMode16;
		rockMaterial7.mainTexture = rockTextureMode17;
		rockMaterial8.mainTexture = rockTextureMode18;
		rockMaterial9.mainTexture = rockTextureMode19;
		// TREE MATERIALS
		treeMaterial.SetTexture ("_MainTex", greenTreeTexture);   // assigning green tree texture
		treeMaterial.SetColor("_TintColor", new Color32(255,240,176,116));   // adjusting tint color of gree tree texture


		// HANDLING TEXTURES
		SplatPrototype[] tex = new SplatPrototype [TerrainTexturesMode3.Length];
		for (int i = 0; i < TerrainTexturesMode3.Length; i++) {
			tex [i] = new SplatPrototype ();
			tex [i].texture = TerrainTexturesMode3 [i];    //Sets the texture

			if (i == 0) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Ground 9texture
			}
			if (i == 1) {
				tex [i].tileSize = new Vector2 (5, 5);    //Sets the size of the Ground 7 texture
			}
			if (i == 2) {
				tex [i].tileSize = new Vector2 (10, 5);    //Sets the size of the Ground 4 texture
			}
			if (i == 3) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the Dirt 05 Seamless texture
			}
			if (i==4) {
				tex [i].tileSize = new Vector2 (10, 10);    //Sets the size of the WATER texture    
			}
		}

		//terraindata.splatPrototypes = tex;
		//terraindata2.splatPrototypes = tex;
		//terrain = Terrain.CreateTerrainGameObject (terraindata).GetComponent<Terrain> ();

		//this.gameObject.GetComponent<TerrainTextureAssigner> ().enabled = false;

	}
}
