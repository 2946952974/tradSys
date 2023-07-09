/****************************************************
    文件：UserWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/12 21:14:57
	功能：用户界面，需要持有用户信息
*****************************************************/

using System.Collections.Generic;
using TradCommon;
using UnityEngine;
using UnityEngine.UI;

public class UserWnd : MonoBehaviour {
    public static UserWnd Instance = null;
    #region 
    [HideInInspector]
    public string useraccount;//TODO，还没返回用户的账号信息
    [HideInInspector]
    public string  username;
    [HideInInspector]
    public int coin;
    [HideInInspector]
    public int dimonde;
    #endregion
    #region 用户信息，待完善，头像和部分信息
    public Image icon;//男或者女
    public Text txtname;
    public Text txtcoin;
    public Text txttradcount;
    public Text txtdimonde; 
    public Text txtbuycount;
    public Text txtregisttime;
    #endregion
    #region 背包信息设置
    public GameObject ItemGroup;//背包外框
    public GameObject ItemUIgo;
    private List<ItemUI> itemUIList = new List<ItemUI>();//这里负责添加物品，这个是一个无限扩充的

    private Canvas canvas;
    public ItemTip itemTip;//物品提示框
    public SellTip sellTip;//出售提示框
    private bool isItemTipShow = false;
    private Vector2 ItemTipPosOffset = new Vector2(10, -10);//提示框偏移量
    #endregion
    public void InitWnd() {
        Instance = this;
        canvas= GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void Login(string data) {//需要设置用户表 和 背包列表
        string[] strUserandBag = data.Split('^');//(sex|name...) +^+(id,count|id,count)
        string[] strUserList = strUserandBag[0].Split('|');//sex+"|"+name....
        string[] strBagList=null;
        if (strUserandBag.Length == 2) {
         strBagList= strUserandBag[1].Split('|');// name + "|" + sex + "|"+coin.ToString()+"|"+dimonde.ToString()+"|"+registtime.ToString()+"|"+tradcount.ToString()+"|"+buycount.ToString()+"|"+account;
        }
        useraccount =strUserList[7];
        username = strUserList[0];
        coin = int.Parse (strUserList[2]);
        dimonde = int.Parse (strUserList[3]);

        txtname.text ="<Color=yellow>"+ strUserList[0]+"</Color>";//name
        string sex = strUserList[1];
        if (sex.Equals("女")) {
            icon.sprite = Resources.Load<Sprite>("Icon/woman");
        }
        else {
            icon.sprite = Resources.Load<Sprite>("Icon/man");
        }
        txtcoin.text = "金币数量:   " + "<Color=yellow>" + strUserList[2]+"</Color>";//coin
        txtdimonde.text = "钻石数量:   " + "<Color=blue>" + strUserList[3]+"</Color>";//dimond
        txtregisttime.text = "注册时间:" + "<Color=red>"+ strUserList[4]+"</Color>";//registtime
        txttradcount.text = "交易次数:   " + "<Color=yellow>" + strUserList[5]+"</Color>";//tradcount
        txtbuycount.text= "购买次数:   " + "<Color=yellow>" + strUserList[6]+ "</Color>";//buycount

        if (strBagList != null) {
            for(int i = 0; i < strBagList.Length-1; i++) {
                string[] str = strBagList[i].Split(',');
                int id =int.Parse(str[0]);
                int count =int.Parse( str[1]);
                AddItem(id, count);
            }
        }
    }
    private void Update() {
        if (isItemTipShow) {
            Vector2 MousePoistion;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out MousePoistion);
            itemTip.SetItemTipPos(MousePoistion + ItemTipPosOffset);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameRoot.Instacne.ChangeTalk();
        }
    }
    public void ShowItemTip(string content) {
        isItemTipShow = true;
        itemTip.Show(content);
    }
    public void HideItemTip() {
        isItemTipShow = false;
        itemTip.Hide();
    }
    public void SellItemTip(ItemUI itemui) {
        sellTip.gameObject.SetActive(true);
        sellTip.SetInfo(itemui);
    }
    private void AddItem(int id,int count) {//如果新建的话是set，有的话就是add
            bool isadd = false;
            int addcount = count;
        while (addcount>0) {
            foreach(ItemUI itemui in itemUIList) {//背包中所有的都遍历一遍，找到空的填入
                if (itemui.item.ID == id&&addcount>0){
                    int curaddcount = addcount;
                    for(int i = 0; i <curaddcount; i++){
                        isadd = itemui.AddItem();
                        if (isadd == false) {
                            break;
                        }
                        addcount--;
                    }
                }
            }
            if (addcount!= 0) {//证明同类型已满，没有加载完毕,新建一个加入到列表中去
                Item item = ResSvc.Instance.GetItemByID(id);
                ItemUI itemui = Instantiate(ItemUIgo).GetComponent<ItemUI>();
                itemui.gameObject.transform.SetParent(ItemGroup.transform);
                itemui.SetItem(item);
                addcount--;
                itemUIList.Add(itemui);
            }
        }
    }
    public void BtnEnterTradWnd() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIOpenPage);
        GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Frensh,LoginWnd.Instance.inputAccount.text);//最好传自己的account？
        GameRoot.Instacne.OpenTrad();
    }
    public void FrenshUI(string userinfo,string userbag) {//只刷新 tradcount,buycount,coin,diamond
        string[] str = userinfo.Split(',');
        int tradcount = int.Parse(str[0]);
        int buycount = int.Parse(str[1]);
        int coin = int.Parse(str[2]);
        int diamonde = int.Parse(str[3]);
        this.coin = coin;
        this.dimonde = diamonde;
        txttradcount.text= "交易次数:   " + "<Color=yellow>" + tradcount+ "</Color>";//tradcount
        txtbuycount.text = "购买次数:   " + "<Color=yellow>" + buycount + "</Color>";//buycount
        txtcoin.text = "金币数量:   " + "<Color=yellow>" + coin + "</Color>";//coin
        txtdimonde.text = "钻石数量:   " + "<Color=blue>" + diamonde + "</Color>";//dimond
        
        //TODO 更新背包
        foreach(ItemUI itemUI in itemUIList) {
            Destroy(itemUI.gameObject);
        }
        itemUIList.Clear();
        string[] strbag = userbag.Split('|');
        for (int i = 0; i < strbag.Length - 1; i++) {
            string[] stritem = strbag[i].Split(',');
            int id = int.Parse(stritem[0]);
            int count = int.Parse(stritem[1]);
            AddItem(id, count);
        }
    }
}