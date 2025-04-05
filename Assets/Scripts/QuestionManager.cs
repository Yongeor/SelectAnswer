using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Crosstales.RTVoice.Tool;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


/// <summary>
/// 题目
/// </summary>
[Serializable]
public class QuestionData
{
    public string topic;//题目
    public string[] options;//选项
    public string correctAnswer;//正确选项
}

/// <summary>
/// 试卷
/// </summary>
[Serializable]
public class QuestionDataList
{
    public QuestionData[] questions;//题目列表
}


public class QuestionManager : MonoBehaviour
{
    //将第一题题目内容和选择内容和传正确选项给AnswerContentPanel，管理下一题等
    //倒计时

    public SpeechText SpeechText;
    

    public Button submitBtn;//提交
    public Button nextQuestionBtn;//下一题
    public Text resultText;//显示结果
    public Text countdownText;//倒计时

    public Text progressText;//显示进度
    public Slider progressSlider;//题目进度条


    
    public AnswerContentPanel answerContentPanel;  // 引用答题面板的脚本

    private QuestionDataList questionDataList;     // 存储解析后的题目数据
    private QuestionData currentQuestion;//当前题目
    private int currentQuestionIndex = 0;          // 当前题目索引
    private string correctAnswer;                  // 当前题目的正确答案
    private string selectedAnswer;                 // 用户选择的答案
    private float timeLimit = 30f;                 // 每道题的时间限制（秒）
    private Coroutine countdownCoroutine;          // 用于存储倒计时协程
    private int score = 0;                         // 记录答对的题目数量

    private async Task Start()
    {
        submitBtn.onClick.AddListener(OnSubmit);
        nextQuestionBtn.onClick.AddListener(()=>
        {
            LoadNextQuestion();
        });
        
        nextQuestionBtn.gameObject.SetActive(false);
        submitBtn.interactable = false; //初始化禁用提交按钮，直到用户选择了选项


        LoadQuestionsFromJson();
        
        
        // LoadNextQuestion(true);
        //
        // SpeakSpeechTextTask(currentQuestion.topic);
    }

    
    //从本地Json文件加载题目
    void LoadQuestionsFromJson()
    {
        //Resources
        // TextAsset jsonFile = Resources.Load<TextAsset>("questions");
        //StreamingAssets
        
        // var jsonFile= File.ReadAllText(Application.streamingAssetsPath + "/questions.json");
        // if (jsonFile != null)
        // {
        //     questionDataList = JsonUtility.FromJson<QuestionDataList>(jsonFile);
        //     progressSlider.value = 0;
        //     progressSlider.maxValue = questionDataList.questions.Length;
        // }
        // else
        // {
        //     Debug.LogError("无法找到 JSON 文件");
        // }

        
        
        StartCoroutine(GetQuestions());
    }
    
