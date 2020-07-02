using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class Task0 : Story {        // open c0.txt

    private string task1_folderString;
    private FolderA task1_folder;
    private readonly string task1_password = RandomString(8);

    // Start is called before the first frame update
    void Start() {
        
        folderString = RandomTaskString(6, out folder);
        forbidden.Add(folderString);
        Debug.Log(folderString);

        task1_folderString = RandomTaskString(6, out task1_folder);
        forbidden.Add(task1_folderString);
        Debug.Log(task1_folderString + " " + task1_password);

        folder.AddChild("c0.txt", "[log]\n\nData leakage detected.\n\n" + task1_folderString + "\n" + task1_password);
        file = (TextFile) folder.Children["c0.txt"];

        initialised = true;

    }

    // Update is called once per frame
    void Update() {

        if (taskStage < 100)

            if (CurrentWindow.currentWindow.File == file) {

                Directory.CreateDirectory(dataPath + folder.Path.Substring(28));
                File.WriteAllText(dataPath + file.Path.Substring(28), "Cannot access content.");

                PassParameters(0, 1, task1_folderString, task1_folder, task1_password);
                taskStage = 100;

            }

    }

}
