using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupMenu : BasePopup
{
    private void OnEnable()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTNCLICK);
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

    public void OnClickPopupSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }
    public void OnClickedResumeButton()
    {
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
    }
    
    public void OnClickedQuitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}
