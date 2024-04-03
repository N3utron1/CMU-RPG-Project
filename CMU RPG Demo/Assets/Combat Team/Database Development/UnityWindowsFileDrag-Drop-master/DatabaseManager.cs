using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public List<Question> questionDB;
    public static DatabaseManager dbmInstance;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
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

        // fill database
        questionDB = processedLines;

        // TEST
        PrintQuestions();
    }

    public static Question ParseQuestionString(string questionString)
    {
        string[] parts = questionString.Split(',');

        if (parts.Length != 2)
        {
            // Handle invalid input or return null, depending on your requirements
            Debug.LogError("Invalid question string format");
        }

        string questionText = parts[0].Trim();
        string answer = parts[1].Trim();

        Question question = new Question(questionText, answer);
        // question.nonAnswers.Add(questionText); // Assuming the question itself is included in non-answers for non-multiple choice types

        return question;
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
