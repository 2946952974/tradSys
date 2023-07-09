/****************************************************
    文件：ItemTip.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/16 12:10:6
	功能：物品提示UI
*****************************************************/
using UnityEngine;
using UnityEngine.UI;

public class ItemTip : MonoBehaviour {
    public Text textitemtip;
    public Text textconten;
    public Image bg;
    public float TargetAlpha;
    public float smothing = 4;
    public CanvasGroup CanvasGroup;
    private void Start() {
        TargetAlpha = 0;
    }
    private void Update() {
        if (CanvasGroup.alpha != TargetAlpha) {
            //Debug.Log(CanvasGroup.alpha);
            //渐变//不会到达最终点
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, TargetAlpha, smothing * Time.deltaTime);
            if (Mathf.Abs(CanvasGroup.alpha - TargetAlpha) < 0.01)//差值到一定程度
             {
                CanvasGroup.alpha = TargetAlpha;
            }
        }
    }
    public void Show(string text) {
        textitemtip.text = text;
        textconten.text = text;
        TargetAlpha = 1;
    }
    public void Hide() {
        TargetAlpha = 0;
    }
    public void SetItemTipPos(Vector3 mousePos) {
        transform.localPosition = mousePos;
    }
}