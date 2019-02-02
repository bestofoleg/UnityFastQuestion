using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class MainMenuHandlerScript : MonoBehaviour
{
    
    //Prefabs
    public Animator UIAnimator;

    public Text MessagesTextUIPrefab;
    
    //Question
    public GameObject QuestionPanel;

    public Text QuestionText;
    
    public InputField AnswerTextBox;

    public GameObject GetQuestionButton;

    public GameObject AskToQuestionButton;
    
    //Message
    public GameObject MessagesPanel;

    public InputField MyQuestionTextBox;

    public GameObject SendMyQuestionButton;

    public GameObject OtherQuestionsButton;

    public Text MyQuestionText;
    
    public Text TextFromMyQuestionTextBox;

    public GameObject UpdateMessagesButton;

    public GameObject OtherUsersAnswersScrollView;
    
    
    private const int MAX_ANSWERS_COUNT = 3; 
    
    private int AnswersCount;

    private IService <Question> MyQuestionService;

    private MessageService MyMessageService;

    private Question CurrentQuestion;

    private Question MyCurrentQuestion;

    private Dictionary<string, Language> LanguagesDictionary;
    
    private void Awake()
    {
        
        LanguagesDictionary = new Dictionary<string, Language>();
        
        LanguagesDictionary.Add("russian", new Language(4, "russian"));
        LanguagesDictionary.Add("english", new Language(5, "english"));
        
        MyQuestionService = new QuestionService();
        MyMessageService = new MessageService();
        AnswersCount = 0;
    }

    public void GetQuestion()
    {
        
        StartCoroutine(MyQuestionService.GetRandom(
                delegate(Question responseQuestion)
                    {
                        
                        QuestionText.text = responseQuestion.question;
                        CurrentQuestion = responseQuestion;
                        
                        QuestionText.gameObject.SetActive(true);
                        AnswerTextBox.gameObject.SetActive(true);
                        GetQuestionButton.SetActive(false);
                        AskToQuestionButton.SetActive(true);
        
                        UpdateMessages();
                        
                        UIAnimator.SetBool("IsQuestionSlide", true);
                        UIAnimator.SetBool("IsMessageSlide", true);
                        UIAnimator.SetBool("IsMessageFullScreen", false);
                        
                        OtherUsersAnswersScrollView.SetActive(true);
                        MyQuestionTextBox.gameObject.SetActive(false);
                        
                    }
                )
        );
        
    }

    private void RemoveMessagesFromPanel()
    {
        
        for (int i = 0; i < MessagesPanel.transform.childCount; i++)
        {
            GameObject.Destroy(MessagesPanel.transform.GetChild(i).gameObject);
        }
        
    }

    private void RemoveAndShowNewMessages(List<Message> messages)
    {
        RemoveMessagesFromPanel();
        
        foreach (var message in messages)
        {
            
            Text t = Instantiate(
                MessagesTextUIPrefab,
                Vector3.zero,
                Quaternion.identity
            );
            
            t.text = message.message;
            t.gameObject.transform.SetParent(MessagesPanel.transform);
            t.rectTransform.localScale = Vector3.one;
                    
        }
    }

    private void UpdateMessages()
    {
        RemoveMessagesFromPanel();

        StartCoroutine(MyMessageService.GetMessagesByQuestionId(
                CurrentQuestion.id, 
                delegate(List <Message> messages)
                {
                    
                    RemoveAndShowNewMessages(messages);
                    
                }
            )
        );
       
    }

    public void SendAnswerToQuestion()
    {
        
        Message message = new Message();

        message.message = AnswerTextBox.text;
        message.question_id = CurrentQuestion;

        StartCoroutine(MyMessageService.Add(
                message,
                delegate(Message m)
                {
                    AnswerTextBox.text = "";
        
                    if (AnswersCount < MAX_ANSWERS_COUNT-1)
                    {
                        UpdateMessages();
            
                        UIAnimator.SetBool("IsQuestionSlide", false);
                        AskToQuestionButton.SetActive(false);
                        GetQuestionButton.SetActive(true);
            
                        AnswersCount ++;
                    }
                    else
                    {
                        UIAnimator.SetBool("IsQuestionSlide", false);
            
                        RemoveMessagesFromPanel();
            
                        MyQuestionTextBox.gameObject.SetActive(true);
                        SendMyQuestionButton.SetActive(true);
                        OtherQuestionsButton.SetActive(false);
                        AskToQuestionButton.SetActive(false);
                        GetQuestionButton.SetActive(true);
                        OtherUsersAnswersScrollView.SetActive(false);
                    }
                }
            )
        );

    }

    public void SendMyQuestion()
    {

        MyCurrentQuestion = new Question();

        MyCurrentQuestion.question = MyQuestionTextBox.text;

        MyCurrentQuestion.language_id = LanguagesDictionary["russian"];
        
        StartCoroutine(MyQuestionService.Add(
                MyCurrentQuestion, 
                delegate(Question question)
                {
                    MyCurrentQuestion.id = question.id;
                    
                    UIAnimator.SetBool("IsMessageFullScreen", true);
                    UIAnimator.SetBool("IsMessageSlide", false);
        
                    MyQuestionTextBox.gameObject.SetActive(false);
                    MyQuestionText.gameObject.SetActive(true);
                    UpdateMessagesButton.SetActive(true);
                    SendMyQuestionButton.SetActive(false);  
        
                    MyQuestionText.text = TextFromMyQuestionTextBox.text;
                    TextFromMyQuestionTextBox.text = "";
        
                    UpdateMessagesFromServer();

                    AnswersCount = 0;

                }
            )
        );
        
    }

    public void UpdateMessagesFromServer()
    {
        
        StartCoroutine(MyMessageService.GetMessagesByQuestionId(
                MyCurrentQuestion.id, 
                delegate(List<Message> messages)
                {
                    OtherUsersAnswersScrollView.SetActive(true);
                    
                    RemoveAndShowNewMessages(messages);
                }
            )
        );
        
    }

}
