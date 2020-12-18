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
        WriterData();
    }

    

    void WriterData()
    {

        //Creating Path
        string filepath = Application.dataPath + "/CSV/" + "SpatialEvents.csv";
        
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[7];
        rowDataTemp[0] = "PlayerName";
        rowDataTemp[1] = "Event";
        rowDataTemp[2] = "Timestamp";
        rowDataTemp[3] = "Position_X";
        rowDataTemp[4] = "Position_Y";
        rowDataTemp[5] = "Position_Z";
        rowDataTemp[6] = "Stage";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < 10; i++)
        {
            rowDataTemp = new string[7];
            rowDataTemp[0] = "Carlos"; 
            rowDataTemp[1] = "Position";
            rowDataTemp[2] = System.DateTime.Now.ToString();
            rowDataTemp[3] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[4] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[5] = UnityEngine.Random.Range(-10, 10).ToString();
            rowDataTemp[6] = "2";
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
