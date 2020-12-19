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
            /*
             * 
                When calling the constructor of Eventinfo, a timestamp is automatically generated which stores the seconds since startup.
                Therefore, when we want to read an EventInfo, we will:
                    1) Store the actual timestamp of that event.
                    2) Call the constructor (which will generate the timestamp, and it will be wrong, because it will not be the actual previous timestamp).
                    3) Rewrite that timestamp for the correct one, which we stored in step 1.

                // Will be improved to a better option. It only rewrites the String time, not the seconds nor time_stamp variables.
             * 
             */

            //Create (NOTE: Usage of another constructor of Eventinfo)
            String time_event = data[row][3];
            Eventinfo n_event = new Eventinfo(data[row][0], uint.Parse(data[row][1]), (CUSTOM_EVENT_TYPE)Enum.Parse(typeof(CUSTOM_EVENT_TYPE), data[row][2]),
                new Vector3(float.Parse(data[row][4]), float.Parse(data[row][5]), float.Parse(data[row][6])),
                uint.Parse(data[row][7]));
            n_event.time = time_event;

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

