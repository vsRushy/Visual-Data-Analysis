using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WriterCSV : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();
    // Start is called before the first frame update
    void Start()
    {
        
        //EventManager.eventManager.events.Enqueue(new Eventinfo("Sebi", 0, CUSTOM_EVENT_TYPE.POSITION, new Vector3(2, 1, 2), 1));
        //EventManager.eventManager.events.Add(new Eventinfo("Carlos", 1, "Death", new Vector3(2, 1, 2), 2));
        //EventManager.eventManager.events.Add(new Eventinfo("Doctor", 2, "Damage", new Vector3(2, 1, 2), 3));

        //WriterData(EventManager.eventManager.events);
    }

    // For testing purposes
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            WriterData(EventManager.events);
        }
    }
    

    void WriterData(Queue<Eventinfo> data)
    {
        
        //Creating Path
        string filepath = Application.dataPath + "/CSV/" + "SpatialEvents.csv";
        
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[8];
        rowDataTemp[0] = "PlayerName";
        rowDataTemp[1] = "PlayerId";
        rowDataTemp[2] = "Event";
        rowDataTemp[3] = "Timestamp";
        rowDataTemp[4] = "Position_X";
        rowDataTemp[5] = "Position_Y";
        rowDataTemp[6] = "Position_Z";
        rowDataTemp[7] = "Stage";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        foreach(Eventinfo row in data)
        {
            rowDataTemp = new string[8];
            rowDataTemp[0] = row.player_name;
            rowDataTemp[1] = row.player_id.ToString();
            rowDataTemp[2] = row.type.ToString();
            rowDataTemp[3] = row.time;
            rowDataTemp[4] = row.position.x.ToString();
            rowDataTemp[5] = row.position.y.ToString();
            rowDataTemp[6] = row.position.z.ToString();
            rowDataTemp[7] = row.stage.ToString();
            rowData.Add(rowDataTemp);
        }
        for (int i = 0; i < 10; i++)
        {
            rowDataTemp = new string[8];
            rowDataTemp[0] = "Carlos";
            rowDataTemp[1] = 1.ToString();
            rowDataTemp[2] = CUSTOM_EVENT_TYPE.POSITION.ToString();
            rowDataTemp[3] = System.DateTime.Now.ToString();
            rowDataTemp[4] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[5] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[6] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[7] = "2";
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        //string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filepath);
        outStream.WriteLine(sb);
        outStream.Close();

        Debug.Log("heys");
    }
}
