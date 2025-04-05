using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerContentPanel : MonoBehaviour
{
    public Text topicText;
    public Toggle[] selectToggles;
    public Text[] selectLabels;

    public QuestionManager questionManager;

    // private void Awake()
    // {
    //     for (int i = 0; i < selectLabels.Length; i++)
    //     {
    //         selectToggles[i].onValueChanged.AddListener((value) =>
    //         {
    //             OnOptionSelected();
    //         });
    //     }
    // }

    //比如设置第一题，第二题
    public void SetAnswerContentPanel(string topic, string[] selectContent, string correctAns)
    {
        topicText.text = topic;
        for (int i = 0; i < selectLabels.Length; i++)
        {
            selectLabels[i].text = selectContent[i];
            selectToggles[i].isOn = false;
            selectToggles[i].interactable = true;

            //移除上一个监听
            selectToggles[i].onValueChanged.RemoveAllListeners();
            //设置选项改变的监听
            selectToggles[i].onValueChanged.AddListener((value) =>
            {
                OnOptionSelected();
            });
        }
    }

    //当用户选择一个选项时，启动提交按钮
    private void OnOptionSelected()
    {
        bool anySelected = false;
        foreach (var toggle in selectToggles)
        {
            if (toggle.isOn)
            {
                anySelected = true;
                break;
            }
        }
        //启用或者禁用提交按钮
        questionManager.submitBtn.interactable = anySelected;
    }
    
    
    
    // 设置选项按钮的可交互性
    public void SetTogglesInteractable(bool state)
    {
        foreach (Toggle toggle in selectToggles)
        {
            toggle.interactable = state;
        }
    }
    
    
    //获取用户选择的答案
    public string GetSelectedAnswer()
    {
        for (int i = 0; i < selectToggles.Length; i++)
        {
            if (selectToggles[i].isOn)
            {
                return selectLabels[i].text;
            }
        }
        return null;
    }
}