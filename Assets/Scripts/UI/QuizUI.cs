using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject questionObject;
    [SerializeField] private TextMeshProUGUI questionField;
    [SerializeField] private AnswersUI answersUI;
    [SerializeField] private TextMeshProUGUI isAnswerCorrectField;
    [SerializeField] private string correctAnswerText = "Correct";
    [SerializeField] private string incorrectAnswerText = "Incorrect";
    [SerializeField] private Button nextQuestionButton;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Button restartButton;

    private List<QuizQuestion> quizQuestions;
    private int currentQuestionNumber;
    private bool answerIsSelected;
    private int rightAnswerCounter;

    private void Awake()
    {
        questionObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
        isAnswerCorrectField.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        startGameButton.gameObject.SetActive(true);

        startGameButton.onClick.AddListener(StartQuiz);
        restartButton.onClick.AddListener(OnRestartButtonPress);
        nextQuestionButton.onClick.AddListener(MoveToNextQuestion);
        answersUI.AnswerSelectEvent.AddListener(OnAnswerSelect);
    }

    public void SetQuestions(List<QuizQuestion> quizQuestions)
    {
        if (quizQuestions.Count == 0)
        {
            Debug.LogError("There is no questions");
            BreakQuiz();
            return;
        }

        this.quizQuestions = quizQuestions;
    }

    public void StartQuiz()
    {
        currentQuestionNumber = 0;
        rightAnswerCounter = 0;

        startGameButton.gameObject.SetActive(false);
        questionObject.SetActive(true);
        ShowQuestion(currentQuestionNumber);
    }

    public void BreakQuiz()
    {
        questionObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
        isAnswerCorrectField.gameObject.SetActive(false);

        background.texture = null;

        answersUI.ClearAnswers();
    }

    public void EndQuiz()
    {
        BreakQuiz();
        ShowScore();

        restartButton.gameObject.SetActive(true);
    }

    private void ShowQuestion(int number)
    {
        if (number > quizQuestions.Count - 1)
        {
            EndQuiz();
            return;
        }

        QuizQuestion currentQuestion = quizQuestions[number];

        nextQuestionButton.gameObject.SetActive(false);
        isAnswerCorrectField.gameObject.SetActive(false);

        questionField.text = currentQuestion.question;

        string backgoundPath = currentQuestion.background;
        background.texture = Resources.Load(backgoundPath.Substring(0, backgoundPath.Length - 4)) as Texture;

        answersUI.CreateAnswers(currentQuestion.answers);
    }

    private void OnAnswerSelect(Answer answer)
    {
        if (answerIsSelected)
        {
            return;
        }

        nextQuestionButton.gameObject.SetActive(true);
        isAnswerCorrectField.gameObject.SetActive(true);
        
        if (answer.correct)
        {
            rightAnswerCounter++;
            isAnswerCorrectField.text = correctAnswerText;
        }
        else
        {
            isAnswerCorrectField.text = incorrectAnswerText;
        }

        answerIsSelected = true;
    }

    private void MoveToNextQuestion()
    {
        currentQuestionNumber++;
        answerIsSelected = false;

        ShowQuestion(currentQuestionNumber);
    }

    private void ShowScore()
    {
        score.gameObject.SetActive(true);
        score.text = $"{rightAnswerCounter} / {quizQuestions.Count}";
    }

    private void OnRestartButtonPress()
    {
        startGameButton.gameObject.SetActive(true);
        score.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }
}
