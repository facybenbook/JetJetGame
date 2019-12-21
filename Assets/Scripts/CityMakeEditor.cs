using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CityMake))]
public class CityMakeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CityMake myCityMake = (CityMake)target;

        if( GUILayout.Button("Compile City") )
        {
            myCityMake.PerlinMake();
        }

        DrawDefaultInspector();
    }
}
