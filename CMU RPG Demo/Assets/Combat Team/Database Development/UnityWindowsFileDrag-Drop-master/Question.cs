using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Question
{
    public string type; // "m_c", "t_f"
    public string questionText;
    public string answer; // Correct answer, can be a string or "T/F"
    public List<string> nonAnswers; // For multiple choice questions, randomly picked from other questions
    public string[] questions; // For multiple choice questions, randomly picked from other questions

    // Constructor to initialize the question object
    public Question(string type, string questionText, string answer)
    {
        this.type = type;
        this.questionText = questionText;
        this.answer = answer;
        this.nonAnswers = new List<string>();
        //Question variable
        this.questions = new string[4];
    }

    public void GenerateNonAnswers()
    {
        List<Question> allQuestions = DatabaseManager.dbmInstance.questionDB;

        // Randomly pick non-answers from other multiple choice questions
        foreach (Question q in allQuestions)
        {
            if (q.type.Equals("multiple_choice", StringComparison.OrdinalIgnoreCase) && q != this)
            {
                nonAnswers.Add(q.answer);
            }

            // Break the loop when enough non-answers are selected (you can adjust this based on your requirements)
            if (nonAnswers.Count >= 3)
            {
                break;
            }
        }

        // Shuffle the non-answers to randomize their order
        //ShuffleNonAnswers();
        //Add GetPossibleAnswers() here, and remove ShuffleNonAnswers().
        this.questions = GetPossibleAnswers();
    }

    public string[] GetPossibleAnswers()
    {
        List<string> allAnswers = nonAnswers.Concat(new List<string> { answer }).ToList();

        System.Random rng = new System.Random();
        int n = allAnswers.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = allAnswers[k];
            allAnswers[k] = allAnswers[n];
            allAnswers[n] = value;
        }

        return allAnswers.Take(4).ToArray();
    }

    // Helper method to shuffle the non-answers
    private void ShuffleNonAnswers()
    {
        System.Random rng = new System.Random();
        int n = nonAnswers.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = nonAnswers[k];
            nonAnswers[k] = nonAnswers[n];
            nonAnswers[n] = value;
        }
    }

    public string toString() {
        if (this.type == "multiple_choice") {
            string list = "Type: " + type + ", Answer: " + answer + " Non-answers: ";
            
            foreach (string s in nonAnswers) {
                list += (s + ", ");
            }
        }

        return "Type: " + type + ", Answer: " + answer;
    }
}