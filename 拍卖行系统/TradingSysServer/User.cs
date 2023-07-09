/************************************************************
    文件: User.cs
	作者: 别离或雪
    邮箱: 2946952974@qq.com
    日期: 2021/10/9 18:13
	功能: 用户类，用于存储用户信息
*************************************************************/


using System;

namespace TradingSysServer {
    class User {
        public int id;
        public string account;
        public string passward;//这个好像不需要
        public string name;
        public string sex;
        public int coin;
        public int diamond;
        public DateTime registtime;
        public int tradcount;
        public int buycount;
    }
    class Trad {
        public int tradid;
        public string account;
        public string username;
        public int itemid;
        public int sellcount;
    }
    class Item {
        public int itemid;
        public string itemname;
        public int sellprice;
        public int buyprice;
    }
    class UserBag {
       public string account;
       public int itemid;
       public int count;
    }
}
