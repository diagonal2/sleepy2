using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Task5 : Story {

    private string task4_newFolderString;

    private float timer582 = 0f;

    public Blackscreen blackscreen;
    public Button closeButton;

    // Start is called before the first frame update
    void Start() {

        /* taskStage = 500;

        newButtonPassword = "ow,zfE,S";
        editTextFilePassword = "aaaaaaaa";

        folder = (FolderA) home.FindItem("/a/d/m/i/n/x/");
        folder.AddChild("c5.txt", "[Log]\n\nMessage recovered.\n\n"
            + "In this world you're not supposed to edit text files; it breaks all sorts of things. Nevertheless one day this command just pops into existence. It is as if the System wants something to break outside the box.\n"
            + "There is only one place you can push this command into.\n@activateEditTextFile\n" + editTextFilePassword);
        file = (TextFile) folder.Children["c5.txt"];

        task4_newFolderString = "adbake";

        story.renameButton.SetActive(true);
        story.deleteButton.SetActive(true);

        initialised = true; */

        taskStage = 590;

        task4_newFolderString = "adbake";

        FolderA traversal = home;
        for (int i = 0; i < 6; i++) {
            traversal.AddChild(task4_newFolderString[i].ToString(), "");
            traversal = (FolderA) traversal.Children[task4_newFolderString[i].ToString()];
        }
        traversal.AddChild("c4.txt", "Cannot access content.");

        story.renameButton.SetActive(true);
        story.deleteButton.SetActive(true);
        story.inputField.readOnly = false;

        initialised = true;

    }

    // Update is called once per frame
    void Update() {
        
        if (taskStage >= 500 & taskStage < 600) {

            if (!initialised) {

                folder = (FolderA) home.FindItem("/a/d/m/i/n/x/");
                folder.AddChild("c5.txt", "[Log]\n\nMessage recovered.\n\n"
                    + "In this world you're not supposed to edit text files; it breaks all sorts of things. Nevertheless one day this command just pops into existence. It is as if the System wants something to break outside the box.\n"
                    + "There is only one place you can push this command into.\n@activateEditTextFile\n" + editTextFilePassword);
                file = (TextFile) folder.Children["c5.txt"];

                task4_newFolderString = (string) info[4];

                initialised = true;

            }

            if (taskStage == 582) {

                if (timer582 < 2f) timer582 += Time.deltaTime;

                else {

                    blackscreen.Activate();

                    string[] tmp = home.Children.Keys.ToArray<string>();
                    foreach (string s in tmp)
                        if (s != protagonist.Name) home.RemoveChild(s);
                    Initiate(home, 6);
                    FolderA traversal = home;
                    for (int i = 0; i < 6; i++) {
                        traversal.AddChild(task4_newFolderString[i].ToString(), "");
                        traversal = (FolderA) traversal.Children[task4_newFolderString[i].ToString()];
                    }
                    traversal.AddChild("c4.txt", "Cannot access content.");
                    CurrentDirectory.currentDirectory.UpdateDirectory(home);

                    story.inputField.readOnly = false;
                    closeButton.interactable = true;
                    closeButton.onClick.Invoke();
                    protagonist.text = File.ReadAllText(Application.dataPath + "/Images/Protagonist.txt");

                    taskStage = 590;

                }

            }

            if (taskStage >= 590) {

                if (story.newButton.activeSelf == true) taskStage = 600;

            }

        }

    }

}
