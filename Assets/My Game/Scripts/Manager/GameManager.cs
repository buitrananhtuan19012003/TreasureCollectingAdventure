using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;
    private void Start()
    {
        isPlaying = true;
        Time.timeScale = 1f;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                scr.DoAnimationLoadingProgress(5, () =>
                {
                    UIManager.Instance.ShowScreen<ScreenLogin>();
                    scr.Hide();
                });
            }
        }
    }

    public void PauseGame()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void RestarGame()
    {
        ChangeScene("Menu");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupLose>();
        }
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
