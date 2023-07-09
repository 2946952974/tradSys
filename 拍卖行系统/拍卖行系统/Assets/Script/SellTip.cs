/****************************************************
    文件：SellTip.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/16 21:33:32
	功能：Nothing
*****************************************************/

using TradCommon;
using UnityEngine;
using UnityEngine.UI;

public class SellTip : MonoBehaviour //account,itemid,sellcount=1
{
    public Image image;
    public Text txtinfo;
    public Text txtsellcount;
    private Item item;


    private int currentcount;//出售的数量
    private int maxcount;//最大值

    public void SetInfo(ItemUI itemUI) {
        this.item = itemUI.item;
        this.maxcount = itemUI.count;
        currentcount = 1;
        txtsellcount.text = currentcount.ToString();
        image.sprite= Resources.Load<Sprite>("ResImg/Item/"+ item.Sprite);
        txtinfo.text = "您是否要出售 " + currentcount + "个" + item.Name + "?" + "\n" + "售价：<Color=red>" + currentcount * item.SellPrice + "</Color>";
    }
    public void BtnSell() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        string account = LoginWnd.Instance.inputAccount.text;
        int itemid = item.ID;
        int sellcount = currentcount;//数量没有设置TDOO，以后扩展的时候再说吧
        GameRoot.Instacne.Send(RequestCode.TradSys, ActionCode.Sell,account+","+itemid+","+sellcount);
        gameObject.SetActive(false);
    }

    public void BtnAdd() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        currentcount++;
        if (currentcount > maxcount) {
            currentcount = maxcount;
        }
        RefrenshUI();
    }
    public void BtnReduce() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        currentcount--;
        if (currentcount < 1) {
            currentcount = 1;
        }
        RefrenshUI();
    }
    public void RefrenshUI() {
        txtsellcount.text = currentcount.ToString();
        txtinfo.text = "您是否要出售 "+currentcount+"个" + item.Name + "?" + "\n" + "售价：<Color=red>" + currentcount*item.SellPrice + "</Color>";
    }
    public void BtnReturn() {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        gameObject.SetActive(false);
    }
    
}