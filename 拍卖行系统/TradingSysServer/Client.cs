/************************************************************
    文件: Client.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 存储客户端的连接
*************************************************************/

using System;
using System.Net.Sockets;
using TradCommon;

namespace TradingSysServer {
    public class Client {
        private Socket clientSocket;//客户端连接
        private Server server;//服务器连接
        //TODO 消息类，数据库连接，用户等等信息
        private Message msg = new Message();
        public Client() { }
        public Client(Socket socket, Server server) {
            this.clientSocket = socket;
            this.server = server;
        }
        public void Start() {//这里这个连接开启线程监听消息
            if (clientSocket == null||clientSocket.Connected==false)
                return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None,ReciveCallback,null);
        }
        private void ReciveCallback(IAsyncResult ar) {
            try {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (count == 0) {//0字节表示断开了连接,服务器可能接受信息不是很稳定，但是每次都能回调回去，我猜测是跟四次分手有关
                    Console.WriteLine("关闭连接");
                    Close();
                }
                else {
                    Console.WriteLine(DateTime.Now+":::::::::::"+"客户端的消息" + msg.Data + count);
                    msg.ReadMessage(count,OnProcessMessage);//接受消息并回调
                }
                Start();//循环调用，不然只能接受一次消息
            }
            catch(Exception e) {
                Console.WriteLine(e);
                Close();
            }
        }
        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data) {
            server.HandlerRequest(requestCode,actionCode,data,this);
        }
        public void Send(ActionCode actionCode,string data) {
            byte[] bytes = Message.PackData(actionCode, data);
            clientSocket.Send(bytes);
        }
        public void Close() {
            if (clientSocket != null) {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }
    }
}
