/************************************************************
    文件: TradSys.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 交易系统，交易相关处理
*************************************************************/
using MySql.Data.MySqlClient;
using System;
using TradCommon;
namespace TradingSysServer {
    class TradSys {
        private MySqlConnection conn = null;
        public TradSys() { }
        public TradSys(MySqlConnection conn) {
            this.conn = conn;
        }
        private Client client;
        public void HandlerAction(ActionCode action, string data, Client client) {
            this.client = client;
            //lock (conn) {//TODO

            //}
            switch (action) {
                case ActionCode.Trad://加载交易的信息
                    Trad(data,client);
                    break;
                case ActionCode.Buy:
                    Buy(data,client);
                    break;
                case ActionCode.Sell:
                    Sell(data, client);
                    break;
                case ActionCode.Frensh:
                    Frensh(data,client);
                    break;
            }
        }
        private void Trad(string data,Client client) {//这里只有搜索才用，其他的都用刷新，这里进行模糊搜索，先模糊查找到物品
            Console.WriteLine(DateTime.Now + ":::查询交易:" + data);
            if (conn == null) {
                Console.WriteLine("数据库连接失败");
            }
            else {
                conn.Open();
                int curitemid=-1;//先模糊查询物品
                MySqlCommand cmdfinditem = new MySqlCommand("select * from item where itemname LIKE '%"+data+"%'", conn);
                //cmdfinditem.Parameters.AddWithValue("itemname",data);
                MySqlDataReader reader = cmdfinditem.ExecuteReader();
                if (reader.Read()) {
                    curitemid = reader.GetInt32("itemid");
                }
                Console.WriteLine("模糊物品id："+curitemid);

                reader.Close();
                MySqlCommand cmd = new MySqlCommand("select * from trad where username LIKE '%"+data+"%' or itemid=@itemid", conn);
                //cmd.Parameters.AddWithValue("username",data);
                cmd.Parameters.AddWithValue("itemid", curitemid);
                
                reader = cmd.ExecuteReader();

                int tradeid =0;
                string account = "";
                string username = "";
                int itemid = 0;
                int sellcount = 0;

                string tradinfo="";
                while(reader.Read()) {
                    tradeid = reader.GetInt32("tradeid");
                    account = reader.GetString("account");
                    if (account == "???") {//等会持有自己的信息，在登陆的时候
                        continue;//此条交易已结束
                    }
                    username = reader.GetString("username");
                    itemid = reader.GetInt32("itemid");
                    sellcount=reader.GetInt32("sellcount");
                    tradinfo += tradeid.ToString() + "," + account +","+ username + "," + itemid.ToString() + ","+ sellcount.ToString()+"|";
                }
                client.Send(ActionCode.Trad, tradinfo);
                conn.Close();
            }
        }
        private void Buy(string data, Client client) {// tradeid.ToString() + "," + myaccount + "," + targetaccount + "," + itemid + "," + coin;
            //保险起见判断能否购买，更改两个账户的信息，更改trad信息，先更改trad，再更改另外两个的
            string[] strbuyList = data.Split(',');
            int tradid = int.Parse(strbuyList[0]);
            string myaccount = strbuyList[1];
            string targetaccount = strbuyList[2];
            int itemid =int.Parse(strbuyList[3]);
            int coin = int.Parse(strbuyList[4]);
            //查询物品
            Trad trad = QueryTrad(tradid);//这里多一些判断，itemid
            //TODO
            if (!trad.account.Equals(targetaccount)) {
                client.Send(ActionCode.Buy, "false");
                return;
            }else if (!trad.itemid.Equals(itemid)) {
                client.Send(ActionCode.Buy, "false");
                return;
            }
            User myuser=QueryUser(myaccount);
            User targetuser = QueryUser(targetaccount);
            int cost = QueryItem(trad.itemid).sellprice * trad.sellcount;
            if (myuser.coin >=cost) {//钱够//更新数据
                Trad newtrad = new Trad {//跟新交易表,更新用户表，更新背包TDOO
                    tradid = trad.tradid,
                    account="???",
                    username="???",
                    itemid=0,
                    sellcount=0,
                };
                myuser.coin -= cost;
                myuser.buycount++;
                myuser.tradcount++;
                targetuser.coin += cost;
                targetuser.tradcount++;
                UpdateTrad(newtrad);
                UpdateUser(myuser);
                UpdateUser(targetuser);
                //TODO更新用户背包
                UserBag mybag = QueryUserBag(myaccount, trad.itemid);//只更新自己的，在提交交易的时候就已经扣除了
                //UserBag targtebag = QueryUserBag(targetaccount, trad.itemid);
                mybag.count += trad.sellcount;
                //targtebag.count -= trad.sellcount;
                UpdateUserBag(mybag);
                //UpdateUserBag(targtebag);
                
                User myinfo = QueryUser(myaccount);
                string userdata=myinfo.coin.ToString()+","+myinfo.diamond+","+myinfo.tradcount+","+myinfo.buycount;
                client.Send(ActionCode.Buy,"true");

            }
            else {
                //钱不够购买失败
                client.Send(ActionCode.Buy, "false");
                return;
            }
        }
        private void Sell(string data,Client client) {//找到一个不为？？？的交易，更新数据，没有就插入一条
            //TODO
            Console.WriteLine(DateTime.Now + ":::" + data);//account,itemid,count
            //遍历交易列表，更新，没有更新就插入，加锁加在每个大的方法上，不是小的方法，比如这里，这样执行的比较完整，返回的时候就不用管了，客户端需要刷新一遍
            string[] tradinfo = data.Split(',');
            string account = tradinfo[0];
            int itemid =int.Parse(tradinfo[1]);
            int sellcount =int.Parse(tradinfo[2]);
            InsertTrad(account,itemid, sellcount);
            client.Send(ActionCode.Sell, "true");
        }
        private void Frensh(string account,Client client) {//刷新，用户信息，背包，交易
            User user = QueryUser(account);//之刷新这几个
            int tradcount=user.tradcount;
            int buycount=user.buycount;
            int coin= user.coin;
            int dimonde = user.diamond;
            string userbag=FrenshUserBag(account);//id,count|id,count|
            string strtrad=FrenshTrad();//tradeid.ToString() + "," + account + "," + username + "," + itemid.ToString() + "," + sellcount.ToString() + "|";
            string frenshinfo = tradcount + "," + buycount + "," + coin + "," + dimonde + "^" + userbag+"^"+strtrad;
            client.Send(ActionCode.Frensh, frenshinfo);
        }

