/****************************************************
    文件：TradItemUI.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/14 9:28:38
	功能：交易UI 1.根据传入的参数添加交易，按钮上交信息
*****************************************************/

using TradCommon;
using UnityEngine;
using UnityEngine.UI;
public class TradItemUI : MonoBehaviour {
    #region 交易信息
    public int tradid;
    public string account;
    public string username;
    public int itemid;
    public int sellcount;
    #endregion

    public Text txtname;
    public Image img;
    public Text txtusername;
    public Text txtsellcount;
    public void SetTradInfo(int tradid,string account,string username,int itemid,int sellcount) {//提供给TradWnd使用
        this.tradid = tradid;
        this.account = account;
        this.username = username;
        this.itemid = itemid;
        this.sellcount = sellcount;//出售数量
        SetTradItem();
    }
    private void SetTradItem() {
        Item item = ResSvc.Instance.GetItemByID(itemid);
        switch (item.ItemQuality) {
            case Quality.Common:
                txtname.text = "<B><Color=while>" + item.Name + "</Color></B>";
                break;
            case Quality.Good:
                txtname.text = "<B><Color=blue>" + item.Name + "</Color></B>";
                break;
            case Quality.Rare:
                txtname.text = "<B><Color=yellow>" + item.Name + "</Color></B>";
                break;
            case Quality.Epic:
                txtname.text = "<B><Color=purple>" + item.Name + "</Color></B>";
                break;
            case Quality.Legend:
                txtname.text = "<B><Color=red>" + item.Name + "</Color></B>";
                break;
        }
        img.sprite = Resources.Load<Sprite>("ResImg/Item/" + item.Sprite);
        txtusername.text ="出售者：" +username;
        txtsellcount.text ="数量:<color=yellow>"+sellcount+"</color>   "+"价格：<color=red>"+(item.SellPrice*sellcount).ToString()+"</color>";
    }
    public void BtnBuy() {
        Debug.Log("购买"+tradid);
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        int tradeid = this.tradid;
        string myaccount = UserWnd.Instance.useraccount;//我的账号
        string targetaccount = this.account;//目标账户
        int itemid = this.itemid;

        int coin = UserWnd.Instance.coin;

        string buydata = tradeid.ToString() + "," + myaccount + "," + targetaccount + "," + itemid + "," + coin;
        GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Buy, buydata);

        //string myaccount=UserWnd.Instance.useraccount;
        //UserWnd.Instance.coin-= 100;
        //string  account = this.account;
        
        //Debug.Log("coin" + UserWnd.Instance.coin);//可以找到里面的变量并使用
        //发送网络消息，更新交易列表和用户列表//计算金钱够不够，需要
        //tradid   account(出售者) 购买者id。  
    }

}