using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MyTcpTalker
{
    private static Int32 port = 13000;
    private static TcpClient client;
    private static Stream stream;

    public static void Main()
    {
        bool stopLooping = false;
        string str = "";

        Connect("127.0.0.1");
        Console.WriteLine("'Kill' to exit");
        while (stopLooping == false)
        {
            str = Console.ReadLine();
            stopLooping = str.ToUpper() == "KILL";
            if (stopLooping == false)
            {
                SendAndReceive(str);
            }
        }
        Disconnect();
    }

    public static bool Assigned(object obj)
    {
        return (obj != null);
    }

//    public static bool Not(bool b)
//    {
//        return !b;
//    }

    static void Connect(String server)
    {
        try
        {
            client = new TcpClient(server, port);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    static void Disconnect()
    {
        try 
        {
            if (Assigned(stream)) 
            { 
                stream.Close();
            }
            if (Assigned(client))
            {
                client.Close();
            }
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    static void SendAndReceive(string message)
    {
        try
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing. 
            stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Receive the TcpServer.response. 

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine(responseData);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }
}