    // 协程获取问题数据
    private IEnumerator GetQuestions()
    {
        //TODO:数据填好了
        // UnityWebRequest.Post("https://localhost:7047/api/PostQuestion",
        //     "{\n  \"questions\": [\n    {\n      \"topic\": \"下列哪个是太阳系的行星？\",\n      \"options\": [\"太阳\", \"木星\", \"月亮\", \"彗星\"],\n      \"correctAnswer\": \"木星\"\n    },\n    {\n      \"topic\": \"地球的卫星叫什么？\",\n      \"options\": [\"月球\", \"木星\", \"火星\", \"彗星\"],\n      \"correctAnswer\": \"月球\"\n    }\n  ]\n}");
        
        using (UnityWebRequest request = UnityWebRequest.Get("https://localhost:7047/api/Question"))
        {
            // 等待响应
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 成功响应，解析JSON数据
                string jsonResponse = request.downloadHandler.text;
                
                questionDataList = JsonUtility.FromJson<QuestionDataList>(jsonResponse);
                progressSlider.value = 0;
                progressSlider.maxValue = questionDataList.questions.Length;
                
                LoadNextQuestion(true);

                SpeakSpeechTextTask(currentQuestion.topic);

                // 打印获取到的问题数据
                foreach (var question in questionDataList.questions)
                {
                    Debug.Log($"Topic: {question.topic}");
                    Debug.Log($"Options: {string.Join(", ", question.options)}");
                    Debug.Log($"Correct Answer: {question.correctAnswer}");
                }
            }
            else
            {
                // 请求失败，输出错误信息
                Debug.LogError($"Request failed: {request.error}");
            }
        }
    }
    
    
    //下一题按钮
    public void LoadNextQuestion(bool isFirst = false)
    {
        if (questionDataList != null && currentQuestionIndex + 1 <= questionDataList.questions.Length)
        {
            currentQuestionIndex++;
            currentQuestion = questionDataList.questions[currentQuestionIndex - 1];

            if (!isFirst)
            {
                SpeakSpeechTextTask(currentQuestion.topic);
            }

            correctAnswer = currentQuestion.correctAnswer;
            answerContentPanel.SetAnswerContentPanel(currentQuestion.topic, currentQuestion.options, correctAnswer);

            // 重置按钮和结果文本
            resultText.text = "";
            submitBtn.gameObject.SetActive(true);
            submitBtn.interactable = false; // 禁用提交按钮直到用户选择答案
            nextQuestionBtn.gameObject.SetActive(false);

            // 重置倒计时
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            countdownCoroutine = StartCoroutine(StartCountdown(timeLimit));

            // 显示题目进度，例如"第1/5题"
            progressText.text = $"第{currentQuestionIndex}/{questionDataList.questions.Length}题";
            progressSlider.value = currentQuestionIndex;
        }
        else
        {
            // 题目完成处理
            Debug.Log("所有题目已完成！");
            resultText.text = "恭喜你，所有题目已完成！你的得分是：" + score + " / " + questionDataList.questions.Length;
            nextQuestionBtn.gameObject.SetActive(false);
            submitBtn.gameObject.SetActive(false);

            // 这里可以跳转到一个总结界面，或者提供一个重新开始的选项
        }
    }


    //提交按钮
    //调用用户的选择，去对比考卷中的正确答案
    private void OnSubmit()
    {
        selectedAnswer = answerContentPanel.GetSelectedAnswer();  // 通过 answerPanel 获取用户选择的选项

        if (string.IsNullOrEmpty(selectedAnswer))
        {
            resultText.text = "请选择一个选项";
            return;
        }

        // 停止倒计时协程
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        if (selectedAnswer == correctAnswer)
        {
            resultText.text = "回答正确！";
            score++;  // 答对加分
        }
        else
        {
            resultText.text = "回答错误，正确答案是：" + correctAnswer+"";
        }

        answerContentPanel.SetTogglesInteractable(false);  // 禁用选项
        submitBtn.gameObject.SetActive(false);
        nextQuestionBtn.gameObject.SetActive(true);
    }
    
    private IEnumerator StartCountdown(float timeLimit)
    {
        float remainingTime = timeLimit;
        countdownText.color = Color.black;  // 重置倒计时颜色
        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString("F0");

            // 如果剩余时间少于 5 秒，倒计时文本变为红色
            if (remainingTime <= 5)
            {
                countdownText.color = Color.red;
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        countdownText.text = "0";
        resultText.text = "回答超时！";

        answerContentPanel.SetTogglesInteractable(false);  // 禁用选项
        submitBtn.gameObject.SetActive(false);
        nextQuestionBtn.gameObject.SetActive(true);
    }
    

    private void Awake()
    {
        //注册播放语音结束事件
        SpeechText.OnSpeechTextComplete += OnSpeechTextCompleted;
    }
    
    private void OnSpeechTextCompleted()
    {
        if (SpeakSpeechCancellationTokenSource?.IsCancellationRequested == false)
            SpeakSpeechCancellationTokenSource.Cancel();
    }
    
    /// <summary>
    /// 语音播报转化非法字符
    /// </summary>
    private string[] specialCharacters = { "&" };
    public CancellationTokenSource SpeakSpeechCancellationTokenSource;
    public async Task SpeakSpeechTextTask(string speechText)
    {
        //SpeakSpeechCancellationTokenSource != null && !SpeakSpeechCancellationTokenSource.IsCancellationRequested
        if (SpeakSpeechCancellationTokenSource?.IsCancellationRequested == false)
        {
            //连续播放时，同一时帧，还没开始Speak，所以顺序播放动作会顺序播报不会重叠也不会断上一个语音
            SpeechText.Silence();

            while (SpeakSpeechCancellationTokenSource != null)
                await Task.Delay(10);
            // await UniTask.WaitUntil(() => SpeakSpeechCancellationTokenSource == null);
        }
        
        if (string.IsNullOrEmpty(speechText))
            return;
        
        SpeakSpeechCancellationTokenSource = new CancellationTokenSource();

        var speakText = speechText.Trim();
        foreach (string character in specialCharacters)
        {
            if (speakText.Contains(character))
                speakText = speakText.Replace(character, "");
        }
        SpeechText.Text = speakText;
        SpeechText.Speak();
        
        // 一直等待直到被取消
        // await UniTask.WaitUntilCanceled(SpeakSpeechCancellationTokenSource.Token);
        while (!SpeakSpeechCancellationTokenSource.Token.IsCancellationRequested)
            await Task.Delay(10);
        SpeakSpeechCancellationTokenSource.Dispose();
        SpeakSpeechCancellationTokenSource = null;
    }
}