        #region 操作语句
        private User QueryUser(string account) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", account);
            MySqlDataReader reader = cmd.ExecuteReader();
            string name = "";
            string sex = "";
            int coin = 0;
            int dimonde = 0;
            DateTime registtime = DateTime.Now;
            
            int tradcount = 0;
            int buycount = 0;
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
            User user = new User {
                account = account,
                name = name,
                sex = sex,
                coin = coin,
                diamond=dimonde,
                registtime=registtime,
                tradcount=tradcount,
                buycount=buycount
            };
            conn.Close();
            return user;
        }
        private Trad QueryTrad(int tradeid) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from trad where tradeid=@tradeid ", conn);
            cmd.Parameters.AddWithValue("tradeid", tradeid);
            MySqlDataReader reader = cmd.ExecuteReader();
            string account="";
            string username = "";
            int itemid = 0;
            int sellcount = 0;
            if (reader.Read()) {
              account= reader.GetString("account");
              username= reader.GetString("username");
              itemid= reader.GetInt32("itemid");
              sellcount=reader.GetInt32("sellcount");
            }
            Trad trad = new Trad { 
                tradid=tradeid,
                account=account,
                username=username,
                itemid=itemid,
                sellcount=sellcount
            };
            conn.Close();
            return trad;
        }
        private Item QueryItem(int itemid) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from item where itemid=@itemid ", conn);
            cmd.Parameters.AddWithValue("itemid", itemid);
            MySqlDataReader reader = cmd.ExecuteReader();
            string itemname="";
            int sellprice=0;
            int buyprice=0;
            if (reader.Read()) {
               itemname= reader.GetString("itemname");
               sellprice= reader.GetInt32("sellprice");
               buyprice= reader.GetInt32("buyprice");
            }
            Item item = new Item {
                itemid = itemid,
                itemname=itemname,
                sellprice=sellprice,
                buyprice=buyprice,
            };
            conn.Close();
            return item;
        }

        //更新或者插入//TODO
        private UserBag QueryUserBag(string account,int itemid) {//没有的就返回为数量为0的物品
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from userbag where account=@account and itemid=@itemid", conn);
            cmd.Parameters.AddWithValue("account", account);
            cmd.Parameters.AddWithValue("itemid", itemid);

            MySqlDataReader reader = cmd.ExecuteReader();
            int count = 0;
            if (reader.Read()) {
                count = reader.GetInt32("count");
            }
            UserBag userBag = new UserBag { 
                account=account,
                itemid=itemid,
                count=count
            };
            conn.Close();
            return userBag;
        }
        private void InsertUserBag(string account,int itemid,int count) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("insert into userbag set account=@account,itemid=@itemid,count=@count ", conn);
            cmd.Parameters.AddWithValue("account", account);
            cmd.Parameters.AddWithValue("itemid", itemid);
            cmd.Parameters.AddWithValue("count", count);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        private void UpdateUserBag(UserBag userBag) {//在更新的时候判断是否有，没有就插入
            bool hasitem = false; ;
            conn.Open();
            MySqlCommand quecmd = new MySqlCommand("select * from userbag where account=@account and itemid=@itemid", conn);
            quecmd.Parameters.AddWithValue("account", userBag.account);
            quecmd.Parameters.AddWithValue("itemid", userBag.itemid);
            MySqlDataReader quereader = quecmd.ExecuteReader();
            string account = "";
            int itemid=-1;
            if (quereader.Read()) {//证明有数据
                account = quereader.GetString("account");
                itemid = quereader.GetInt32("itemid");
            }
            if (account!= "" && itemid != -1) {
                hasitem = true;
            }
            conn.Close();
            if (hasitem) {//有数据更新
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update userbag set count=@count where account=@account and itemid=@itemid", conn);
                cmd.Parameters.AddWithValue("count", userBag.count);
                cmd.Parameters.AddWithValue("account", userBag.account);
                cmd.Parameters.AddWithValue("itemid", userBag.itemid);

                MySqlDataReader reader = cmd.ExecuteReader();
                string itemname = "";
                int sellprice = 0;
                int buyprice = 0;
                if (reader.Read()) {
                    itemname = reader.GetString("itemname");
                    sellprice = reader.GetInt32("sellprice");
                    buyprice = reader.GetInt32("buyprice");
                }
                conn.Close();
            }
            else {
                //没有数据插入新数据
                InsertUserBag(userBag.account,userBag.itemid,userBag.count);
            }
        }
        private void UpdateTrad(Trad trad) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update trad set account=@account,username=@username,itemid=@itemid,sellcount=@sellcount  where tradeid=@tradeid", conn);
            cmd.Parameters.AddWithValue("account",trad.account);
            cmd.Parameters.AddWithValue("username", trad.username);
            cmd.Parameters.AddWithValue("itemid", trad.itemid);
            cmd.Parameters.AddWithValue("sellcount", trad.sellcount);
            
            cmd.Parameters.AddWithValue("tradeid", trad.tradid);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        private void UpdateUser(User user) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update user set coin=@coin,diamond=@diamond ,tradcount=@tradcount,buycount=@buycount where account=@account", conn);
            cmd.Parameters.AddWithValue("coin", user.coin);
            cmd.Parameters.AddWithValue("diamond", user.diamond);
            cmd.Parameters.AddWithValue("tradcount", user.tradcount);
            cmd.Parameters.AddWithValue("buycount", user.buycount);

            cmd.Parameters.AddWithValue("account", user.account);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }

        private void InsertTrad(string account,int itemid,int sellcount) {//更新用户背包,遍历空的交易id，有就更新,没有就插入,
            User user = QueryUser(account);
            UserBag userBag = QueryUserBag(account,itemid);
            userBag.count -= sellcount;
            UpdateUserBag(userBag);

            conn.Open();
            int tradeid = -1;
            bool hasempty=false;
            MySqlCommand cmd = new MySqlCommand("select * from trad where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", "???");
            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()) {
                tradeid = reader.GetInt32("tradeid");
            }
            if (tradeid == -1) {//找到最新的
                reader.Close();
                int curtradeid = 0;
                hasempty = false;
                MySqlCommand cmdselect = new MySqlCommand("select * from trad", conn);
                reader = cmdselect.ExecuteReader();
                while(reader.Read()) {
                    curtradeid++;
                }
                curtradeid++;//下一条索引
                tradeid = curtradeid;
                Console.WriteLine("tradeid"+tradeid);
            }
            else {
                hasempty = true;
            }

            if (hasempty) {//有空的位置,这里逻辑需要判断
                reader.Close();
                MySqlCommand cmdupdate = new MySqlCommand("Update trad set account=@account,username=@username,itemid=@itemid,sellcount=@sellcount  where tradeid=@tradeid", conn);
                cmdupdate.Parameters.AddWithValue("account",user.account);
                cmdupdate.Parameters.AddWithValue("username",user.name);
                cmdupdate.Parameters.AddWithValue("itemid",itemid);
                cmdupdate.Parameters.AddWithValue("sellcount", sellcount);
                cmdupdate.Parameters.AddWithValue("tradeid",tradeid);
                reader = cmdupdate.ExecuteReader();
            }
            else{//没有空的位置//如何获取tradid最大值呢
                reader.Close();
                MySqlCommand cmdinsert = new MySqlCommand("insert into trad set tradeid=@tradeid, account=@account,username=@username,itemid=@itemid,sellcount=@sellcount ", conn);
                cmdinsert.Parameters.AddWithValue("tradeid", tradeid);
                cmdinsert.Parameters.AddWithValue("account",user.account);
                cmdinsert.Parameters.AddWithValue("username",user.name);
                cmdinsert.Parameters.AddWithValue("itemid",itemid);
                cmdinsert.Parameters.AddWithValue("sellcount",sellcount);
                reader = cmdinsert.ExecuteReader();
                Console.WriteLine("insert:" + tradeid);
            }
            conn.Close();
        }
        #endregion
        private string FrenshUserBag(string account) {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from userbag where account=@account ", conn);
            cmd.Parameters.AddWithValue("account", account);
            MySqlDataReader reader = cmd.ExecuteReader();
            string userbag = "";
            int itemid = 0;
            int count = 0;
            try {
                while (reader.Read()) {
                    itemid = reader.GetInt32("itemid");
                    count = reader.GetInt32("count");
                    userbag += itemid + "," + count + "|";
                }
            }
            catch (Exception e) {
                Console.WriteLine("数据库查询信息错误：" + e);
            }
            conn.Close();
            return userbag;
        }
        private string FrenshTrad() {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from trad", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int tradeid = 0;
            string account = "";
            string username = "";
            int itemid = 0;
            int sellcount = 0;

            string tradinfo = "";
            while (reader.Read()) {
                tradeid = reader.GetInt32("tradeid");
                account = reader.GetString("account");
                if (account == "???") {//等会持有自己的信息，在登陆的时候
                    continue;//此条交易已结束
                }
                username = reader.GetString("username");
                itemid = reader.GetInt32("itemid");
                sellcount = reader.GetInt32("sellcount");
                tradinfo += tradeid.ToString() + "," + account + "," + username + "," + itemid.ToString() + "," + sellcount.ToString() + "|";
            }
            conn.Close();
            return tradinfo;
        }
    }
}
