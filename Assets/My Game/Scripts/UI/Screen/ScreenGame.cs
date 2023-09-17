using LupinrangerMovementSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenGame : BaseScreen
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private TextMeshProUGUI ammoText;
    private float timeRemaining;
    private bool timerIsRunning = false;
    
    public MenuInputActions InputActions;

    private void Awake()
    {
        SetTimeRemain(120);
    }
    public void OnClickPopupSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                if (UIManager.HasInstance && GameManager.HasInstance && AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
                    GameManager.Instance.PauseGame();
                    UIManager.Instance.ShowPopup<PopupSetting>();
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SetTimeRemain(float v)
    {
        timeRemaining = v;
    }
    public override void Init()
    {
        base.Init();

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_AMMO, OnUpdateAmmo);
        }
    }

    private void OnDestroy()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_AMMO, OnUpdateAmmo);
        }
    }

    private void OnUpdateAmmo(object value)
    {
        if (value is int ammo)
        {
            ammoText.text = ammo.ToString();
        }
        
    }
}
