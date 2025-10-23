using System;
using System.Collections.Generic;
using UnityEngine;

//****************************************************
//* Cubes Controller Script                         **
//****************************************************
//*         Manage the movements of the cubes in the *
//*         scene from the data coming.              *
//****************************************************

public class CubesController : MonoBehaviour
{
    #region WORKING-STORAGE
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

    float actualXPos = 0f, actualYPos = 0f;
    public int col = 0;

    List<string> rows;
    #endregion

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        RED = GameObject.Find("RED");
        GREEN = GameObject.Find("GREEN");
        BLUE = GameObject.Find("BLUE");
        YELLOW = GameObject.Find("YELLOW");
    }

    /// <summary>
    /// Update position of each cube according to the received string
    /// </summary>
    void DrawInitialSituation()
    {
        float actualXPos = 0f, actualYPos = 0f, newXPos = 0f;
        int rowIndex = 0;

        Debug.Log("Incoming TCP message: " + o.xBuffer);
        isStringSent = true;
        receivedChars = o.xBuffer.ToCharArray();
        row1 = new string(receivedChars, 24, 6);

        if (row1 != "EEEEEE")
        {
            GetActualPositions(row1);

            for (int col = 0; col < 6; col++)
            {
                switch (col)
                {
                    case 0: newXPos = 24.0f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break; break;
                    case 1: newXPos = 22.5f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break;
                    case 2: newXPos = 21.0f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break;
                    case 3: newXPos = 19.5f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break;
                    case 4: newXPos = 18.0f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break;
                    case 5: newXPos = 16.5f; MoveCube(row1[col], newXPos, actualYPos + 1f, 0.02453f); break;
                }
            }
        }
    }

    /// <summary>
    /// This is called before updating each cube position
    /// </summary>
    /// <param name="a"></param>
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

    /// <summary>
    /// The routine that updated the position of a specific cube
    /// </summary>
    /// <param name="cubeToMove"></param>
    /// <param name="xPosition"></param>
    /// <param name="yPosition"></param>
    /// <param name="zPosition"></param>
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

    /// <summary>
    /// Animating the single cube received from the C64
    /// </summary>
    void AnimateCube()
    {
        char cubeToMove = Convert.ToChar(o.xBuffer.Split(";")[0]);
        Single xPosition = Single.Parse(o.xBuffer.Split(";")[1]);
        Single yPosition = Single.Parse(o.xBuffer.Split(";")[2]);

        MoveCube(cubeToMove, xPosition, yPosition, 0.02453f);
    }

    /// <summary>
    /// Update the scene every 1 frame
    /// </summary>
    void Update()
    {
        o = GameObject.Find("TCP_Component").GetComponent<TCP_Driver>();
        char[] charArray = new char[o.xBuffer.Length];

        if (o.xBuffer.EndsWith("\r"))
        {
            isStringSent = true;
            Debug.Log("Incoming TCP message: " + o.xBuffer);
            if (o.xBuffer.Contains("GENERA"))
                DrawInitialSituation();
            else
                AnimateCube();

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

    // END CLASS
}
