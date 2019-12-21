using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CityMake))]
public class CityMakeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CityMake theCityMake = (CityMake)target;

        Color sv_color = GUI.color;
        GUIStyle style = new GUIStyle(GUI.skin.button);

        //
        GUILayout.Space(10);

        GUI.color = Color.cyan;
        style.normal.textColor = Color.blue;
        if( GUILayout.Button("Compile City", style) )
            theCityMake.Compile();

        GUILayout.Space(2);

        GUI.color = Color.white;
        style.normal.textColor = Color.red;
        if( theCityMake.gameObject.transform.Find("_Build") )
            if( GUILayout.Button("Clear", style) )
                theCityMake.Decompile();

        GUILayout.Space(10);
        //

        GUI.color = sv_color;

        DrawDefaultInspector();
    }
}
