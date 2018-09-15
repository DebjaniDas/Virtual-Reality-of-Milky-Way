using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetPlotter : MonoBehaviour
{


    // Name of the input file, no extension
    public string inputfile;
 
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;
    private List<Dictionary<string, object>> repeatedStars;

    // Full column names
    public string columnRa;
    public string columnDec;
    public string columnDist;
    public string columnTeff;
    public string columnPNum;
    public string columnStarName;
    public string columnOrbRadius;
    public float distance;
    public float ra;
    public float dec;
    public float teff;
    public float x1;
    public float y1;
    public float z1;
    public float z;


    // The prefab for the data points to be instantiated
    public GameObject PointPrefab; //this is Planet Point, the planet prefab

    public GameObject PointHolder2; //we'll use this as prefab's parent to store the stars later on


 
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

        columnOrbRadius = columnList[8];

        //make an empty list for cleaned stars so there is no repetition of star names 
        var cleanedStars = new List<string>();
        var uncleanedStars = new List<string>();
        //uncleanedStars.Add("rowid");
        var orbRadii = new List<string>();



        //loop through the values in pl_hostname (star names) and add them to the cleanedStars list, avoiding repetitions
        for (var i = 0; i < pointList.Count; i++)
        {
            string star = Convert.ToString(pointList[i][columnStarName]);
            if (!cleanedStars.Contains(star))
            {
                cleanedStars.Add(star);
            }
        }


        for (var i = 0; i < pointList.Count; i++)
        {
            string star = Convert.ToString(pointList[i][columnStarName]);

            uncleanedStars.Add(star);

        }

       

        //For each star in the cleanedStars list:
        for (var i = 0; i < cleanedStars.Count; i++)
        {
            //get the name of the star in cleanedStars at "i"
            var nameOfStar = cleanedStars[i];//this is a string

            //get the last index (bc repetition) of that star in uncleanedStars -> essentially PointList
            int index = (uncleanedStars.FindLastIndex(x => x.Contains(nameOfStar)));


            float pNum = Convert.ToSingle(pointList[index][columnPNum]);

            //since for some stars there are multiple planets, this gets the orbradius of each planet (each row = different indices in pointList)
            for (var a = 0; a < pNum; a++)
            {
                Type b = typeof(System.String);
                object orbRadiusType = (pointList[(index) - a][columnOrbRadius]).GetType();

                if (orbRadiusType.Equals(b))
                {
                    Debug.Log("Encountered a NaN in orbradius.");
                }
                else
                {
                    string orbRadius = Convert.ToString(pointList[(index) - a][columnOrbRadius]);
                    orbRadii.Add(orbRadius); //so now we should have a list of all the non-NaN orbital radii for all 3 planets of star A (eg)

                }

            }

            foreach (string key in orbRadii) // loops through list of orbital radii
            {
                if (key == "0") //if the radius = 0
                {
                    Debug.Log("the orbradius is 0");
                }
                else
                {
                    object distType = pointList[index][columnDist].GetType();

                    Type a = typeof(System.String);

                    //if the distance is NaN, removes info from that row so the planet with the NaN doesn't plot
                    if (distType.Equals(a))
                    {
                        Debug.Log("Encountered a NaN dist.");
                    }
                    else
                    {
                        // Get value in poinList for the selected star at ith "row", in "column" Name
                        distance = Convert.ToSingle(pointList[index][columnDist]);
                        ra = Convert.ToSingle(pointList[index][columnRa]);
                        dec = Convert.ToSingle(pointList[index][columnDec]);
                    }

                    //Convert above values from cartesian to spherical coordinates--> THESE ARE THE STAR COORDINATES
                    x1 = Convert.ToSingle(distance * Math.Cos(dec) * Math.Cos(ra));
                    y1 = Convert.ToSingle(distance * Math.Cos(dec) * Math.Sin(ra));
                    z1 = Convert.ToSingle(distance * Math.Sin(dec));

                    //position the planet "x" away (according to radius) from the star along z axis
                    float radius = Convert.ToSingle(key);
                    z = z1 - radius;

                    //planet has same coordinates as star for x and y axis
                    // Instantiate as gameobject variable so that it can be manipulated within loop
                    GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x1, y1, z),
                    Quaternion.identity);

                    // Make dataPoint child of PointHolder2 object 
                    dataPoint.transform.parent = PointHolder2.transform;

                    // Assigns original values to dataPointName
                    string dataPointName = nameOfStar + "'s planet " + orbRadii.IndexOf(key);

                    // Assigns name to the prefab
                    dataPoint.transform.name = dataPointName;
                }
            }

            orbRadii.Clear();

        }
  

    }


}
