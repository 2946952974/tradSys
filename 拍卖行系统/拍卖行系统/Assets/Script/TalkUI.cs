/****************************************************
    文件：TalkUI.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/17 22:11:14
	功能：对话UI
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class TalkUI : MonoBehaviour 
{
    public Text txtTalk;
    public void SetTalk(string data) {
        txtTalk.text = data;
    }
}