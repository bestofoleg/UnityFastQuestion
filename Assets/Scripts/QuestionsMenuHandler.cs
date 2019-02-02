using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsMenuHandler : MonoBehaviour
{
    
    public Animator UIAnimator;

    public Text MessagesTextUIPrefab;
    
    public GameObject QuestionPanel;

    public Text QuestionText;
    
    public InputField AnswerTextBox;

    public GameObject GetQuestionButton;

    public GameObject AskToQuestionButton;
    
    public Text MyQuestionText;
    
    public Text TextFromMyQuestionTextBox;
    
    public InputField MyQuestionTextBox;
    
    public GameObject SendMyQuestionButton;
    
    
    
    private IService <Question> MyQuestionService;
    
    private Question CurrentQuestion;
    
    private Question MyCurrentQuestion;
    
    private Dictionary<string, Language> LanguagesDictionary;

    private void Awake()
    {
        MyQuestionService = new QuestionService();
        
        LanguagesDictionary = new Dictionary<string, Language>();
        
        LanguagesDictionary.Add("russian", new Language(4, "russian"));
        LanguagesDictionary.Add("english", new Language(5, "english"));
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
        
                    //!!!UpdateMessages();
                        
                    UIAnimator.SetBool("IsQuestionSlide", true);
                    UIAnimator.SetBool("IsMessageSlide", true);
                    UIAnimator.SetBool("IsMessageFullScreen", false);
                        
                    //!!!OtherUsersAnswersScrollView.SetActive(true);
                    //!!!MyQuestionTextBox.gameObject.SetActive(false);
                        
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
                    //!!!UpdateMessagesButton.SetActive(true);
                    SendMyQuestionButton.SetActive(false);  
        
                    MyQuestionText.text = TextFromMyQuestionTextBox.text;
                    TextFromMyQuestionTextBox.text = "";
        
                    //!!!UpdateMessagesFromServer();

                    //!!!AnswersCount = 0;

                }
            )
        );
        
    }
    
}