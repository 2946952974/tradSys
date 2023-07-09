/****************************************************
    文件：TalkWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/17 20:46:34
	功能：世界聊天页面
*****************************************************/

using TradCommon;
using UnityEngine;
using UnityEngine.UI;

public class TalkWnd : MonoBehaviour //被用户界面持有，按下回车键就会显示，如何关闭呢TDOO
{
    public static TalkWnd Instance = null;
    public void InitWnd() {
        Instance = this;
    }

    public InputField inputsenddata;//自己说的话
    public GameObject talkUIgo;
    public GameObject talkOtherUIgo;
    public GameObject talkGroup;

    public void BtnSendTalk() {
        //在本地生成一个，然后在往服务器发送一个
        TalkUI talkUI= Instantiate(talkUIgo).GetComponent<TalkUI>();
        talkUI.SetTalk(inputsenddata.text+":"+UserWnd.Instance.txtname.text);
        talkUI.gameObject.transform.SetParent(talkGroup.transform);
        string data = UserWnd.Instance.txtname.text+ ":"+inputsenddata.text;//acount:talk
        GameRoot.Instacne.Send(RequestCode.TalkSys, ActionCode.Talk, data);
    }
    public void SetTalk(string data){//直接显示 username：talk
        Debug.Log(data);
        TalkUI talkUI= Instantiate(talkOtherUIgo).GetComponent<TalkUI>();
        talkUI.SetTalk(data);
        talkUI.gameObject.transform.SetParent(talkGroup.transform);
    }
}