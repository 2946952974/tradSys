/****************************************************
    文件：AudioSvc.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：2021/10/17 14:38:54
	功能：声音服务模块
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{
    public static AudioSvc Instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;
    public void InitSvs() {
        Instance = this;
        Debug.Log("Init AudioSvc.....");
    }
    public void PlayBGMusic(string name, bool isLoop = true) {
        AudioClip audio = ResSvc.Instance.LoadAudio("Auido/" + name, true);
        if (bgAudio.clip == null || bgAudio.clip.name != audio.name) {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }
    public void PlayUIAudio(string name) {
        AudioClip audio = ResSvc.Instance.LoadAudio("Auido/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}
public class Constant {
    #region 音效名字
    public const string UIClickBtn = "uiClickBtn";
    public const string UICloseBtn = "uiCloseBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UILoginBtn = "uiLoginBtn";
    public const string UIOpenPage = "uiOpenPage";
    #endregion
}
