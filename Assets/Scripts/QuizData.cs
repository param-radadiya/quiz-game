
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class QuizData
{

    //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Option
    {
        public string no { get; set; }
        public string OptN { get; set; }
    }

    public class Options
    {
        internal string no;
        internal object pageList;

        public List<Option> Option { get; set; }
    }

    public class QuizQ
    {
        public string no { get; set; }
        public string que { get; set; }
        public string ans { get; set; }
    }

    public class QuizQs
    {
        public List<QuizQ> QuizQ { get; set; }
    }

    public class Root
    {
        public Options Option { get; set; }
        public QuizQs QuizQ { get; set; }
    }
}
