using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Question
{
    public string questionText;
    public string answer;  // T or F

    public Question(string questionText, string answer)
    {
        this.questionText = questionText;
        this.answer = answer;
    }

    public string toString() {
        return questionText + ": " + answer;
    }
}
