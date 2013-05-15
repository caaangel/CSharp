using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MyTcpListener
{
    private static Int32 port = 13000;
    private static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
    private static TcpListener server = null;
    private static Byte[] bytes = new Byte[256];
    private static String data = null;
    private static int I = 0;

    static void Setup()
    {
        try
        {
            server = new TcpListener(localAddr, port);
            server.Start();
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    static void Terminate()
    {
        server.Stop();
    }

    static void EnterListeningLoop()
        {
            try
            {
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    while ( ((I = stream.Read(bytes, 0, bytes.Length)) != 0) )
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, I);
                        Console.WriteLine("Incoming: {0}", data);

                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Replying: {0}", data);
                    }

                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

    public static void Main()
    {
        Setup();
        EnterListeningLoop();
        Terminate();
        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }
}