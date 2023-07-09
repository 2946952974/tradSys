/****************************************************
    文件：LoginWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/11 22:47:20
	功能：登录界面
*****************************************************/

using TradCommon;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : MonoBehaviour {
    public static LoginWnd Instance=null;
    public InputField inputAccount; //这里输入的就是账号
    public InputField inputPass;
    public void InitWnd() {
        Instance = this;
    }
    public void Login() {
        AudioSvc.Instance.PlayUIAudio(Constant.UILoginBtn);
        if (inputAccount.text == "" || inputPass.text == "") {
            GameRoot.Instacne.AddTips("账户和密码不能为空！！！");
            return;
        }
       
        string data = inputAccount.text+"|"+inputPass.text;
        GameRoot.Instacne.Send(RequestCode.LoginSys,ActionCode.Login,data);
    }  
    public void Regist() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        GameRoot.Instacne.OpenRegist();
    }
}