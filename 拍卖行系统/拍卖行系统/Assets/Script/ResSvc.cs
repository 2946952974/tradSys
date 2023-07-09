/****************************************************
    文件：ResSvc.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/13 20:15:3
	功能：资源加载服务
*****************************************************/

using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance = null;
    public void InitSvc() {
        Instance = this;
        Debug.Log("Init ResSvc......");
        InitItemCfg();
    }
    #region 声音加载
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false) {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au)) {
            au = Resources.Load<AudioClip>(path);
            if (cache) {
                adDic.Add(path, au);
            }
        }
        return au;
    }
    #endregion
    #region 物品配置加载
    private Dictionary<int, Item> ItemList = new Dictionary<int, Item>();
    private void InitItemCfg() {
        TextAsset xml = Resources.Load<TextAsset>("ResCfg/traditem");
        if (!xml) {
            Debug.LogWarning("xml dile:" + "文件路径" + "not exist");
        }
        else {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList Items = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < Items.Count; i++) {
                //共有
                int id = 0;
                string name = "";
                Quality quality = Quality.Common;
                string description = "";
                int capacity = 0;
                int buyprice = 0;
                int sellPrice = 0;
                string sprite = "";
                foreach (XmlNode e in Items[i].ChildNodes) {//item[];//先判断类型，再通过通过分类分开赋值
                    switch (e.Name) {
                        case "id":
                            id = int.Parse(e.InnerText);
                            break;
                        case "name":
                            name = e.InnerText;
                            break;
                        case "quality":
                            quality = (Quality)System.Enum.Parse(typeof(Quality), e.InnerText);
                            break;
                        case "description":
                            description = e.InnerText;
                            break;
                        case "capacity":
                            capacity = int.Parse(e.InnerText);
                            break;
                        case "buyprice":
                            buyprice = int.Parse(e.InnerText);
                            break;
                        case "sellPrice":
                            sellPrice = int.Parse(e.InnerText);
                            break;
                        case "sprite":
                            sprite = e.InnerText;
                            break;
                    }
                }
                Item consumable = new Item(id, name,quality, description, capacity, buyprice, sellPrice, sprite);
                ItemList.Add(id, consumable);
            }
        }
    }
    public Dictionary<int, Item> GetItemList() {
        return ItemList;
    }
    public Item GetItemByID(int id) {//这里设置成背包取ID
        Item item=null;
        if (ItemList.TryGetValue(id,out item)) {
        }
        return item;
    }
    #endregion
}


