using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizData : MonoBehaviour
{
    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Option
    {
        public string no { get; set; }
        public string OptN { get; set; }
    }

    public class Options
    {
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
        public Options Options { get; set; }
        public QuizQs QuizQs { get; set; }
    }
}
