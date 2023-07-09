/************************************************************
    文件: ControlManager.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能:管理器，根据消息，系统之间跳转
*************************************************************/

using System;
using System.Collections.Generic;
using TradCommon;
using MySql.Data.MySqlClient;

namespace TradingSysServer {
    public class ControlManager {
        private LoginSys loginSys;
        private TradSys tradSys;
        private TalkSys talkSys;
        private Server server;
        public static MySqlConnection conn = null;

        public ControlManager() { }
        public ControlManager(Server server) {
            // conn = new MySqlConnection("server=localhost;User Id=root;passwrod=;Database=tradingsys;Charset=utf8");
            conn = new MySqlConnection("Database=tradingsys;Data Source=127.0.0.1;port=3306;User Id=root;Password=root");
            this.server = server;
            loginSys = new LoginSys(conn);
            tradSys = new TradSys(conn);
            talkSys = new TalkSys(conn, server);
        }

        public void HandlerRequest(RequestCode request, ActionCode action,string data, Client client) {
            switch (request) {
                case RequestCode.None:
                    Console.WriteLine("默认系统");
                    break;
                case RequestCode.LoginSys:
                    loginSys.HandlerAction(action, data, client);
                    break;
                case RequestCode.TradSys:
                    tradSys.HandlerAction(action, data, client);
                    break;
                case RequestCode.TalkSys:
                    talkSys.HandlerAction(action, data, client);
                    break;
            }

        }
    }
}