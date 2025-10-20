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
        char[] charArray = new char[o.xBuffer.Length];

        if (o.xBuffer.EndsWith("\r"))
        {
            Debug.Log("Incoming TCP message: " + o.xBuffer);
            isStringSent = true;            
            receivedChars = o.xBuffer.ToCharArray();

            row5 = new string(receivedChars, 0, 6);
            row4 = new string(receivedChars, 6, 6);
            row3 = new string(receivedChars, 12, 6);
            row2 = new string(receivedChars, 18, 6);
            row1 = new string(receivedChars, 24, 6);

            rows = new List<string>();
            rows.Clear();
            rows.Add(row1);
            rows.Add(row2);
            rows.Add(row3);
            rows.Add(row4);
            rows.Add(row5);

            MoveCubesToNewPositions();
            
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
        int rowIndex = 0;

        foreach (string row in rows)
        {
             rowIndex++;

            // Calculating actual positions of cubes in the current column
            // for row 1 there is no need to move anything, just get actual positions
            if (row != "EEEEEE")
             {
                GetActualPositions(row);

                for (int col = 0; col < 6; col++)
                {
                    switch (col)
                    {
                        case 0: newXPos = 24.0f; break;
                        case 1: newXPos = 22.5f; break;
                        case 2: newXPos = 21.0f; break;
                        case 3: newXPos = 19.5f; break;
                        case 4: newXPos = 18.0f; break;
                        case 5: newXPos = 16.5f; break;
                    }

                    switch (rowIndex)
                    {
                        case 1: MoveCube(row[col], newXPos, actualYPos + 1f, 0.02453f); break;
                        case 2: MoveCube(row[col], newXPos, actualYPos + 2.5f, 0.02453f); break;
                        case 3: MoveCube(row[col], newXPos, actualYPos + 4f, 0.02453f); break;
                        case 4: MoveCube(row[col], newXPos, actualYPos + 5.5f, 0.02453f); break;
                        case 5: MoveCube(row[col], newXPos, actualYPos + 7f, 0.02453f); break;
                    }
                }
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
