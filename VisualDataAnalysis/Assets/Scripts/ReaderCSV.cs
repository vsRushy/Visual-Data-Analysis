using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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
                Debug.Log(row.name + "," + row.type + "," + row.timestamp);
            }
           

        }
        else if(playerName.Equals("Sebi"))
        {
            
            string[][] data = Read(playerName);

            SerializeRead(data, EventManager.sebiEvents);
            

            //Debug info
            foreach (Eventinfo row in EventManager.sebiEvents)
            {
                Debug.Log(row.name + "," + row.type + "," + row.timestamp);
            }
        }
        else if (playerName.Equals("Marc"))
        {
            
            string[][] data = Read(playerName);

            SerializeRead(data, EventManager.marcEvents);
           

            //Debug info
            foreach (Eventinfo row in EventManager.marcEvents)
            {
                Debug.Log(row.name + "," + row.type + "," + row.timestamp);
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
            //Create 
            Eventinfo n_event = new Eventinfo(data[row][0], data[row][1], float.Parse(data[row][2]),
                new Vector3(float.Parse(data[row][3]), float.Parse(data[row][4]), float.Parse(data[row][5])),
                uint.Parse(data[row][6]));


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

