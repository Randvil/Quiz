using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuizQuestion
{
    public string question;
    public List<Answer> answers;
    public string background;

    public override string ToString()
    {
        string answersString = string.Empty;

        foreach (Answer answer in answers)
        {
            answersString += $"{answer}; ";
        }

        return $"Question: {question}. Answers: {answersString}";
    }
}
