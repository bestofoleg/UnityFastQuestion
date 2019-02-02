using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MessageService : IService <Message>
{

    public MessageService()
    {}

    public IEnumerator Add(Message message, Action<Message> callback)
    {
        
        string JsonMessageString = JsonConvert.SerializeObject(message);
    
        var www = new UnityWebRequest(ReferensesNames.ROOT_URL + "/addMessage", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonMessageString);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            callback(message);
        }
        else
        {
            Debug.LogWarning(
                "Server has response an error from URL = "+ReferensesNames.ROOT_URL+"/addMessage. " + www.error
            );
        }

    }

    public IEnumerator GetMessagesByQuestionId(long id, System.Action <List <Message>> callback)
    {

        LongJSON longJson = new LongJSON();
        longJson.value = id;
        string LongValueJSONString = JsonConvert.SerializeObject(longJson);
        
        Dictionary<string,string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        byte[] binaryData = System.Text.Encoding.UTF8.GetBytes(LongValueJSONString.ToCharArray());
        
        WWW www = null;

        yield return www = new WWW(ReferensesNames.ROOT_URL + "/getMessageByQuestion", binaryData, headers);

        if (www.error == null)
        {
            callback(JsonConvert.DeserializeObject<List<Message>>(www.text));
        }
        else
        {
            Debug.LogWarning(
                "Server has response an error from URL = "+ReferensesNames.ROOT_URL+"/getMessageByQuestion. " + www.error
            );
        }

    }

    public IEnumerator GetRandom(Action<Message> callback)
    {
        throw new NotImplementedException();
    }

}