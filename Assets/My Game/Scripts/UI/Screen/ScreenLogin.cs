using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenLogin : BaseScreen
{
    public TMP_InputField username;
    public TMP_InputField password;
    private void OnEnable()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_BGM_1);
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void StartGame()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoadingGame>();
        }
        this.Hide();
    }
}
