using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    public List<Question> questionDB;
    public static DatabaseManager dbmInstance;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void Start()
    {
        dbmInstance = this;
    }

    void Update()
    {
        
    }

    public bool hasFile()
    {
        return (questionDB.Any());
    }

    public void ReadCSVFile(string filePath)
    {
        List<string> rawLines = new List<string>();
        List<Question> processedLines = new List<Question>();
        try
        {
            // Read all lines from the CSV file
            string[] fileLines = File.ReadAllLines(filePath);

            // Add each line to the list
            rawLines.AddRange(fileLines);
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading CSV file: {e.Message}");
        }

        // convert the lines into a list of questions, then add it to the question list
        foreach (string l in rawLines) {
            processedLines.Add(ParseQuestionString(l));
        }

        FillDatabase(processedLines);
    }

    public static Question ParseQuestionString(string questionString)
    {
        string[] parts = questionString.Split(',');

        if (parts.Length != 3)
        {
            // Handle invalid input or return null, depending on your requirements
            Debug.LogError("Invalid question string format");
        }

        string type = parts[0].Trim();
        string questionText = parts[1].Trim();
        string answer = parts[2].Trim();

        Question question = new Question(type, questionText, answer);
        // question.nonAnswers.Add(questionText); // Assuming the question itself is included in non-answers for non-multiple choice types

        return question;
    }

    void FillDatabase(List<Question> questions) 
    {
        questionDB = questions;

        // generate non answers for mc questions
        foreach (Question q in questionDB) {
            if (q.type.Equals("multiple_choice"))
            {
                q.GenerateNonAnswers();
            }
        }
        // TODO: REMOVE
        //PrintQuestions();
    }

    void PrintQuestions() 
    {
        foreach (Question q in questionDB) 
        {
            string s = q.toString();
            Debug.LogError(s);
        }
    }
  
}
