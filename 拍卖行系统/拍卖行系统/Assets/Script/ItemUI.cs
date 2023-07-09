/****************************************************
    文件：ItemUi.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/13 20:16:53
	功能：物类UI类，1.持有物品的基本信息，并设置 2.提供给外面判断物品多少的信息
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public Image itemicon;
    public Text itemname;
    public Text txtcount;
    public Item item;
    public int count;

    public void SetItem(Item item,int count=1) {
        this.item = item;
        this.count = count;
        this.txtcount.text ="";//为一个就设置为''
        
        itemicon.sprite = Resources.Load<Sprite>("ResImg/Item/"+item.Sprite);
        if (itemicon.sprite == null) {
            Debug.LogWarning("图片资源未找到");
        }
        switch (item.ItemQuality) {
            case Quality.Common:
                itemname.text ="<B><Color=while>" +item.Name+"</Color></B>";
                break;
            case Quality.Good:
                itemname.text = "<B><Color=blue>" + item.Name+ "</Color></B>";
                break;
            case Quality.Rare:
                itemname.text = "<B><Color=yellow>" + item.Name+ "</Color></B>";
                break;
            case Quality.Epic:
                itemname.text = "<B><Color=purple>" + item.Name+ "</Color></B>";
                break;
            case Quality.Legend:
                itemname.text = "<B><Color=red>" + item.Name+ "</Color></B>";
                break;
        }
    }
    public bool AddItem(int count=1){//这里只设置一个，不要计算多个
        if(this.count>=item.Capacity)
        return false;
        else {
           this.count+= count;
            this.txtcount.text = "<B>" + this.count.ToString()+"</B>";
            return true;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        UserWnd.Instance.ShowItemTip(GetItemUIInfo());
    }
    public void OnPointerExit(PointerEventData eventData) {
        UserWnd.Instance.HideItemTip();
    }
    public void OnPointerDown(PointerEventData eventData) {
        AudioSvc.Instance.PlayUIAudio(Constant.UIClickBtn);
        UserWnd.Instance.SellItemTip(this);
    }
    public string  GetItemUIInfo() {
        string itemName="";
        switch (item.ItemQuality) {
            case Quality.Common:
                itemName = "<B><Color=while>"+"名字："+item.Name + "</Color></B>" + "\n";
                break;
            case Quality.Good:
                itemName = "<B><Color=blue>" + "名字：" + item.Name + "</Color></B>" + "\n";
                break;
            case Quality.Rare:
                itemName = "<B><Color=yellow>" + "名字：" + item.Name + "</Color></B>" + "\n";
                break;
            case Quality.Epic:
                itemName = "<B><Color=purple>" + "名字：" + item.Name + "</Color></B>"+"\n";
                break;
            case Quality.Legend:
                itemName = "<B><Color=red>" + "名字：" + item.Name + "</Color></B>" + "\n";
                break;
        }

        string itemcount = "<Color=yellow>" + "数量：" + count.ToString() + "</Color>" + "\n";
        string itemcapacity = "<Color=yellow>" + "容量：" + item.Capacity.ToString()+"</Color>"+ "\n";
        string des = "描述："+item.Description + "\n";
        string buyprice = "购买价格：" + item.BuyPrice.ToString() + "\n";
        string sellprice = "出售价格：" + item.SellPrice.ToString()+ "\n";
        string strinfo = itemName+ itemcount + itemcapacity + des + buyprice + sellprice;
        return strinfo;
    }
}
public class Item {
    public int ID;
    public string Name;
    public Quality ItemQuality;
    public string Description;
    public int Capacity;
    public int BuyPrice;
    public int SellPrice;
    public string Sprite;
    public Item(int ID, string Name, Quality ItemQuality, string Description, int Capacity, int BuyPrice, int SellPrice, string Sprite) {
        this.ID = ID;
        this.Name = Name;
        this.ItemQuality = ItemQuality;
        this.Description = Description;
        this.Capacity = Capacity;
        this.BuyPrice = BuyPrice;
        this.SellPrice = SellPrice;
        this.Sprite = Sprite;
    }

}
public enum Quality {
    [Tooltip("常见")]
    Common,
    [Tooltip("优良")]
    Good,
    [Tooltip("稀有")]
    Rare,
    [Tooltip("史诗")]
    Epic,
    [Tooltip("传说")]
    Legend
}