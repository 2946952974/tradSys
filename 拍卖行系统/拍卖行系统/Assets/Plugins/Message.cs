/****************************************************
    文件：Message.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/10 19:33:5
	功能：传输数据管理类
*****************************************************/

using System;
using System.Linq;
using System.Text;
using TradCommon;
using UnityEngine;

public class Message {
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
    public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallBack) {//回调函数
        index += newDataAmount;
        if (index <= 4) {
            return;
        }
        else {
            int count = BitConverter.ToInt32(Data, 0);
            ActionCode actionCode = (ActionCode)BitConverter.ToInt32(Data, 4);
            string s = System.Text.Encoding.UTF8.GetString(Data,8, count - 4);
            processDataCallBack(actionCode,s);
            Console.WriteLine("接收到" + count + "字节");
        }
        index = 0;
    }
    public static byte[] PackData(RequestCode requestCode, ActionCode actionCode, string data)//包装
    {

        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        int dataAmount = actionCodeBytes.Length + requestCodeBytes.Length + dataBytes.Length;//数据长度
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        dataAmountBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>()
            .Concat(actionCodeBytes).ToArray<byte>()
            .Concat(dataBytes).ToArray<byte>();
        return dataAmountBytes;
    }

}