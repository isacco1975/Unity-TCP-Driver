using UnityEngine;

//**************************************************
//* ISAAC GARCIA PEVERI (IGP TECH BLOG)            *
//**************************************************
//*    IN THIS EXAMPLE, WE ARE CONTROLLING THE     *
//*    CUBE POSITION BY THE DATA RECEIVED BY THE   *
//*    TCP_DRIVER.CS SCRIPT                        *
//**************************************************


public class CubeController : MonoBehaviour
{

    public TCP_Driver o;                //Referencing the TCP_Driver.cs Script
    public string receivedString;       //The data received will be here
    public bool isStringSent = false;   //Flag

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		//DO NOTHING HERE. EVERYTHING IS MANAGED BY UPDATE() ROUTINE
    }

    // Update is called once per frame
    void Update()
    {
		//Get the data from the TCP_Driver.cs script (xBuffer)
		//(data is passed from TCP_Driver.cs)
		//So this example also shows how to pass data between different scripts.
		
        o = GameObject.Find("TCP_Component").GetComponent<TCP_Driver>();
        receivedString = o.xBuffer;
        Debug.Log("from Server: " + receivedString);

        if (receivedString.EndsWith("\r")) //got the data
        {
            isStringSent = true;

            //Manipulating position of the cube
			
            Debug.Log("Incoming TCP message: " + receivedString);
            string[] values = receivedString.Split(';');
            
            string x = values[0];
            string y = values[1];
            string z = values[2];

            transform.localPosition = new Vector3(float.Parse(x), float.Parse(z), float.Parse(y));

            //Resetting the data received after using them
			
            if (isStringSent) 
            {
                isStringSent = false;
                receivedString = "";
            }
        }
    }
}
