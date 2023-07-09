/****************************************************
    文件：NetSvc.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/11 22:28:47
	功能：网络服务
*****************************************************/
using UnityEngine;
using TradCommon;
using System.Net.Sockets;
using System;

public class NetSvc:MonoBehaviour{
    //public const string IP = "150.158.152.165";//150.158.152.165
    public const string IP = "127.0.0.1";//150.158.152.165
    private const int PORT = 6688;
    private Socket clientSocket;
    private Message msg = new Message();

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.S)) {
        //    //Debug.Log("发送消息");
        //    RequestCode requestCode = RequestCode.LoginSys;
        //    ActionCode actionCode = ActionCode.Login;
        //    SenRequest(requestCode, actionCode, "123|123|bieli|男|");
        //}
    }
    public void OnInit() {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try {
            clientSocket.Connect(IP, PORT);
            ClientStart();
        }
        catch (Exception e) {
            Debug.LogWarning("无法连接服务器" + e);
        }
    }
    private void ClientStart() {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, RececiveCallBack, null);
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
            msg.ReadMessage(count, OnProcessDataCallback);
            OnInit();//循环调用不然只能接受一次消息
        }
        catch(Exception e) {
            Debug.LogWarning(e);
        }
    }
    private void OnProcessDataCallback(ActionCode actionCode, string data){//服务器发送过来的requestcode和data
        Debug.Log(actionCode + "方法" + data);
            switch (actionCode) {
                case ActionCode.None:
                    break;
                case ActionCode.Login:
                GameRoot.Instacne.LoginSync(data);
                    break;
                case ActionCode.Register:
                GameRoot.Instacne.Register(data);
                    break;
                case ActionCode.Trad:
                GameRoot.Instacne.TradSync(data);
                    break;
                case ActionCode.Buy:
                GameRoot.Instacne.BuySync(data);
                    break;
                case ActionCode.Sell:
                GameRoot.Instacne.SellSync(data);
                    break;
                case ActionCode.Frensh:
                GameRoot.Instacne.FreshSync(data);
                    break;
                case ActionCode.Talk:
                GameRoot.Instacne.TalkSync(data);
                break;
        }
    }
    public void SenRequest(RequestCode requestCode, ActionCode actionCode, string data) {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
    public void Sell(String data) {
        Debug.Log(data);
    }
}