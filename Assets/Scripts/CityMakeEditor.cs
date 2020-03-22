
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CityMake))]
public class CityMakeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CityMake citymake = (CityMake)target;

        Color guicolor = GUI.color;
        GUIStyle style = new GUIStyle(GUI.skin.button);


        //


        // gather
        GUILayout.Space(4);
        EditorGUILayout.LabelField("Gather", EditorStyles.boldLabel);

        GUI.color = Color.white;
        style.normal.textColor = Color.black;
        if( GUILayout.Button("Gather prefabs in underworld", style) )
        {
            citymake.DetectPrefabs();
            EditorUtility.SetDirty(target);
        }




        // show
        GUILayout.Space(6);
        EditorGUILayout.LabelField("Show", EditorStyles.boldLabel);

        if(!citymake.IsShowPlacements())
        {
            GUI.color = Color.white;
            style.normal.textColor = Color.black;
            if( GUILayout.Button("Show Placements", style) )
            {
                citymake.ShowPlacements();
                EditorUtility.SetDirty(target);
            }

        }
        else
        {
            GUI.color = Color.grey;
            style.normal.textColor = Color.black;
            if( GUILayout.Button("Hide Placements", style) )
            {
                citymake.HidePlacements();
                EditorUtility.SetDirty(target);
            }

        }



        // make
        GUILayout.Space(6);
        EditorGUILayout.LabelField("Make", EditorStyles.boldLabel);

        if(!citymake.IsCompiled())
        {
            GUI.color = Color.cyan;
            style.normal.textColor = Color.blue;
            if( GUILayout.Button("Compile City", style) )
            {
                citymake.Compile();
                EditorUtility.SetDirty(target);
            }

        }
        else
        {
            GUI.color = Color.grey;
            style.normal.textColor = Color.black;

            if( GUILayout.Button("Clear City", style) )
            {
                citymake.Decompile();
                EditorUtility.SetDirty(target);
            }

        }




        GUILayout.Space(20);
        //

        GUI.color = guicolor;

        DrawDefaultInspector();
    }
}
