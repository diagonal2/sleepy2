using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Task3 : Story {

    private string task4_folderString;
    private FolderA task4_folder;

    // Start is called before the first frame update
    void Start() {

        /* taskStage = 300;

        folder = (FolderA) home.FindItem("/a/d/b/a/n/e/");
        folder.AddChild("c3.txt", "Cannot access content.");
        file = (TextFile) folder.Children["c3.txt"];

        task4_folderString = RandomTaskString(6, out task4_folder);
        forbidden.Add(task4_folderString);

        File.WriteAllText("C:/Users/user0/Unity/sleepy2/Testdata/a/d/b/a/n/e/c3.txt.push", "@activateRenameButton\n" + renameButtonPassword + "\n\n@content\n[Log]\n\nData leakage detected.\n\n" + task4_folderString + "\n\nMessage recovered.\n\nRenaming items may lead to unexpected errors in the future due to outdated references. Rename with care.\n");
        // File.WriteAllText("C:/Users/user0/Unity/sleepy2/Testdata/a/d/b/a/n/e/c3.txt.push", "@content\nCannot access content.\n");

        story.deleteButton.SetActive(true);

        initialised = true; */

    }

    // Update is called once per frame
    void Update() {     // open c3.txt with correct commands
        
        if (taskStage >= 300 && taskStage < 400) {

            if (!initialised) {

                folder.AddChild("c3.txt", "Cannot access content.");
                CurrentDirectory.currentDirectory.UpdateDirectory(folder);
                file = (TextFile) folder.Children["c3.txt"];

                task4_folderString = RandomTaskString(6, out task4_folder);     // no need to create directory as it is created in Task1
                forbidden.Add(task4_folderString);

                File.WriteAllText(dataPath + folder.Path.Substring(28) + "c3.txt", "@activateRenameButton\n" + renameButtonPassword + "\n\n@content\n[Log]\n\nData leakage detected.\n\n" + task4_folderString + "\n\nMessage recovered.\n\nRenaming items may lead to unexpected errors in the future due to outdated references. Rename with care.\n");
                File.WriteAllText(dataPath + folder.Path.Substring(28) + "c3.txt.push", "@content\nCannot access content.\n");

                initialised = true;

            }

            if (CurrentWindow.currentWindow.File == file) {

                if (story.renameButton.activeSelf == true) {
                    PassParameters(3, 4, task4_folderString, task4_folder, null);
                    taskStage = 400;
                }

            }

        }

    }

}
