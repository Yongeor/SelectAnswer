using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
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

    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        /// <summary>
        /// 获取所有试题数据
        /// </summary>
        /// <returns>返回QuestionDataList对象</returns>
        [HttpGet]
        public IActionResult Get()
        {
            //TODO：前端数据post给后端，后端记录到mysql，（或者自己去配置到mysql）
            //TODO：后端读取的是mysql
            // 模拟数据
            var questionDataList = new QuestionDataList
            {
                questions = new QuestionData[]
                {
                    new QuestionData
                    {
                        topic = "1 + 1 = ?",
                        options = new string[] { "1", "2", "3", "4" },
                        correctAnswer = "2"
                    },
                    new QuestionData
                    {
                        topic = "What is the capital of France?",
                        options = new string[] { "London", "Berlin", "Paris", "Madrid" },
                        correctAnswer = "Paris"
                    },
                    new QuestionData
                    {
                        topic = "太阳系?",
                        options = new string[] { "月亮", "太阳", "乐器", "火星" },
                        correctAnswer = "乐器"
                    },
                }
            };

            // 使用 JsonConvert 序列化为 JSON 字符串
            var json = JsonConvert.SerializeObject(questionDataList);

            // 返回 JSON 字符串
            return Content(json, "application/json");
        }
    }
}
