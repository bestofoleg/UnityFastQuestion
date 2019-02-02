using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class QuestionService : IService <Question>
{
    
    public QuestionService()
    {}

    public IEnumerator GetRandom(Action<Question> callback)
    {

        WWW www = null;

        yield return www = new WWW(ReferensesNames.ROOT_URL + "/getRandomQuestion");
        
        if (www.error == null)
        {
            callback(JsonConvert.DeserializeObject<Question>(www.text));
        }
        else
        {
            Debug.LogWarning(
                    "Server has response an error from URL = "+ReferensesNames.ROOT_URL+"/getRandomQuestion. " + www.error
                );
        }

    }
    
    public IEnumerator Add(Question question, System.Action <Question> callback)
    {

        List <Question> questions = new List<Question>();
        questions.Add(question);
        string QuestionsJSONString = JsonConvert.SerializeObject(questions);
        
        Dictionary<string,string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        byte[] binaryData = System.Text.Encoding.UTF8.GetBytes(QuestionsJSONString.ToCharArray());
        
        WWW www = null;

        yield return www = new WWW(ReferensesNames.ROOT_URL + "/addQuestion", binaryData, headers);


        if (www.error == null)
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                callback(JsonConvert.DeserializeObject<Question>(www.text));    
            }
        }
        else
        {    
            Debug.LogWarning(
                "Server has response an error from URL = "+ReferensesNames.ROOT_URL+"/addQuestion. " + www.error
            );
        }
        
        
    }
    
}