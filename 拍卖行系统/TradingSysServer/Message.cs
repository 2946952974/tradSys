/************************************************************
    文件: Message.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 消息类，用于打包消息
*************************************************************/

using System;
using System.Linq;
using System.Text;
using TradCommon;
namespace TradingSysServer {
    class Message {
        private byte[] data = new byte[1024];
        private int index = 0;//存取多少字节
        public byte[] Data {
            set {
                data = value;
                Console.WriteLine("setDATA" + data);
            }
            get {
                return data;
            }
        }
        public int StartIndex {
            get { return index; }
        }
        public int RemainSize {//剩余空间
            get {
                return data.Length - index;
            }
        }
        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> processDataCallBack){//回调函数
            index += newDataAmount;
            if (index <= 4) {
                return;
            }
            else {
                int count = BitConverter.ToInt32(Data, 0);
                RequestCode requestCode = (RequestCode)BitConverter.ToInt32(Data, 4);
                ActionCode actionCode  = (ActionCode)BitConverter.ToInt32(Data, 8);
                string s = System.Text.Encoding.UTF8.GetString(Data, 12, count-8);
                processDataCallBack(requestCode, actionCode, s);
                index = 0;
            }
            index = 0;
        }
        public static byte[] PackData(ActionCode actionCode,string data) {//打包消息
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;//协议的长度
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>(); //长度+协议
            return newBytes.Concat(dataBytes).ToArray<byte>();//（长度+协议）+数据
        }
       
    }
}
