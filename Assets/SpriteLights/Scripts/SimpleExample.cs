using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleExample : MonoBehaviour {

	public Material material;

	// Use this for initialization
	void Start () {
		
		//The amount of lights to create.
		int amount = 5;

		//Initialize the SpriteLights system.
		SpriteLights.Init(0, 0, Camera.main.fieldOfView, Screen.height);

		//Create a lightData array.
		SpriteLights.LightData[] lightData = new SpriteLights.LightData[amount];

		//Loop through each lightData struct in the array to set it up.
		for (int i = 0; i < amount; i++)
		{
			//Create a new lightData array.
			lightData[i] = new SpriteLights.LightData();

			//Set the size of the light.
			lightData[i].size = 1;

			//Set the color of the front facing light.
			lightData[i].frontColor = Color.red;

			//Set the color of the back facing light.
			lightData[i].backColor = Color.green;

			//Set the rotation of the light (which way it is facing).
			lightData[i].rotation = Quaternion.Euler(new Vector3(0, 0, 1));

			//Set the position of the light.
			lightData[i].position = new Vector3(0, 0, i);
		}

		//Create the light mesh.
		SpriteLights.CreateLights("SomeLights", lightData, material, UnityEngine.Rendering.IndexFormat.UInt16);
	}
	

}
