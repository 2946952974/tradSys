/************************************************************
    文件: LoginSys.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 登录系统，编写登录协议
*************************************************************/

using System;
using TradCommon;
using MySql.Data.MySqlClient;
namespace TradingSysServer {
    public class LoginSys {

        private MySqlConnection conn = null;
        public LoginSys() {}
        public LoginSys(MySqlConnection conn) {
            this.conn = conn;
        }

        private Client client;
        public void HandlerAction(ActionCode action,string data,Client client) {
            this.client = client;
            switch (action) {
                case ActionCode.Login:
                    Login(data, client);
                    break;
                case ActionCode.Register:
                    Register(data, client);
                    break;
            }
        }
        private void Login(string data,Client client) {
            Console.WriteLine(DateTime.Now+":::登录:" + data);
            if (conn == null) {
                Console.WriteLine("数据库连接失败");
            }
            else {
                string[] strList = data.Split('|');
                string account = strList[0];
                string passward = strList[1];
                bool isCompear= Query(account, passward);
                if (isCompear) {
                    Console.WriteLine("登录成功");
                    string userdata=QueryData(account);
                    string userbag = QueryUserBag(account);
                    client.Send(ActionCode.Login,userdata+"^"+userbag);//(sex|name...) +^+(id,count|id,count)
                }
                else {
                    Console.WriteLine("登陆失败");
                    client.Send(ActionCode.Login, "false");
                }
                return;
            }
        }
        private void Register(string data, Client client) {
            Console.WriteLine(DateTime.Now+":::注册::"+data);
            if (conn == null) {
                Console.WriteLine("数据库连接失败");
            }
            else {
                string[] strList = data.Split('|');
                string account = strList[0];
                string pass = strList[1];
                string name = strList[2];
                string sex = strList[3];
                bool has = HasAccount(account);
                if (has) {
                    Console.WriteLine("已拥有此账号");
                    client.Send(ActionCode.Register, "false");
                }
                else {//这里写注册逻辑
                    bool isadd = AdddAcoount(account, pass, name, sex);
                    client.Send(ActionCode.Register, isadd.ToString());
                }
            }
        }

        private bool Query(string accont,string passward) {
            //lock (conn) {//出问题再来改
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where account=@account ",conn);
            cmd.Parameters.AddWithValue("account",accont);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool isCompear = false;
            if (reader.Read()) {
                string _pass = reader.GetString("passward");
                if (_pass.Equals(passward)) {
                    isCompear = true;
                }
                else {
                    isCompear = false;
                }
            }            
            conn.Close();
            return isCompear;
        }
        private string QueryData(string account) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", account);
            MySqlDataReader reader = cmd.ExecuteReader();
            string name="";
            string sex="";
            int coin=0;
            int dimonde=0;
            DateTime registtime=DateTime.Now;

            int tradcount=0;
            int buycount=0;
            if (reader.Read()) {
                name = reader.GetString("name");
                sex = reader.GetString("sex");
                coin = reader.GetInt32("coin");
                dimonde = reader.GetInt32("diamond");
                sex = reader.GetString("sex");
                registtime = reader.GetDateTime("registtime");

                tradcount = reader.GetInt32("tradcount");
                buycount = reader.GetInt32("buycount");
            }
            string data = name + "|" + sex + "|"+coin.ToString()+"|"+dimonde.ToString()+"|"+registtime.ToString()+"|"+tradcount.ToString()+"|"+buycount.ToString()+"|"+account;
            conn.Close();
            return data;
        }
        private string QueryUserBag(string account) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from userbag where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", account);
            MySqlDataReader reader = cmd.ExecuteReader();
            string userbag = "";
            int itemid=0;
            int count=0;
            try {
                while(reader.Read()) {
                   itemid= reader.GetInt32("itemid");  
                   count = reader.GetInt32("count");
                   userbag += itemid + "," + count+"|";
                }
            }
            catch (Exception e) {
                Console.WriteLine("数据库查询信息错误："+e);
            }
            conn.Close();
            return userbag;
        }
        private bool HasAccount(string account) {
            conn.Open();
            bool has = false;
            MySqlCommand cmd = new MySqlCommand("select * from user where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", account);
            MySqlDataReader reader = cmd.ExecuteReader();
           
            if (reader.Read()) {
                has = true;//有数据就代表有这个账号
            }
            conn.Close();
            return has;
        }
        private bool AdddAcoount(string account,string pass,string name,string sex) {
            conn.Open();
            int coin = 1000;
            int diamond = 100;
            DateTime registtime = DateTime.Now;
            MySqlCommand cmd = new MySqlCommand("insert into user set account=@account,passward=@passward,name=@name,sex=@sex," +
                "coin=@coin,diamond=@diamond,registtime=@registtime,tradcount=@tradcount,buycount=@buycount", conn);
            cmd.Parameters.AddWithValue("account", account);
            cmd.Parameters.AddWithValue("passward", pass);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("sex", sex);
            cmd.Parameters.AddWithValue("coin",coin);
            cmd.Parameters.AddWithValue("diamond",diamond);
            cmd.Parameters.AddWithValue("registtime", registtime);

            cmd.Parameters.AddWithValue("tradcount",0);
            cmd.Parameters.AddWithValue("buycount",0);

            int id=(int)cmd.ExecuteNonQuery();
            bool add = false;
            if (id!= 0) {//没有插入成功
                add = true;
            }
            conn.Close();
            return add;
        }
    }
}