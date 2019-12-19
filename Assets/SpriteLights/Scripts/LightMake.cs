using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LightMake : EditorWindow {

    UnityEngine.Rendering.IndexFormat meshIDXfmt = UnityEngine.Rendering.IndexFormat.UInt32; // Set to 16 for only 64k verts

    [ColorUsageAttribute(true,true)]
	Color lightColor = Color.white;

    Material lightsMaterial;
    Texture theTex;

    float size = 1f;
    float spacing = 30f;

    GameObject obj = null;
    MeshFilter selectedMeshFilter = null;

    //

    [MenuItem("JetJet/LightMake")]
    public static void ShowWindow()
    {
        GetWindow<LightMake>("LightMake");
    }

    void OnGUI()
    {



        EditorGUILayout.LabelField("Wireframe", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        lightsMaterial = EditorGUILayout.ObjectField(new GUIContent("Light Material"), lightsMaterial, typeof(Material), false) as Material;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        lightColor = EditorGUILayout.ColorField(new GUIContent("Light Color"), lightColor, false, true, true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        size = EditorGUILayout.FloatField("Light Size", size);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        spacing = EditorGUILayout.FloatField("Light Spacing", spacing);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        theTex = EditorGUILayout.ObjectField(new GUIContent("Texture"), theTex, typeof(Texture), false) as Texture;
        GUILayout.EndHorizontal();



        if(!Selection.activeGameObject)
        {
            EditorGUILayout.LabelField("Please select some,", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("game objects please,", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("before that you cannnot !!!,", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("you may not,", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("do WireFrame.", EditorStyles.boldLabel);
        }
        else if (GUILayout.Button("Make Wireframe"))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                MakeLights(obj);
            }
        }
     }


     private void OnDrawGizmos()
     {
         if(!Selection.activeGameObject) return;



     }

     // https://answers.unity.com/questions/566779/how-to-find-the-vertices-of-each-edge-on-mesh.html

     class Edge
     {
         public Vector3 v1;
         public Vector3 v2;

         public Edge(Vector3 v1, Vector3 v2)
         {
             if (v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z <= v2.z))))
             {
                 this.v1 = v1;
                 this.v2 = v2;
             }
             else
             {
                 this.v1 = v2;
                 this.v2 = v1;
             }
         }

         public Vector3 FromTo()
         {
             return (v2 - v1);
         }

         public float Length()
         {
             return FromTo().magnitude;
         }
     }

    // Edge[] GetMeshEdges(Mesh mesh)
    // {
    //     HashSet<Edge> edges = new HashSet<Edge>();
    //
    //     for (int i = 0; i < mesh.triangles.Length; i += 3)
    //     {
    //         var v1 = mesh.vertices[mesh.triangles[i]];
    //         var v2 = mesh.vertices[mesh.triangles[i + 1]];
    //         var v3 = mesh.vertices[mesh.triangles[i + 2]];
    //         edges.Add(new Edge(v1, v2));
    //         edges.Add(new Edge(v1, v3));
    //         edges.Add(new Edge(v2, v3));
    //     }
    //
    //     return edges.ToArray();
    // }
    //


     void MakeLights(GameObject obj){

 		List<Vector3> lightData = new List<Vector3>();
        Debug.Log(obj.name);

        selectedMeshFilter = (MeshFilter)obj.GetComponent("MeshFilter");
        Mesh mesh = selectedMeshFilter.sharedMesh;


        List<Vector3> VertexList = new List<Vector3>();
        foreach(Vector3 vertex in mesh.vertices)
           VertexList.Add(vertex);

        VertexList = VertexList.Distinct().ToList();

        foreach(Vector3 v in VertexList)
        {
            lightData.Add( worldPos(v, obj) );
        }

        HashSet<Edge> edges = new HashSet<Edge>();
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {

            var v1 = worldPos(mesh.vertices[mesh.triangles[i]], obj);
            var v2 = worldPos(mesh.vertices[mesh.triangles[i + 1]], obj);
            var v3 = worldPos(mesh.vertices[mesh.triangles[i + 2]], obj);

            Edge e1 = new Edge(v1, v2);
            Edge e2 = new Edge(v1, v3);
            Edge e3 = new Edge(v2, v3);

            // Discard long edge of triangle
            if ( e1.Length() >= e2.Length() ) {
                if (e1.Length() >= e3.Length())
                {
                    edges.Add(e2);
                    edges.Add(e3);
                }
                else
                {
                    edges.Add(e1);
                    edges.Add(e2);
                }
            }
            else {
                if ( e2.Length() >= e3.Length() )
                {
                    edges.Add(e1);
                    edges.Add(e3);
                }
                else
                {
                    edges.Add(e1);
                    edges.Add(e2);
                }
            }

            // edges.Add(e1);
            // edges.Add(e2);
            // edges.Add(e3);

        }


        // edges = edges.Distinct().ToList();

        foreach(Edge edg in edges)
        {

    		int numLights = (int)Mathf.Ceil( edg.Length() / spacing);
    		Vector3 currentPosition = edg.v1;
    		Vector3 offsetVec = Math3d.SetVectorLength( edg.FromTo(), spacing);

    		for (int e = 0; e < numLights; e++) {
    			lightData.Add(currentPosition);
    			currentPosition += offsetVec;
    		}

        }

//
// for (item in hash.Key){ Debug.Log(hash.Key.toString()); }
        //
        // Edge[] edg = GetMeshEdges(mesh);
        // for (int i = 0; i < edg.Length; i++)
        // {
        //
        //     Debug.Log(edg[i]);
        //
        //     // Vector3 wpos = obj.transform.TransformPoint(vertices[i]);
        //     // lightData.Add(wpos);
        // }

        // Iterate over the vertex positions and UVs of the mesh.
        // var vertices = mesh.vertices;
        // var uvs = mesh.uv;
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     // At each vertex, sample the elevation texture at the corresponding UV coordinate.
        //     int x = Convert.ToInt32(uvs[i].x * ElevationTexture.width);
        //     int y = Convert.ToInt32(uvs[i].y * ElevationTexture.height);
        //     Color color = ElevationTexture.GetPixel(x, y);
        //
        //     // Convert the resulting color value to an elevation in meters.
        //     float elevation = ColorToElevation(color);
        //
        //     // Use the tile size in meters at the given zoom level to determine the relative
        //     // scale of elevation values in the mesh.
        //     const double earthCircumferenceMeters = 6378137.0 * Math.PI * 2.0;
        //     double tileSize = earthCircumferenceMeters / (1 << ZoomLevel);
        //     double height = elevation / tileSize;
        //     vertices[i].y = (float)height;
        // }





 		//Loop through
 		// foreach (Mapity.Highway highway in Mapity.Singleton.highways.Values){
 		//
 		// 	for(int i = 0; i < highway.wayMapNodes.Count - 1; i++){
 		//
 		// 		//Get the from-to nodes
 		// 		Mapity.MapNode fromNode = (Mapity.MapNode)highway.wayMapNodes[i];
 		// 		Mapity.MapNode toNode = (Mapity.MapNode)highway.wayMapNodes[i+1];
 		//
 		// 		//Get the road segment start and end point.
 		// 		Vector3 from = fromNode.position.world.ToVector();
 		// 		Vector3 to = toNode.position.world.ToVector();
 		// 		Vector3 fromToVec = to - from;
 		// 		float length = fromToVec.magnitude;
 		// 		int lightAmount = (int)Mathf.Ceil(length / spacing);
 		// 		Vector3 currentPosition = from;
 		//
 		// 		//Get a translation vector
 		// 		Vector3 offsetVec = Math3d.SetVectorLength(fromToVec, spacing);
 		//
 		//
 		// 		//Place light at a certain interval
 		// 		for (int e = 0; e < lightAmount; e++) {
 		//
 		// 			lightData.Add(currentPosition);
 		// 			currentPosition += offsetVec;
 		// 		}
 		//
 		// 	}
 		// }

 		//Create the lights
 		GameObject[] lightObjects = SpriteLights.CreateLights(obj.name + " Lights ", lightData.ToArray(), size, lightsMaterial, meshIDXfmt);

        //Parent
 		for (int i = 0; i < lightObjects.Length; i++)
            lightObjects[i].transform.parent = obj.transform;

 		//Create the mesh and prefab
 		CreateAssets(lightObjects, obj.name);

 	}

    Vector3 worldPos(Vector3 vec, GameObject obj)
    {
        return obj.transform.TransformPoint(vec);
    }


     void CreateAssets(GameObject[] lightObjects, string name) {

 		for (int i = 0; i < lightObjects.Length; i++) {

 			string meshPrefix = name + " Lights";
            string meshName = meshPrefix + " 0";

 			//Does the file alreadys exists?
            bool hasFilename = false;
 			string[] guids = AssetDatabase.FindAssets(meshName + " t:mesh");

 			if (guids.Length == 0) {
                hasFilename = true;
            }
            else
            {
 				int index = 1;

 				while (!hasFilename) {
 					//Try another file name
 					meshName = meshPrefix + " " + index.ToString();
 					guids = AssetDatabase.FindAssets(meshName + " t:mesh");
 					if (guids.Length == 0)
 						hasFilename = true;

                    if(index > 1000) {
                        Debug.Log("Over 1000 light meshes for this individual model! Reduce lightmeshes in /Assets/Lights/");
                        break;
                    }

 					index++;
 				}
 			}

 			//Save the mesh
            if(hasFilename)
            {
                AssetDatabase.CreateAsset(lightObjects[i].GetComponent<MeshFilter>().sharedMesh, "Assets/Lights/" + meshName + ".asset");
     			AssetDatabase.SaveAssets();
            }

 			//Save the game object
 			// //PrefabUtility.CreatePrefab("Assets/Lights/" + meshName + ".prefab", lightObjects[i]);
            //PrefabUtility.SaveAsPrefabAsset(lightObjects[i], "Assets/Lights/" + meshName + ".prefab");
         }
 	}

}
