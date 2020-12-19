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
