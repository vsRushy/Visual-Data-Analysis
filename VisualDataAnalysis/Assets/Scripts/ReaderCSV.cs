using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ReaderCSV : MonoBehaviour
{
    public string playerName;
   

    // Start is called before the first frame update
    void Start()
    {
        if (playerName.Equals("Carlos"))
        {
           
            string[][] data = Read(playerName);
            SerializeRead(data, EventManager.carlosEvents);

            //Debug info
            foreach(Eventinfo row in EventManager.carlosEvents)
            {
                Debug.Log(row.player_name + "," + row.type + "," + row.time);
            }
           

        }
        else if(playerName.Equals("Sebi"))
        {
            
            string[][] data = Read(playerName);

            SerializeRead(data, EventManager.sebiEvents);
            

            //Debug info
            foreach (Eventinfo row in EventManager.sebiEvents)
            {
                Debug.Log(row.player_name + "," + row.type.ToString() + "," + row.time);
            }
        }
        else if (playerName.Equals("Marc"))
        {
            
            string[][] data = Read(playerName);

            SerializeRead(data, EventManager.marcEvents);
           

            //Debug info
            foreach (Eventinfo row in EventManager.marcEvents)
            {
                Debug.Log(row.player_name + "," + row.type.ToString() + "," + row.time);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SerializeRead(string[][] data, List<Eventinfo> selectedList)
    {
        if (data == null)
            return;

        uint nrows = (uint)data.GetLength(0);

        for (int row = 0; row < nrows; ++row)
        {
            //Create (NOTE: Usage of another constructor of Eventinfo)
            Eventinfo n_event = new Eventinfo(data[row][0], uint.Parse(data[row][1]), (CUSTOM_EVENT_TYPE)Enum.Parse(typeof(CUSTOM_EVENT_TYPE), data[row][2]),
                new Vector3(float.Parse(data[row][4]), float.Parse(data[row][5]), float.Parse(data[row][6])),
                uint.Parse(data[row][7]));

            //Add row info
            selectedList.Add(n_event);
        }

    }

    public string[][] Read(string playerName)
    {
        string filepath = Application.dataPath + "/CSV/" + "SpatialEvents.csv";

        if (File.Exists(filepath))
        {

            string data;
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            data = read.ReadToEnd();

            if (data == "")
            {
                Debug.LogError("File at " + filepath + " is empty");
                return null;
            }

            string[] rows = data.Split('\n');

            List<string[]> ret = new List<string[]>();
            foreach (string row in rows)
            {
                if(row.Contains(playerName))
                {
                    string[] rowData = row.Split(',');
                    ret.Add(rowData);
                }

            }

            return ret.ToArray();
            
        }
        else
        {
            Debug.LogError("File at " + filepath + " does not exist");
            return null;
        }
    }

}

