using System;

[Serializable]
public class Question
{

    public long id { get; set; }

    public string question { get; set; }

    public Language language_id { get; set; }

    public Question()
    {}
    
    public override string ToString()
    {
        return "Question = {id = "+id+", question = "+question+", lang = "+language_id+"}";
    }
    
}