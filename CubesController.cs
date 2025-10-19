using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class CubesController : MonoBehaviour
{

    public TCP_Driver o;
    public bool isStringSent = false;
    public bool isStart = false;
    char[] receivedChars;

    public GameObject RED;
    public GameObject GREEN;
    public GameObject BLUE;
    public GameObject YELLOW;
    public GameObject EMPTY;

    public string row1 = "";
    public string row2 = "";
    public string row3 = "";
    public string row4 = "";
    public string row5 = "";

    public char[] xrow1, xrow2, xrow3, xrow4, xrow5;

    float redActualXPos;
    float greenActualXPos;
    float yellowActualXPos;
    float blueActualXPos;

    float redActualYPos;
    float greenActualYPos;
    float yellowActualYPos;
    float blueActualYPos;

    float actualXPos = 0f, actualYPos = 0f;
    public int col = 0;

    List<string> rows; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RED = GameObject.Find("RED");
        GREEN = GameObject.Find("GREEN");
        BLUE = GameObject.Find("BLUE");
        YELLOW = GameObject.Find("YELLOW");
    }

    // Update is called once per frame
    void Update()
    {
        o = GameObject.Find("TCP_Component").GetComponent<TCP_Driver>();
        //Debug.Log("from Server: " + o.xBuffer);
        char[] charArray = new char[o.xBuffer.Length];
        Debug.Log("Incoming TCP message: " + o.xBuffer);

        if (o.xBuffer.EndsWith("\r"))
        {
            isStringSent = true;            
            
            if (o.xBuffer.Contains("START"))
            {
                isStart = true;
                string s = o.xBuffer.Split('.')[0];
                receivedChars = s.Substring(24).ToCharArray();
            } else
            {
                receivedChars = o.xBuffer.ToCharArray();

                //redActualXPos = RED.GetComponent<Transform>().localPosition.x;
                //greenActualXPos = GREEN.GetComponent<Transform>().localPosition.x;
                //yellowActualXPos = YELLOW.GetComponent<Transform>().localPosition.x;
                //blueActualXPos = BLUE.GetComponent<Transform>().localPosition.x;
                //
                //redActualYPos = RED.GetComponent<Transform>().localPosition.y;
                //greenActualYPos = GREEN.GetComponent<Transform>().localPosition.y;
                //yellowActualYPos = YELLOW.GetComponent<Transform>().localPosition.y;
                //blueActualYPos = BLUE.GetComponent<Transform>().localPosition.y;

                row5 = new string(receivedChars, 0, 6);
                row4 = new string(receivedChars, 6, 6);
                row3 = new string(receivedChars, 12, 6);
                row2 = new string(receivedChars, 18, 6);
                row1 = new string(receivedChars, 24, 6);

                rows = new List<string>();

                rows.Add(row1);
                rows.Add(row2);
                rows.Add(row3);
                rows.Add(row4);
                rows.Add(row5);

                MoveCubesToNewPositions();
            }

            switch (o.xBuffer)
            {
                case "XXX":
                     break;
                default:
                    Debug.Log("Received unknown command");
                    break;
            }

            if (isStart)
            {
                Debug.Log("Received START command");
                DrawInitialSituation();
                isStart = false;
            }

            if (isStringSent)
            {
                isStringSent = false;
                o.xBuffer = "";
                
                for (int idx = 0; idx < receivedChars.Length; idx++)
                {
                    receivedChars[idx] = '\0';
                }
            }
        }
    }

    void DrawInitialSituation()
    {
        //pos1 x=24
        //pos2 x=22.5
        //pow3 x=21
        //pos4 x=19.5
        //pos5 x=18
        //pos6 x=16.5
        float xPos = 0;
        string cubeToMove = "";

        for (int idx = 0; idx < receivedChars.Length; idx++)
        {
            switch (idx)
            {
                case 0: xPos = 24.0f; break;
                case 1: xPos = 22.3f; break;
                case 2: xPos = 21.0f; break;
                case 3: xPos = 19.5f; break;
                case 4: xPos = 18.0f; break;
                case 5: xPos = 16.5f; break;
            }
               
            MoveCube(receivedChars[idx], xPos, 1f, 0.02453f);
        }
    }

    void MoveCube(char cubeToMove, float xPosition, float yPosition, float zPosition)
    {
        switch (cubeToMove)
        {
            case 'R': RED.transform.localPosition = new Vector3(xPosition, yPosition, zPosition); break;
            case 'G': GREEN.transform.localPosition = new Vector3(xPosition, yPosition, zPosition); break;
            case 'B': BLUE.transform.localPosition = new Vector3(xPosition, yPosition, zPosition); break;
            case 'Y': YELLOW.transform.localPosition = new Vector3(xPosition, yPosition, zPosition); break;
        }
    }

    void MoveCubesToNewPositions()
    {
        float actualXPos = 0f, actualYPos = 0f, newXPos = 0f;
        bool firstRow = true;

        foreach (string row in rows)
        {
             // Calculating actual positions of cubes in the current column
             // for row 1 there is no need to move anything, just get actual positions
             if (row != "EEEEE")
             {
                GetActualPositions(row);

                switch (col)
                {
                    case 0: newXPos = 24.0f; break;
                    case 1: newXPos = 22.3f; break;
                    case 2: newXPos = 21.0f; break;
                    case 4: newXPos = 18.0f; break;
                    case 5: newXPos = 16.5f; break;
                }

                if (firstRow)
                {
                    firstRow = false;
                } else
                    MoveCube(row[col], newXPos, actualYPos + 2.5f, 0.02453f);
             }
        }
    }

    void GetActualPositions(string a)
    {
        if (a.Contains("R"))
        { 
            col = a.IndexOf("R");
            actualXPos = RED.GetComponent<Transform>().localPosition.x;
            actualYPos = RED.GetComponent<Transform>().localPosition.y;
        }

        if (a.Contains("Y"))
        {
            col = a.IndexOf("Y");
            actualXPos = YELLOW.GetComponent<Transform>().localPosition.x; 
            actualYPos = YELLOW.GetComponent<Transform>().localPosition.y;
        }

        if (a.Contains("G"))
        {
            col = a.IndexOf("G");
            actualXPos = GREEN.GetComponent<Transform>().localPosition.x;
            actualYPos = GREEN.GetComponent<Transform>().localPosition.y;
        }

        if (a.Contains("B"))
        {
            col = a.IndexOf("B");
            actualXPos = BLUE.GetComponent<Transform>().localPosition.x;
            actualYPos = BLUE.GetComponent<Transform>().localPosition.y;
        }
    }


    // END CLASS
}
