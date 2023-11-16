using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Answer
{
    public string text;
    public bool correct;

    public override string ToString()
    {
        string correctly = correct ? "correctly" : "incorrectly";

        return $"{text} ({correctly})";
    }
}
