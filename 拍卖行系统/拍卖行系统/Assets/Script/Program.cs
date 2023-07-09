using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
/// <summary>
/// 客户端
/// </summary>
class Program:MonoBehaviour{
    static Socket consocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//指定协议

    static IPAddress ip =IPAddress.Parse("127.0.0.1");//本地IP
    static IPEndPoint point = new IPEndPoint(ip, 6688);//端口号
    static private byte[] buffer = new byte[1024];
    static private byte[] bufferSend = new byte[1024];
    
    private void Start() {
        consocket.Connect(point);
        StartRecive();
        //buffer = Encoding.UTF8.GetBytes("你好服务器！！！");
        //consocket.Send(buffer);
    }
    static void StartRecive() {
        consocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,ReciveCallBack,null);
    }
    static void ReciveCallBack(IAsyncResult iar) {
        int len = consocket.EndReceive(iar);
        if (len == 0) {
            return;
        }
        string str = Encoding.UTF8.GetString(buffer, 0, len);
        Debug.Log(str);
        str = "你好服务器";
        lock (bufferSend) {
            bufferSend = Encoding.UTF8.GetBytes(str);
            consocket.Send(bufferSend);
        }
        StartRecive();
    }
}
