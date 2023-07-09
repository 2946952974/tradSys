/****************************************************
    文件：GameRoot.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/11 22:27:42
	功能：脚本入口
*****************************************************/

using TradCommon;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    public static GameRoot Instacne;
    public NetSvc netSvc;
    public ResSvc resSvc;
    public AudioSvc audioSvc;
    public DynamicWnd dynamicWnd;
  
    #region  持有各种界面，控制界面开关
    public LoginWnd loginWnd;
    public RegistWnd registWnd;
    public UserWnd userWnd;
    public TradWnd tradWnd;
    public TalkWnd talkWnd;
    private string dataloginSync="";//登录信息返回值
    private string datatradSync = "";//交易记录返回值
    private string databuySync = "";//购买返回值
    private string datafrenshSync = "";//刷新信息返回值
    private string datasellSync = "";//提交物品返回值
    private string datatalkSync = "";//聊天返回值
    private string tipslogin ="";
    private string tipregist = "";
    #endregion
    private void Start() {
        Instacne = this;
        Init();
    }
    public void Init() {
        //资源服务
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvs();
        //界面
        dynamicWnd.InitWnd();
        loginWnd.InitWnd();
        userWnd.InitWnd();
        tradWnd.InitWnd();
        talkWnd.InitWnd();
        netSvc.OnInit();//网络模块
    }
    public void Send(RequestCode request,ActionCode action,string data) {
        if (netSvc != null) {
            netSvc.SenRequest(request,action,data);
        }
        else {
            Debug.LogError("网络模块未实例化");
        }
    }

    private void Update() {
        if (tipregist != "") {//注册
            if (tipregist.Equals("false")) {
                AddTips("账号已存在！！！");
            }
            else {
                AddTips("注册成功");
            }
            tipregist = "";
        }
        if (tipslogin != "") {//登录
            AddTips(tipslogin);
            tipslogin = "";
        }
        else {
            if (dataloginSync != "") {
                Login(dataloginSync);
                dataloginSync = "";
            }
        }
        if (datatradSync != "") {
            Trad(datatradSync);
            datatradSync = "";
        }
        if (databuySync != "") {
            Buy(databuySync);
            databuySync = "";
        }
        if (datafrenshSync != "") {
            Frensh(datafrenshSync);
            datafrenshSync = "";
        }
        if (datasellSync != "") {
            Sell(datasellSync);
            datasellSync = "";
        }
        if (datatalkSync != "") {
            Talk(datatalkSync);
            datatalkSync = "";
        }
    }

    #region LoginandRegist
    public void OpenLogin() {
        loginWnd.gameObject.SetActive(true);
        registWnd.gameObject.SetActive(false);
    }
    public void OpenRegist() {
        registWnd.gameObject.SetActive(true);
        loginWnd.gameObject.SetActive(false);
    }
    public void OpenUser() {
        userWnd.gameObject.SetActive(true);
        registWnd.gameObject.SetActive(false);
        loginWnd.gameObject.SetActive(false);
    }
    public void ChangeTalk() {
        bool activself = talkWnd.gameObject.activeSelf;
        talkWnd.gameObject.SetActive(!activself);
    }
    public void OpenTrad() {
        tradWnd.gameObject.SetActive(true);
    }
    public void CloseTrad() {
        tradWnd.gameObject.SetActive(false);
    }
    #endregion
    #region 处理网络消息
    public void Register(string data) {
        if (data.Equals("false")) {
            tipregist =data;
        }
        else {
           
        }
    }
    public void Login(string data) {//主线程，通过update间接调用
        OpenUser();
        userWnd.Login(data);
    }
    public void Trad(string data) {
        tradWnd.Trad(data);
    }
    public void Buy(string data) {
        if (data.Equals("false")) {
            GameRoot.Instacne.AddTips("交易已更新");
        }
        else {
            GameRoot.Instacne.AddTips("购买成功");
        }
        Send(RequestCode.TradSys, ActionCode.Frensh,loginWnd.inputAccount.text);//刷新
    }
    public void Frensh(string data) {
        string[] frenshinfo= data.Split('^');
        string userinfo=frenshinfo[0];
        string userbag=frenshinfo[1];
        string strtrad=frenshinfo[2];
        UserWnd.Instance.FrenshUI(userinfo, userbag);
        TradWnd.Instance.FrenshUI(strtrad);
    }
    public void Sell(string data) {
        if (data.Equals("false")) {
            GameRoot.Instacne.AddTips("提交物品失败");
        }
        else {
            GameRoot.Instacne.AddTips("提交成功");
        }
       Send(RequestCode.TradSys, ActionCode.Frensh,loginWnd.inputAccount.text);
    }
    public void Talk(string data) {
        if (data.Equals("true")) {//发送成功
            GameRoot.Instacne.AddTips("发送成功");
        }
        else {//其他客户端发送过来
            talkWnd.SetTalk(data);
        }
    }
    public void TalkSync(string data) {
        datatalkSync = data;
    }
    public void SellSync(string data) {
        datasellSync = data;
    }
    public void FreshSync(string data) {
        datafrenshSync = data;
    }
    public void BuySync(string data) {//无论如何都需要更新UI，服务器需要一个刷新方法
        databuySync = data;
    }
    public void TradSync(string data) {
        datatradSync = data;
    }
    public void LoginSync(string data) {//传递到用户界面,//这些信息都是非主线程调用的，不能直接干预主线程，异步接受，update判断
        if (data == "false") {
            tipslogin = "账号或密码错误！！！";
        }
        else {
            tipslogin = "登录成功";
            dataloginSync = data;
        }
    }
    #endregion

    public void AddTips(string tip) {
        dynamicWnd.AddTips(tip);
    }
}