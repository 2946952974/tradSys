/****************************************************
    文件：Test.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/10 19:20:59
	功能：出售提示框
*****************************************************/

using UnityEngine;
using TradCommon;
using System.Net.Sockets;
using System;

public class Test:MonoBehaviour{
    public const string IP = "127.0.0.1";
    private const int PORT = 6688;
    private Socket clientSocket;
    private Message msg = new Message();

    public void Start() {
        OnInit();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)){
            //Debug.Log("发送消息");
            RequestCode requestCode = RequestCode.LoginSys;
            ActionCode actionCode = ActionCode.Login;
            SenRequest(requestCode, actionCode, "123|123|bieli|男|");
        }
    }
    public void OnInit() {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try {
            clientSocket.Connect(IP, PORT);
            ClientStart();
        }
        catch(Exception e) {
            Debug.LogWarning("无法连接服务器" + e);
        }
    }
    private void ClientStart() {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None,RececiveCallBack,null);
    }
    private void RececiveCallBack(IAsyncResult ar) {
        try {
            if (clientSocket.Connected == false || clientSocket == null) return;
            int count = clientSocket.EndReceive(ar);
            if (count == 0) {
                clientSocket = null;
                Debug.LogError("网络掉线");
                OnInit();
            }
            msg.ReadMessage(count,OnProcessDataCallback);
            Start();//循环调用不然只能接受一次消息
        }
        catch {

        }
    }
    private void OnProcessDataCallback(ActionCode actionCode, string data)//服务器发送过来的requestcode和data
    {
        Debug.Log(actionCode + "方法" + data);
    }
    public void SenRequest(RequestCode requestCode,ActionCode actionCode,string data) {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
}