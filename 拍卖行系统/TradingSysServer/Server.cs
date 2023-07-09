/************************************************************
    文件: Server.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 游戏服务器，1.负责监听客户端
*************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TradCommon;

namespace TradingSysServer {
    public class Server {
        private Socket Serversocket;
        private IPEndPoint iPEndPoint;
        private List<Client> clientList = new List<Client>();
        private ControlManager controlManager;

        public static readonly string clientloc= "clientLSTloc";
        public Server (){}
        public Server(string ipstr,int port) {
            controlManager = new ControlManager(this);
            SetIpendPoint(ipstr, port);
        }
        public void SetIpendPoint(string ipstr,int port) {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipstr), port);
        }
        public void Start() {
            Serversocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            Serversocket.Bind(iPEndPoint);
            Serversocket.Listen(0);
            Serversocket.BeginAccept(AcceptCallBack,null);//开启线程监听
        }
        void AcceptCallBack(IAsyncResult ar) {
            Socket clientSocket = Serversocket.EndAccept(ar);
            if (clientSocket != null) {
                Console.WriteLine("监听到一个客户端");
            }
            //这里只负责存储连接，不负责操纵连接
            Client client = new Client(clientSocket,this);
            client.Start();//内部开启接受消息
            clientList.Add(client);
            Serversocket.BeginAccept(AcceptCallBack, null);//循环这个方法，继续监听
        }
        public void RemoveClient(Client client) {
            lock (clientloc) {
                clientList.Remove(client);
            }
        }
        /// <summary>
        /// 响应客户端
        /// </summary>
        /// <param name="client"></param>
        public void SentRsponse(Client client,ActionCode actionCode,string data) {
            //TODO
            // client.Send(actionCode, data);
        }
        public void SendAll(string data,Client host) {
            foreach(Client client1 in clientList) {
                if (client1!=host) {
                    client1.Send(ActionCode.Talk,data);
                }
            }
        }
        public void HandlerRequest(RequestCode request,ActionCode action,string data,Client client) {//中介
            controlManager.HandlerRequest(request,action,data,client);
        }

    }

}
