using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using System.IO;


public class FileDragAndDrop : MonoBehaviour
{
    public string filePath;
    List<string> log = new List<string>();
    void OnEnable ()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }
    
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }
    void OnFiles(List<string> aFiles, POINT aPos)
    {
        string file = "";
        // scan through dropped files and filter out non .csv files
        foreach(var f in aFiles)
        {
            var fi = new System.IO.FileInfo(f);
            var ext = fi.Extension.ToLower();
            if (ext == ".csv")
            {
                file = f;
                break;
            }
        }

        // If the user dropped a supported file, save its path
        if (file != "")
        {   
            filePath = file;
            DatabaseManager.dbmInstance.ReadCSVFile(file);
        }
        Debug.LogError("File recieved");

        // for testing
        // string[] csvLines = ReadCSVFile(filePath);

        // // Now 'csvLines' contains each line from the CSV file as a separate string
        // foreach (string line in csvLines)
        // {
        //     Debug.LogError(line);
        // }

        // check to see if .csv file is formatted correctly

            // if not, tell user to format it correctly

        // make the list of questions for enemies to pull from

        // tell the user the import was successful

    }

    // comment out this method to get rid of the UI in the build
    private void OnGUI()
    {
        string tmp = null;
        if (Event.current.type == EventType.Repaint && filePath!= null)
        {
            tmp = filePath;
            filePath = null;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Box("Drag .csv here", GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndHorizontal();
    }
}
