using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class LoadQuestion : MonoBehaviour
{
    // Loading a Quiz question 

    public Text ques;

    
    public void LoadfromJSON()
    {
        string json = File.ReadAllText(Application.datapath + "/quiz_que.json");
        QuizData data = JsonUtility.FromJson<QuizData>(json);
        ques.text = data.ques;
                
    }

}
