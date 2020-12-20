using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisualizationManager))]
public class VisualizationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VisualizationManager visualization = target as VisualizationManager;
        if (DrawDefaultInspector())
        {
            //visualization.GenerateVisualization();
        }

        GUILayout.Space(5);
        GUILayout.Label(" --- Only on Play mode --- ");
        if (GUILayout.Button("Generate HeatMap"))
        {
            visualization.GenerateVisualization();
        }
        if (GUILayout.Button("Destroy HeatMap"))
        {
            visualization.DestroyHeatMap();
        }
    }
}
