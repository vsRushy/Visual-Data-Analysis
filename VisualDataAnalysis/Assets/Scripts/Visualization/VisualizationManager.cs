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
    public int width = 10;
    public int height = 10;
    public float tileSize = 0.9f;
    public GameObject tileObj;
    public Gradient colorGradient;
    [Range(0, 1)]
    public float alpha = 0.5f;
    public Vector3 offset;
    private int maxCounts = 0;

    private Transform holder;
    public class HeatObject
    {
        public int eventCounts = 0;
        public GameObject tile = null;

        public HeatObject(GameObject obj, int eventC = 0)
        {
            tile = obj;
            eventCounts = eventC;
        }
        public HeatObject()
        {
        }
    }

    private HeatObject[,] grid;

   
    
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
        GenerateVisualization();
    }

    public void GenerateVisualization()
    {
        Debug.Log(" --------------------------- Generating new visualization ---------------------------");
        // Deletes the old grid
        holder = RecreateHolder(); 
        // Generate the grid 
        GenerateGridObjects();
        // Apply values from the event list
        AddHeatValues();
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
                myNewMaterial.color = Color.white - new Color(0,0,0, 1-alpha);
                newObj.GetComponent<Renderer>().material = myNewMaterial;

                //  Set each object to the correspondent grid
                grid[x, y].tile = newObj;
            }
        }
    }

    private void AddHeatValues()
    {
        // Firstly we get the max count of events of type X
        List<Eventinfo> eventList = GetListByUser();
        for (int i = 0; i < eventList.Count; ++i)
        {
            // Filter the event
            if ((int)eventList[i].type != (int)heatmap_filter)
                continue;

            maxCounts++;
        }
        Debug.Log("There are a total of " + maxCounts + " events in the category: " + heatmap_filter);

        // Then we proceed to actually add those values
        for (int i = 0; i < eventList.Count; ++i)
        {
            // Filter the event
            if ((int)eventList[i].type != (int)heatmap_filter)
                continue;

            // Get the position
            Vector3 position = eventList[i].position;

            // Get the tile depending on posX and posY
            Debug.Log("Pushing value to: " + (int)position.x + "," + (int)position.y);
            int correctedX = (int)position.x - (int)offset.x;
            int correctedZ = (int)position.z - (int)offset.z;
            Debug.Log("Recorrected toto: " + correctedX + "," +correctedZ);
            AddValue(grid[correctedX, correctedZ]);
        }
    }
    private void AddValue(HeatObject heatObject)
    {
        // Add the counter on this heat object
        ++heatObject.eventCounts;

        // Tint the tile correspondently
        Color oldColor = heatObject.tile.GetComponent<Renderer>().material.color;
        // If no value hasnt entered yet, we paint it green, so we can later on interpolate between green and red
        if (oldColor == Color.white)         
        {
            heatObject.tile.GetComponent<Renderer>().material.color = colorGradient.Evaluate(0f);
        }
        else // We move from green to red
        {
            float f = Mathf.Clamp01((float)heatObject.eventCounts/ maxCounts);
            heatObject.tile.GetComponent<Renderer>().material.color = colorGradient.Evaluate(f) - new Color(0, 0, 0, 1 - alpha);
        }
    }

    // Mouse picking option to get the actual HeatObject
    public void AddValue(GameObject objectHit)
    {
        for(int x = 0; x < width; ++x)
        {
            for(int y =0; y< height; ++y)
            {
                if(grid[x,y].tile == objectHit)
                {
                    Debug.Log("Found that clicked tile in: " + x + "," + y);
                    AddValue(grid[x, y]);
                    return;
                }
            }
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
        // Reset counts
        maxCounts = 0;

        // Initialize grid
        grid = new HeatObject[width, height];

        for(int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                grid[x, y] = new HeatObject();
            }
        }

        // Recreate the actual holder
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
