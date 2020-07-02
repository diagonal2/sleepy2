using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Task2 : Story {    // create c3.txt by deleting c1.txt

    private TextFile task1_file;
    private List<string> blockerPaths = new List<string>();

    // Start is called before the first frame update
    void Start() {

        /* taskStage = 200;

        Directory.CreateDirectory("C:/Users/user0/Unity/sleepy2/Testdata/a/d/b/a/n/e/");

        folder = (FolderA) home.FindItem("/a/d/b/a/n/e/");
        folder.AddChild("c1.txt", "");

        transform.parent.parent.Find("FolderA/DeleteButton").gameObject.SetActive(true);

        task1_file = (TextFile) folder.Children["c1.txt"];
        blockerPaths = new List<string>(new string[] { "/a/d/c/", "/a/d/c/", "/a/d/c/" });

        task1_file.SetCantDeleteMessage(blockerPaths[0] + " is blocking deletion of " + task1_file.Path.Substring(28) + ". Consider asking for permission from so.");
        task1_file.SetDeletable(false);

        story.deleteButton.SetActive(true);

        initialised = true; */

    }

    // Update is called once per frame
    void Update() {
        
        if (taskStage >= 200 && taskStage < 300) {

            if (!initialised) {
                
                task1_file = (TextFile) folder.Children["c1.txt"];

                for (int i = 0; i < 3; i++) {
                    RandomTaskString(Random.Range(3, 7), out FolderA tmp);
                    blockerPaths.Add(tmp.Path.Substring(28));
                }
                blockerPaths.Sort();                // create virtual blockerPaths
                Debug.Log(blockerPaths[0] + " " + blockerPaths[1] + " " + blockerPaths[2]);

                task1_file.SetCantDeleteMessage(blockerPaths[0] + " is blocking deletion of " + task1_file.Path.Substring(28) + ". Consider asking for permission from so.");
                task1_file.SetDeletable(false);

                initialised = true;

            }

            for (int i = 0; i < 3; i++) {

                if (home.FindItem(blockerPaths[i]) != null) {
                    task1_file.SetCantDeleteMessage(blockerPaths[i] + " is blocking deletion of " + task1_file.Path.Substring(28) + ". Consider asking for permission from so.");
                    break;
                }

                if (i == 2) {
                    task1_file.SetDeletable(true);
                    taskStage = 290;
                }

            }

            if (!folder.Children.ContainsKey("c1.txt")) {

                PassParameters(2, 3, folderString, folder, null);
                taskStage = 300;

            }

        }

    }

}
