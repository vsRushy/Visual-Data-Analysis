using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationManager : MonoBehaviour
{
    private static VisualizationManager _instance;
    public static VisualizationManager Instance { get { return _instance; } }

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
    //public GraphFilter graph_filter = GraphFilter.Activate_Switch_1; // If we have time? xd

    public GameObject tileObj;
    private Transform holder;

    private GameObject[,] grid;

    public int width = 10;
    public int height = 10;
    public float tileSize = 0.9f;
    public Gradient colorGradient;
    public int maxCounts = 50;
    public Vector3 offset;
    private int eventCounts = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        grid = new GameObject[width,height];
        GenerateVisualization();
    }

    public void GenerateVisualization()
    {
        grid = new GameObject[width, height];
        // Generate the grid 
        holder = RecreateHolder(); // Deletes the old grid
        GenerateGridObjects();
        List<Eventinfo> eventList = GetListByUser();

        for(int i = 0; i < eventList.Count; ++i)
        {
            // Filter the event
            if ((int)eventList[i].type != (int)heatmap_filter)
                continue;

            // Get the position
            Vector3 position = eventList[i].position;

            // Get the tile depending on posX and posY
            // Add value (eventually color) to that tile obj
            // We can even do height (scale.y) depending on that value - pretty cool
            AddValue(grid[(int)position.x, (int)position.z]);

        }
    }
    private void GenerateGridObjects()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                // Instantiate the tile
                GameObject newObj = Instantiate(tileObj, new Vector3(x, 0, y) + offset, Quaternion.identity);
                newObj.transform.parent = holder;
                newObj.transform.localScale = new Vector3(tileSize, 0.2f, tileSize);
                // Change its material so each  tile has its own that can be customized
                Material myNewMaterial = new Material(newObj.GetComponent<Renderer>().material);
                myNewMaterial.color = Color.white;
                // TODO: Add transparency
                newObj.GetComponent<Renderer>().material = myNewMaterial;
                //  Set each object to the correspondent grid
                grid[x, y] = newObj; // ???????
            }
        }
    }

    public void AddValue(GameObject tile)
    {
        Color newColor = new Color();
        Color oldColor = tile.GetComponent<Renderer>().material.color;
        // If no value hasnt entered yet, we paint it green, so we can later on interpolate between green and red
        if (oldColor == Color.white)         
        {
            newColor = Color.green;
            tile.GetComponent<Renderer>().material.color = colorGradient.Evaluate(0f);
        }
        else    // We move from green to red
        {
            float f = Mathf.Clamp01((float)eventCounts / maxCounts);
            newColor = colorGradient.Evaluate(f);
            //newColor = Color.Lerp(oldColor, Color.red, 0.2f);
            //tile.GetComponent<Renderer>().material.color = newColor;
        }
    }

    private List<Eventinfo> GetListByUser()
    {
        if (EventManager.eventManager != null)
        {
            // Gets the list from the reader / event manager
            List<Eventinfo> retList = EventManager.eventManager.GetListByUser(user_filter);

            // Just to notify if the list is empty
            if(retList.Count == 0)
                Debug.Log("There are no items in the list");

            return retList;
        }
        Debug.LogError("Event Manager doesn't exist!");
        return null;
    }
  
    private Transform RecreateHolder()
    {
        string holderName = "VisualizationHolder";
        if (GameObject.Find(holderName))
        {
            DestroyImmediate(GameObject.Find(holderName));
        }

        Transform newHolder = new GameObject(holderName).transform;
        newHolder.parent = this.transform;
        return newHolder;
    }
}
