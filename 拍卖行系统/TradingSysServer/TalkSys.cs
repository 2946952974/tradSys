/************************************************************
    文件: TalkSys.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 聊天系统
*************************************************************/

using MySql.Data.MySqlClient;
using System;
using TradCommon;

namespace TradingSysServer {
    class TalkSys {
        private MySqlConnection conn = null;
        public TalkSys() { }
        public TalkSys(MySqlConnection conn,Server server) {
            this.conn = conn;
            this.server = server;
        }
        private Client client;
        private Server server;
        public void HandlerAction(ActionCode action, string data, Client client) {
            this.client = client;
            //lock (conn) {//TODO
            //}
            switch (action) {
                case ActionCode.Talk://加载交易的信息
                    Talk(data, client, server);
                    break;
              
            }
        }
        public void Talk(string data,Client client,Server server) {//name：data
            //处理数据 查找账户名字，发送数据
            //string[] userinfo = data.Split(',');
            //string account = userinfo[0];
            //string talk = userinfo[1];
            //User user = QueryUser(account);
            //string talkinfo = user.name +":"+ talk;
            server.SendAll(data, client);
            client.Send(ActionCode.Talk,"true");//只有本地才会发送这个消息，显示成功
        }
        //private User QueryUser(string account) {
        //    conn.Open();
        //    MySqlCommand cmd = new MySqlCommand("select * from user where account=@account ", conn);
        //    cmd.Parameters.AddWithValue("account", account);
        //    MySqlDataReader reader = cmd.ExecuteReader();
        //    string name = "";
        //    string sex = "";
        //    int coin = 0;
        //    int dimonde = 0;
        //    DateTime registtime = DateTime.Now;

        //    int tradcount = 0;
        //    int buycount = 0;
        //    if (reader.Read()) {
        //        name = reader.GetString("name");
        //        sex = reader.GetString("sex");
        //        coin = reader.GetInt32("coin");
        //        dimonde = reader.GetInt32("diamond");
        //        sex = reader.GetString("sex");
        //        registtime = reader.GetDateTime("registtime");

        //        tradcount = reader.GetInt32("tradcount");
        //        buycount = reader.GetInt32("buycount");
        //    }
        //    User user = new User {
        //        account = account,
        //        name = name,
        //        sex = sex,
        //        coin = coin,
        //        diamond = dimonde,
        //        registtime = registtime,
        //        tradcount = tradcount,
        //        buycount = buycount
        //    };
        //    conn.Close();
        //    return user;
        //}
    }
}
