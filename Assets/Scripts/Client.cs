using UnityEngine;

using Unity.Networking.Transport;
    using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool m_Done;

    public Text mytext;
    public InputField ipadd;

    public void addmsg(string txt)
    {
        mytext.text = txt+"\n";
    }

    public void Start()//string ip= "127.0.0.1", ushort port=8007)
    {

        m_Driver = NetworkDriver.Create();

    }

    public void ConnectToServer()
    {
        
        m_Connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.Parse(ipadd.text.ToString(), 8007);
        endpoint.Port = 8007;
        m_Connection = m_Driver.Connect(endpoint);

        addmsg("...connect to server on " + endpoint.Address);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            if (!m_Done)
                addmsg("Something went wrong during connect");
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;

        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                addmsg("We are now connected to the server");

                uint value = 1;
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteUInt(value);
                m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                addmsg("Got the value = " + value + " back from the server");
                m_Done = true;
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                addmsg("Client got disconnected from server");
                m_Connection = default(NetworkConnection);
            }
        }
    }
}