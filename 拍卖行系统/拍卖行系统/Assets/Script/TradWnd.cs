/****************************************************
    文件：TradWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/14 9:1:55
	功能：交易界面 1.持有所有交易的信息，并且根据服务器传过来的信息进行更新
    //添加
    //更新,搜素等。
    //提交
    //tradid,account,username,itemid,sellcount。。。。。。。。。。。。。。。。//只能看见不是我提交的物品，我交易记录只能看我的，这样交易ID可以删除和复用了
*****************************************************/

using System.Collections.Generic;
using TradCommon;
using UnityEngine;
using UnityEngine.UI;

public class TradWnd : MonoBehaviour {
    //每个ID是固定的，id是不会删除的，但是里面的内容会改变，每次更改只用发送更改的信息就行了,信息打包，还是逐条？
    public static TradWnd Instance=null;
    #region 交易列表,和预制体
    public InputField inputfind;
    public GameObject tradGroup;
    public GameObject tradItem;
    #endregion
    private Dictionary<int, TradItemUI> tradUIList = new Dictionary<int, TradItemUI>();//tradid，info
    public void InitWnd() {
        Instance = this;
    }
    //int tradid=1;
    private void Update() {
        //if (Input.GetKeyDown(KeyCode.E)) {
        //    AddTradItem(tradid,"123","别离或雪",tradid,1000);
        //    tradid++;
        //}
        //if (Input.GetKeyDown(KeyCode.R)) {
        //    AddTradItem(1, "123", "你那被",5, 2000);
        //}
        //if (Input.GetKeyDown(KeyCode.D)) {
        //    AddTradItem(1, "???", "你那被", 5, 2000);
        //}
    }
    public void Trad(string data) {
        ClearTradList();
        string[] tradList = data.Split('|');
        for(int i = 0; i < tradList.Length - 1; i++) {
            string[] tradinfo = tradList[i].Split(',');
            int tradid = int.Parse(tradinfo[0]);
            string account = tradinfo[1];
            string  username = tradinfo[2];
            int itemid = int.Parse(tradinfo[3]);
            int sellcount = int.Parse(tradinfo[4]);
            AddTradItem(tradid, account, username, itemid, sellcount);
        }
    }
    public void BtnReturn() {
        AudioSvc.Instance.PlayUIAudio(Constant.UICloseBtn);
        GameRoot.Instacne.CloseTrad();
        GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Frensh, LoginWnd.Instance.inputAccount.text);
    }
    public void BtnFrensh() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        if (inputfind.text!= "") {
            string find = inputfind.text;
            GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Trad, find);//刷新，有搜索，只刷新搜索，没有刷新就刷新全部
        }
        else {
            GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Frensh, LoginWnd.Instance.inputAccount.text);
        }
    }
    public void BtnFind(){
        string find = inputfind.text;
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        
        GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Trad,find);//模糊搜索、、搜查用户名，或者物品名字，这里需要根据物品名字转换成物品id，名字要准确
    }
    public void AddTradItem(int tradid,string account,string username,int itemid,int sellcount) {
        if (tradUIList.ContainsKey(tradid)) {//已拥有这个id，更新或者删除
            //后续信息是有的，就更新
            //后续信息没有就移出这个列表？因为交易完成了，如何处理这个交易列表呢，数据库每次交易完毕都会吧这个交易id内容制空，并把更新的数据（全部）发送过来进行更新，移出就可以，后续在使用这个id的时候又会加进来
            if (account == "???") {//表示没有账户和数据
                TradItemUI DeltradItem;
                tradUIList.TryGetValue(tradid, out DeltradItem);
                tradUIList.Remove(tradid);
                Destroy( DeltradItem.gameObject);
            }
            else {
                TradItemUI curtradItemUI;
                tradUIList.TryGetValue(tradid,out curtradItemUI);
                curtradItemUI.SetTradInfo(tradid, account, username, itemid, sellcount);
            }
            
        }
        else {
            TradItemUI tradItemUI= Instantiate(tradItem).GetComponent<TradItemUI>();
            tradItemUI.SetTradInfo(tradid, account, username, itemid, sellcount);
            tradItemUI.gameObject.transform.SetParent(tradGroup.transform);
            tradUIList.Add(tradItemUI.tradid, tradItemUI);
        }
    }

    public void FrenshUI(string data) {//刷新交易页面，直接用addtraditem就性；
        string[] tradList = data.Split('|');
        ClearTradList();
        for (int i = 0; i < tradList.Length - 1; i++) {
            string[] tradinfo = tradList[i].Split(',');
            int tradid = int.Parse(tradinfo[0]);
            string account = tradinfo[1];
            string username = tradinfo[2];
            int itemid = int.Parse(tradinfo[3]);
            int sellcount = int.Parse(tradinfo[4]);
            AddTradItem(tradid, account, username, itemid, sellcount);
        }
    }
    public void ClearTradList() {
        TradItemUI[] tradItemUIlst = tradGroup.GetComponentsInChildren<TradItemUI>();
        for(int i = 0; i < tradItemUIlst.Length; i++) {
            Destroy(tradItemUIlst[i].gameObject);
        }
        tradUIList.Clear();
    }

}