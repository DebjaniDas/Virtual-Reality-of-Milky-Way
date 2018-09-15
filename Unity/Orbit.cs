using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;


    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    //public GameObject Target; //the cube game obj that we will orbit around (Plotter)
    public float speed;

    public List<GameObject> planetList = new List<GameObject>(); //== pointholder2 except instead of planet names, it's planet coordinates
   
    public List<Vector3> starList = new List<Vector3>(); //== pointholder except instead of star names, it's star coordinates
    Vector3 starCoord;

    // Full column names
    public string columnDist;
    public string columnPNum;
    public string columnStarName;
    public string columnOrbRadius;
  
    public string nombre;
    public string pnombre; 

    // Use this for initialization
    public void Start()
    {
        //get all of the stars' positions from pointholder and put them into starList
        pointList = CSVReader.Read(inputfile);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Assign column name from columnList to Name variables
        columnDist = columnList[22];
        columnPNum = columnList[3];
        columnStarName = columnList[1];
        columnOrbRadius = columnList[8];

        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            object distType = pointList[i][columnDist].GetType();

            Type a = typeof(System.String);

            //if the distance is NaN, removes info from that row
            if (distType.Equals(a))
            {
                //Debug.Log("Encountered a NaN.");
                //remove the ra and dec info in the row with the NaN as well

            }
            else
            {
                // Get value in poinList at ith "row", in "column" Name
                nombre = Convert.ToString(pointList[i][columnStarName]);
                starList.Add(GameObject.Find(nombre).transform.position); //select the star and get its position, should be Vector3

            }
        }

        //get all of the planets' positions from pointholder2 and put them into planetList 
        var cleanedStars = new List<string>();
        var uncleanedStars = new List<string>();
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

            foreach (string key in orbRadii)
            {
                if (key == "0")
                {
                    Debug.Log("the orbradius is 0");
                }
                else
                {
                    //get the coordinates of the star this planet is orbiting:
                    object distType = pointList[index][columnDist].GetType();
                    Type a = typeof(System.String);

                    //if the distance is NaN, removes info from that row
                    if (distType.Equals(a))
                    {
                        Debug.Log("Encountered a NaN dist.");
                        //remove the ra and dec info in the row with the NaN as well
                    }
                    // Assigns original values to planetName
                    pnombre = nameOfStar + "'s planet " + orbRadii.IndexOf(key);
                    planetList.Add(GameObject.Find(pnombre)); //select the planet's name

                }
            }
            orbRadii.Clear();
        }
    } //end of start()

     //Update is called once per frame
    void Update()
    {
        OrbitAround();
    }

    void OrbitAround()
    {
        foreach (GameObject key in planetList) //planetList contains the name of each plotted planet
        {
            int index = planetList.IndexOf(key);
            var targetCoord = starList[index];//starList contains the coordinates of each plotted star
            Debug.Log("PLANET:" + key);
            Debug.Log("TARGETCOORD:" + targetCoord);
            key.transform.RotateAround(targetCoord, Vector3.up, speed * Time.deltaTime);
            //key.transform.position += transform.forward * 50 * Time.deltaTime;
        }
    }


}

