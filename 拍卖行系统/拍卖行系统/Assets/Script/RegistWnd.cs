/****************************************************
    文件：RegistWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/11 23:22:58
	功能：注册界面
*****************************************************/

using TradCommon;
using UnityEngine;
using UnityEngine.UI;
public class RegistWnd : MonoBehaviour {
    public InputField inputname;
    public Toggle toggleMan;
    public Toggle toggleWoman;
    private string sex;
    public InputField inputaccount;
    public InputField inputpass;//密码
    public InputField inputRepass;//确认密码
    
    public void BtnRegist() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        IsMan();
       bool isempty= CheckEmpty();
       bool iscompear= CheckPass(inputpass.text, inputRepass.text);
        if (!isempty) {
            if (iscompear) {
                Debug.Log("注册账户");
                string data = inputaccount.text + "|" + inputpass.text+"|"+inputname.text+"|"+sex;
                GameRoot.Instacne.Send(RequestCode.LoginSys,ActionCode.Register,data);
            }
            else {
                GameRoot.Instacne.AddTips("两次密码不一致");
            }
        }        
    }
    public void BtnReturn() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        GameRoot.Instacne.OpenLogin();
    }

    public bool CheckEmpty() {
        bool isempty = false;
        if(inputname.text=="") {
            GameRoot.Instacne.AddTips("名字不能为空");
            isempty = true;
            return isempty;
        }
        if (inputaccount.text == "") {
            GameRoot.Instacne.AddTips("账号不能为空");
            isempty = true;
            return isempty;
        }
        if (inputpass.text == "") {
            GameRoot.Instacne.AddTips("密码不能为空");
            isempty = true;
            return isempty;
        }
        if (inputRepass.text == "") {
            GameRoot.Instacne.AddTips("第二次密码不能为空");
            isempty = true;
            return isempty;
        }
        if (sex=="") {
            GameRoot.Instacne.AddTips("性别不能为空");
            isempty = true;
            return isempty;
        }
        return isempty;
    }
    public bool CheckPass(string pass1,string pass2) {
        if (pass1.Equals(pass2)) {
            return true;
        }
        return false;
    }

    private void IsMan() {
        if (toggleMan.isOn) {
            sex = "男";
        }
        if (toggleWoman.isOn) {
            sex = "女";
        }
    }
}