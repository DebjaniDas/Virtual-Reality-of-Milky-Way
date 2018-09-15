using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataPlotter : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public int columnRaInd = 20;
    public int columnDecInd = 21;
    public int columnTeffInd = 34;
    public int columnDistInd = 22;
    public int columnPNumInd = 3;

    // Full column names
    public string columnRa;
    public string columnDec;
    public string columnDist;
    public string columnTeff;
    public string columnPNum;
    public string columnStarName;
    public float distance;
    public float ra;
    public float dec;
    public float teff;
    public float mass;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab; //this is star point, the prefab star

    // The prefab for the data points that will be instantiated
    public GameObject PointHolder; //this is pointholder

    // Use this for initialization
    void Start()
    {
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Assign column name from columnList to Name variables
        columnRa = columnList[20];
     
        columnDec = columnList[21];
   
        columnTeff = columnList[34];
     
        columnDist = columnList[22];

        columnPNum = columnList[3];

        columnStarName = columnList[1];

        //Loop through Pointlist, a list holding all the values from CSV file
        for (var i = 0; i < pointList.Count; i++)
        {
            object distType = pointList[i][columnDist].GetType();

            object teffType = pointList[i][columnTeff].GetType();

            Type a = typeof(System.String);

            //if the distance is NaN, removes info from that row
            if (distType.Equals(a)) {

            }
            else {
                // Get value in poinList at ith "row", in "column" Name
                distance = Convert.ToSingle(pointList[i][columnDist]);
                //ra (right ascension) and dec (declination) are star coordinates
                ra = Convert.ToSingle(pointList[i][columnRa]); 
                dec = Convert.ToSingle(pointList[i][columnDec]); 
            }

            //Convert above values from cartesian to spherical coordinates
            float x = Convert.ToSingle(distance * Math.Cos(dec) * Math.Cos(ra));
            float y = Convert.ToSingle(distance * Math.Cos(dec) * Math.Sin(ra));
            float z = Convert.ToSingle(distance * Math.Sin(dec));

 
            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x, y, z),
                    Quaternion.identity);
            
            // Make child of PointHolder object, to keep points within container in hierarchy
            dataPoint.transform.parent = PointHolder.transform;

            string starName = Convert.ToString(pointList[i][columnStarName]);
            // Assigns original values to dataPointName
            string dataPointName = starName;
 
            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            //index into the teff (temperature) column of ith row and make an if condition with ranges for diff colors
            if (teffType.Equals(a))
            {
                Debug.Log("Encountered a NaN (teff).");
                dataPoint.GetComponent<Renderer>().material.color = Color.green;
            }
            else //teff exists
            {
                teff = Convert.ToSingle(pointList[i][columnTeff]);
            }

            if (teff <= 3000)
            {
                // Gets material color and sets it to a new RGBA color we define
                dataPoint.GetComponent<Renderer>().material.color = Color.red;

            }
            else if (teff > 3000 & teff <= 4000){
                //dataPoint.GetComponent<Renderer>().material.color =
                //new Color(x, y, z, 1.0f);
                dataPoint.GetComponent<Renderer>().material.color = Color.magenta; //i want this to be orange tho..

            }
            else if (teff > 4000 & teff <= 5000){
                dataPoint.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (teff > 5000 & teff <= 6000){
                dataPoint.GetComponent<Renderer>().material.color = Color.white;
            }
            else if (teff >6000 & teff <= 7000){
                dataPoint.GetComponent<Renderer>().material.color = Color.cyan;
            }
            else {
                Debug.Log("the temp is greater than 7000");
            }

        }
      

    }
}
