# TCP Driver Script for Unity Objects
This driver permits to receive data over TCP/IP Connection.
It's a TCP Server that receives data on port 64000.

[![Watch the video](https://raw.githubusercontent.com/isacco1975/Unity-TCP/master/thumbnail.jpg)](https://raw.githubusercontent.com/isacco1975/video.mp4)

# Application
It can be used to receive data from Serial Ports over TCP.
I.E. Commodore VICE can transmit using a TCP Serial Port.

I included also a Serial Driver for real Serial Connections:
this driver in VB.NET receives the data from the Serial Port, and
sends them to the TCP Server in Unity.

It's the black console that you see in the video. It simply redirect serial data to the Unity TCP Script.
In the video I am using VICE, with Standard Serial Connection on COM1 at 1200 baud. 
I am using Virtual Serial Ports to create a Bridge between COM1 and COM2.
The Serial Driver connects to COM2. So VICE is sending Serial Data from COM1 and get's received by COM2, then 
re-routed to the Unity TCP Script.

# Usage
Create a simple scene in Unity i.e. with a Cube.
Then add the C# TCP Script to it as a Compoment.
Go to PROJECT SETTINGS/PLAYER/RESOLUTION AND PRESENTATION and Enable RUN IN BACKGROUND OPTION.
Play the scene, start the Serial VB Driver, Start VICE and run the attached PRG. You are all set.
Use the virtual Joy in Port 2 in VICE to move the Unity object.

# Usage without the Serial VB Driver
Put a RS232 TCP Port on 64000, start the scene, load the PRG. You are all set.
Use the virtual Joy in Port 2 in VICE to move the Unity object.

# Connecting to REAL C64 via User Port
To connect to User Port, you need a RS232 TTL converter and a RS232 Serial to USB cable or device like DIGITUS.
Then you have to start the scene in Unity, then the Serial VB Driver pointing to the correct COM port identified by
your serial device. To change the port, modify the App.config file in the solution or in the /bin/debug folder the file TcpSerialDriver.exe.config then run the driver.
Start the program on your C64 and use Joystick in Port 2

[![Watch the video](https://raw.githubusercontent.com/isacco1975/Unity-TCP/master/thumbnail2.jpg)](https://raw.githubusercontent.com/isacco1975/video2.mp4)


