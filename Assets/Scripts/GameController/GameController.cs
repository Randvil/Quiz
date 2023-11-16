using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private QuizUI quizUIPrefab;
    [SerializeField] private string quizFileName;
    [SerializeField] private string firstFileKey = "\"question\"";

    private QuizUI quizUI;

    private void Awake()
    {
        char[] trimChars = { ' ', '\n', '\r', '\t', '[', ']', '{', '}', ',' };
        string[] splittedFiles = Resources.Load<TextAsset>(quizFileName).text.Trim(trimChars).Split(firstFileKey);
        List<string> jsonFiles = new();

        for (int i = 0; i < splittedFiles.Length; i++)
        {
            string file = splittedFiles[i];
            if (string.IsNullOrEmpty(file))
            {
                continue;
            }

            jsonFiles.Add("{" + firstFileKey + file.Trim(trimChars) + "}");
        }

        List<QuizQuestion> quizQuestions = new();

        foreach (string file in jsonFiles)
        {
            QuizQuestion question = JsonUtility.FromJson<QuizQuestion>(file);
            quizQuestions.Add(question);
        }

        quizUI = Instantiate(quizUIPrefab);

        quizUI.SetQuestions(quizQuestions);
    }
}
