using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using static QuizData;

public class LoadQuestion : MonoBehaviour
{
    // Loading a Quiz question 

    public Text ques;
    
    public void LoadfromJSON()
    {
        string json = File.ReadAllText(Application.dataPath + "/quiz_que.json");
        Option data = JsonConvert.DeserializeObject<Option>(json);
        //string jsonString = JsonConvert.SerializeObject(data.no, Formatting.Indented);
        ques.text = data.no;
    }

}
