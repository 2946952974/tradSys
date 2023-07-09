/****************************************************
    文件：DynamicWnd.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/12 14:41:26
	功能：提示UI界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DynamicWnd : MonoBehaviour {
    public static DynamicWnd Instance = null;
    public GameObject ToolTip;//提示UI的父物体
    public Text txtTip;//提示文本
    private bool isTipShow=false;
    private Queue<string> tipQue = new Queue<string>();
    public void InitWnd() {
        Instance = this;
    }
    private void Update() {
        if (tipQue.Count > 0&&isTipShow == false) {
            string tip = tipQue.Dequeue();
            isTipShow = true;
            SetTips(tip);
        }
    }
    public void AddTips(string tips) {
        tipQue.Enqueue(tips);
    }
    private void SetTips(string tip) {
        ToolTip.SetActive(true);//这是自动激活提示动画
        txtTip.text = tip;
        StartCoroutine(AniPlayDone(2.5f,()=> {
            ToolTip.SetActive(false);
            isTipShow = false;//这里就播放完毕了
        }));
    }
    private IEnumerator AniPlayDone(float delay,Action cb) {
        yield return new WaitForSeconds(delay);
        if (cb != null)
            cb();
    } 
}