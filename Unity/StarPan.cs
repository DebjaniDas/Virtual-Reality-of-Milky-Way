using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPan : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    public float speed;

    public List<GameObject> starObjectList = new List<GameObject>();
    public List<String> planetNamesList = new List<String>();
    public List<String> starNamesList = new List<String>();

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
    public string reading;

    private string textFromWWW;
    private string url = "http://192.168.43.142/"; // <-- enter your url here

    IEnumerator GetTextFromWWW()
    {
        WWW www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            Debug.Log("Ooops, something went wrong...");
            Debug.Log(www.error);
        }
        else
        {
            textFromWWW = www.text;
            Debug.Log(textFromWWW);

        }
    }

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
                starObjectList.Add(GameObject.Find(nombre));
                starList.Add(GameObject.Find(nombre).transform.position); //select the star and get its position, should be Vector3
                starNamesList.Add(nombre);

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
                    planetNamesList.Add(pnombre);
                }
            }
            orbRadii.Clear();
        }
    } //end of start()

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetTextFromWWW());
        string stringText = Convert.ToString(textFromWWW);
        string substring = stringText.Split('^')[0];
        int index = ((substring.Length) - 1); //index of last character, which will be the direction reading
        reading = Convert.ToString(substring[index]); //convert to string from char
        Debug.Log("READING: " + reading);

        //counter += Time.deltaTime;
        PanAround(reading);
    }

    void OrbitAround(Vector3 targetCoord, string starsName)
        {
        //for each planet(s) around the star whose position i fed into this:
            foreach (string key in planetNamesList){
              if (key.Contains(starsName)){
                int index3 = planetNamesList.IndexOf(key);
                GameObject planet = planetList[index3];
                planet.transform.RotateAround(targetCoord, Vector3.up, speed * Time.deltaTime);
                }
            }
        }
        void PanAround(string direction){
            foreach (GameObject key in starObjectList){
                int index2 = starObjectList.IndexOf(key);
                var starName = starNamesList[index2];
                Boolean up = true;
                Boolean down = true;
                Boolean left = true;
                Boolean right = true;
                Boolean center = true;
                Boolean empty = true;
                
                try{
                    while (up && direction.Equals("U")){
                        key.transform.position += transform.forward * 5 * Time.deltaTime;
                        OrbitAround(key.transform.position, starName);
                        up = false;
                    }
                    while (down && direction.Equals("D"))
                    {
                        key.transform.position += transform.forward * -5 * Time.deltaTime;
                        OrbitAround(key.transform.position, starName);
                        down = false;
                    }
                    while (left && direction.Equals("L"))
                    {
                        key.transform.position += transform.right * -1 * Time.deltaTime;
                        OrbitAround(key.transform.position, starName);
                        left = false;
                    }
                    while (right && direction.Equals("R"))
                    {
                        transform.position += transform.right * 1 * Time.deltaTime;
                        OrbitAround(key.transform.position, starName);
                        right = false;
                    }
                    while (center && direction.Equals("C"))
                    {
                        Debug.Log("while running center");
                        OrbitAround(key.transform.position, starName);
                        center = false;
                    }
                    while (empty && direction.Equals("E"))
                    {
                        Debug.Log("while running empty");
                        OrbitAround(key.transform.position, starName);
                        empty = false;
                    }     
                }
                catch(System.Exception){}

            }

            }
    }

