using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine.UI;
using UnityEngine.Networking.Types;
using UnityEngine.Networking;
using System.Net;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Linq;

public class Server : MonoBehaviour
{
    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    public Text mytext;

    public void addmsg(string txt)
    {
        mytext.text += txt + "\n";
    }

    [Obsolete]
    void Start()
    {
        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4; // The local address to which the client will connect to is 127.0.0.1
        endpoint.Port = 8007;
        if (m_Driver.Bind(endpoint) != 0)
            addmsg("Failed to bind to port 8007");
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        GetIP();
    }

    public void GetIP()
    {
        /*
        string address;
        int port;
        NetworkID netID;
        NodeID nodeID;
        byte error;

        NetworkTransport.GetConnectionInfo(0, 0, out address, out port, out netID, out nodeID, out error);
        
        addmsg(address + " & " + port); */
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                addmsg(ip.ToString());
                
            }
        }
 //       throw new System.Exception("No network adapters with an IPv4 address in the system!");

    }

    public void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // CleanUpConnections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }
        
        // AcceptNewConnections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            addmsg("Accepted a connection");
        }

        DataStreamReader stream;
        
        for (int i = 0; i < m_Connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadUInt();

                    addmsg("Got " + number + "from" + "\n");
                    number += 2;
                    
                    m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);
                    writer.WriteUInt(number);
                    m_Driver.EndSend(writer);
                }

                /* Not disconnecting
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    addmsg("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }*/
            }
        }

        //quiz options
    }
}