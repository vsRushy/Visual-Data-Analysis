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
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    string[][] data;
        //    switch (EventManager.userName)
        //    {
        //        case VisualizationManager.User.Carlos:
        //            WriterData(EventManager.carlosEvents);
        //            break;
        //        case VisualizationManager.User.Sebi:
        //            WriterData(EventManager.sebiEvents);
        //            break;
        //        case VisualizationManager.User.Marc:
        //            WriterData(EventManager.marcEvents);
        //            break;
        //        case VisualizationManager.User.Peter:
        //            WriterData(EventManager.joseEvents);
        //            break;
        //        case VisualizationManager.User.Gerard:
        //            WriterData(EventManager.gerardEvents);
        //            break;
        //    }
        //}
    }

    


    public static void WriterData(List<Eventinfo> data)
    {
        if (data.Count == 0)
            return;

        List<string[]> rowData = new List<string[]>();
        //Creating Path
        string filepath = Application.dataPath + "/CSV/" + "SpatialEvents.csv";

        string[] rowDataTemp = new string[6];

        // Creating First row of titles manually..
        if (!File.Exists(filepath))
        {
            rowDataTemp[0] = "PlayerName";
            rowDataTemp[1] = "Event";
            rowDataTemp[2] = "Timestamp";
            rowDataTemp[3] = "Position_X";
            rowDataTemp[4] = "Position_Y";
            rowDataTemp[5] = "Position_Z";
            rowData.Add(rowDataTemp);
            Debug.Log("Not Exist");
        }
        // You can add up the values in as many cells as you want.
        foreach(Eventinfo row in data)
        {
            rowDataTemp = new string[6];
            rowDataTemp[0] = row.player_name;
            rowDataTemp[1] = row.type.ToString();
            rowDataTemp[2] = row.time;
            rowDataTemp[3] = row.position.x.ToString(EventManager.culture);
            //float pos_x = row.position.x;
            //float.TryParse(rowDataTemp[3], System.Globalization.NumberStyles.Float, culture, out pos_x);
            rowDataTemp[4] = row.position.y.ToString(EventManager.culture);
            //float pos_y = row.position.y;
            //float.TryParse(rowDataTemp[4], System.Globalization.NumberStyles.Float, culture, out pos_y);
            rowDataTemp[5] = row.position.z.ToString(EventManager.culture);
            //float pos_z = row.position.z;
            //float.TryParse(rowDataTemp[5], System.Globalization.NumberStyles.Float, culture, out pos_z);

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

        StreamWriter outStream = System.IO.File.AppendText(filepath);
        outStream.WriteLine(sb);
        outStream.Close();

        Debug.Log("heys");
    }
}
