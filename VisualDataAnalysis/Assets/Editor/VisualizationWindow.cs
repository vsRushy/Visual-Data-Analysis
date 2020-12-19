using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class VisualizationWindow : EditorWindow
{
    public enum User
    {
        All = 0,
        Peter,
        Carlos,
        Marc,
        Gerard,
        Sebi
    }

    public enum GraphType
    {
        HEATMAP = 0,
        GRAPH
    }

    public enum HeatmapFilter
    {
        POSITION = 0,
        ATTACK,
        JUMP,
        DEATH,
        RECIEVE_DMG,
        ENEMY_KILLED,

    }
    public enum GraphFilter
    {
        Activate_Switch_1 = 0,
        Activate_Switch_2,
        Activate_Switch_3,
        WIN_GAME,
        ENTER_GAME
    }

    public User user_filter = User.All;
    public GraphType graph_type = GraphType.HEATMAP;
    public HeatmapFilter heatmap_filter = HeatmapFilter.POSITION;
    public GraphFilter graph_filter = GraphFilter.Activate_Switch_1;
    
    static int flags = 0;
    static string[] options = new string[] { "Switch 1", "Switch 2", "Switch 3", "End" };
    [MenuItem("Window/Data Visualization")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(VisualizationWindow));
    }
    void OnGUI()
    {
        GUILayout.Label("User Filter", EditorStyles.boldLabel);
        user_filter = (User)EditorGUILayout.EnumPopup("User", user_filter);

        graph_type = (GraphType)EditorGUILayout.EnumPopup("Graph Type", graph_type);

        if (graph_type == GraphType.HEATMAP)
        {
            heatmap_filter = (HeatmapFilter)EditorGUILayout.EnumPopup("Filter", heatmap_filter);
        }
        else
        {
            graph_filter = (GraphFilter)EditorGUILayout.EnumPopup("Filter", graph_filter);
        }

        if (GUILayout.Button("Generate Visualization"))
        {
            //VisualizationManager.Instance.GenerateVisualization();
        }


        flags = EditorGUILayout.MaskField("Player Flags", flags, options);
        //UnityEngine.Debug.Log(flags);
        //myString = EditorGUILayout.TextField("Text Field", myString);

        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        //EditorGUILayout.EndToggleGroup();
    }
}
