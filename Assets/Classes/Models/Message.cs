using System;

[Serializable]
public class Message
{

    public long id { get; set; }

    public Question question_id { get; set; }

    public string message { get; set; }

    public Message()
    {}

    public override string ToString()
    {
        return "Message = {id = "+id+", question_id = "+question_id+", message = "+message+"}";
    }
}