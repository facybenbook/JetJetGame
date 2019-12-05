using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class Fabber : EditorWindow
{
	[MenuItem("Fabber/Create Prefabs From Selection")]
	static void CreatePrefab()
	{
		GameObject[] objs = Selection.gameObjects;

		foreach (GameObject go in objs)
		{
			string localPath = "Assets/" + go.name + ".prefab";
			if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
			{
				if (EditorUtility.DisplayDialog("Are you sure?",
					"The prefab already exists. Do you want to overwrite it?",
					"Yes",
					"No"))
				{
					CreateNew(go, localPath);
				}
			}
			else
			{
				Debug.Log(go.name + " Prefab Created");
				CreateNew(go, localPath);
			}
		}
	}

	[MenuItem("Fabber/Create Prefabs From Selection", true)]
	static bool ValidateCreatePrefab()
	{
		return Selection.activeGameObject != null;
	}

	static void CreateNew(GameObject obj, string localPath)
	{
		Object prefab = PrefabUtility.CreateEmptyPrefab(localPath);
		PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
	}
}
#endif
