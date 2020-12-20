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
    public enum HeatmapFilter
    {
        POSITION = 0,
        ATTACK,
        JUMP,
        DEATH,
        RECIEVE_DMG,
        ENEMY_KILLED,
    }

    // ------------------------ Filters ------------------------
    public User user_filter = User.All;
    public HeatmapFilter heatmap_filter = HeatmapFilter.POSITION;

    // ------------------------ Customization ------------------------
    public int width = 10;
    public int height = 10;
    public float tileSize = 0.9f;
    public bool useHeight = false;
    public float maxHeight = 5f;
    public Gradient colorGradient;
    public GameObject tileObj;
    [Range(0, 1)]
    public float alpha = 0.5f;
    public Vector3 offset;

    // ------------------------ Logic ------------------------
    private int maxCounts = 0;
    private int individualMaxCounts = 0;
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
        //GenerateVisualization();
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
        // 2 rounds:
        // The first one to actually have the relative 100% to calculate color and height 
        // The second one to actually apply values relative to that 100%
       
        //List<Eventinfo> eventList = GetListByUser(user_filter);    // TODO: Uncomment when reader works
        List<Eventinfo> eventList = EventManager.events;

        // Firstly we get the max count of events of type X and the actual max Count of an individual tile
        for (int i = 0; i < eventList.Count; ++i)
        {
            // Filter the event
            if ((int)eventList[i].type != (int)heatmap_filter)
                continue;

            // Get the grid object
            HeatObject gridObject = GetGridObjectByPosition((int)eventList[i].position.x, (int)eventList[i].position.z);
            if (gridObject == null)
                continue;

            // Increase the total counts of the event read
            maxCounts++;
            
            // Increase the grid object's "counts" (times its been pushed)
            gridObject.eventCounts++;

            // Overwrite the max counts if its a bigger value than what we got so far 
            if (gridObject.eventCounts >= individualMaxCounts)
                individualMaxCounts = gridObject.eventCounts;
        }
        Debug.Log("There are a total of " + maxCounts + " events in the category: " + heatmap_filter);
        Debug.Log("The most pushed has " + individualMaxCounts + " events pushed. That will be our 100%");

        // Then we proceed to actually add those values
        for (int i = 0; i < eventList.Count; ++i)
        {
            // Filter the event
            if ((int)eventList[i].type != (int)heatmap_filter)
                continue;

            // Get the object in the grid
            HeatObject gridObject = GetGridObjectByPosition((int)eventList[i].position.x, (int)eventList[i].position.z);
            if (gridObject == null)
                continue;

            // Finally push the event 
            AddValue(gridObject);
        }
    }
    private void AddValue(HeatObject heatObject, bool fromOutside = false)
    {
        if (fromOutside)                // Corrupts the color/size relationship - This is for the mouse picking option
            ++heatObject.eventCounts;   // -> done in the AddHeatValues 

        TintObject(heatObject);

        if (useHeight)
            EnlargeObject(heatObject);
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
                    AddValue(grid[x, y], true);
                    return;
                }
            }
        }
    }

    private HeatObject GetGridObjectByPosition(int x, int z)
    {
        // Get the tile depending on posX and posY + the offset set to the manager
        int correctedX = x - (int)offset.x;
        int correctedZ = z - (int)offset.z;

        // Check the given value its contained in our grid
        if (!CheckBoundaries(correctedX, correctedZ))
        {
            Debug.LogWarning("Pushed position OUT OF RANGE - original positions: " + x + "," + z);
            return null;
        }
        return grid[correctedX, correctedZ];
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
  
    private bool CheckBoundaries(int x, int y)
    {
        return (x < width && y < height
            && x >= 0 && y >= 0);
    }

    private void TintObject(HeatObject heatObject)
    {
        // ======================== Tint the tile correspondently ========================
        // Evaluate the gradient value depending on its current counts relative to the max counts on an object
        float f = Mathf.Clamp01((float)heatObject.eventCounts / individualMaxCounts);
        heatObject.tile.GetComponent<Renderer>().material.color = colorGradient.Evaluate(f) - new Color(0, 0, 0, 1 - alpha);
    }

    private void EnlargeObject(HeatObject heatObject)
    {
        // =============================== Height pijería =================================
        // We calculate the height it should be 
        float actualEnlargement = Mathf.Lerp(0, maxHeight, (float)heatObject.eventCounts / (float)individualMaxCounts);
        // Enlarge it
        heatObject.tile.transform.localScale = new Vector3(tileSize, tileSize, tileSize) + new Vector3(0f, actualEnlargement, 0f);
        // We move up the object the half of that enlargement so it stays on the ground
        heatObject.tile.transform.position = new Vector3(heatObject.tile.transform.position.x, offset.y, heatObject.tile.transform.position.z) + new Vector3(0f, actualEnlargement * 0.5f, 0f);
    }

    public void DestroyHeatMap()
    {
        RecreateHolder();
    }

    private Transform RecreateHolder()
    {
        // Reset counts
        maxCounts = 0;
        individualMaxCounts = 0;

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
