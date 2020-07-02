using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Task4 : Story {    // push content to c4.txt

    private string newFolderString, newFolderPath = "";
    private bool flag490 = false;

    // Start is called before the first frame update
    void Start() {

        /* taskStage = 400;

        newFolderString = "adbake";
        newFolderPath = "/a/d/b/a/k/e/";

        folder = (FolderA) home.FindItem("/a/d/b/a/n/e/");
        folder.readable = false;
        folder.cantReadMessage = "Path mismatch error.\nExpected path: " + newFolderPath;
        folder.AddChild("c4.txt", "Cannot access content.");
        file = (TextFile) folder.Children["c4.txt"];

        story.renameButton.SetActive(true);
        story.deleteButton.SetActive(true);

        initialised = true; */

    }

    // Update is called once per frame
    void Update() {
        
        if (taskStage >= 400 && taskStage < 500) {

            if (!initialised) {

                newFolderString = RandomSubstitute(folderString, out int replaceeIndex);
                for (int i = 0; i < 6; i++) newFolderPath += "/" + newFolderString[i];
                newFolderPath += "/";
                Debug.Log(newFolderPath);

                folder.readable = false;
                folder.cantReadMessage = "Path mismatch error.\nExpected path: " + newFolderPath;
                folder.AddChild("c4.txt", "Cannot access content.");
                file = (TextFile) folder.Children["c4.txt"];

                // PassParameters(4, 11, "", null, folderString[replaceeIndex]);

                initialised = true;

            }

            if (home.FindItem(newFolderPath + "c4.txt") != null && !flag490) {

                folderString = newFolderString;
                folder.cantReadMessage = "Password required.";

                Directory.CreateDirectory(dataPath + folder.Path.Substring(28));
                File.WriteAllText(dataPath + folder.Path.Substring(28) + "c4.txt", "@activateNewButton\n" + newButtonPassword + "\n\n@content\n[Log]\n\nMessage recovered.\n\nThere is currently no route of opening this file which you have permission to use. Ask for permission from the administrator.\n");
                File.WriteAllText(dataPath + folder.Path.Substring(28) + "c4.txt.push", "@content\nCannot access content.\n");

                PassParameters(4, 5, "adminx", null, newFolderString);
                taskStage = 490;
                flag490 = true;

            }   // taskStage = 500 is set in Story

        }

    }

}
