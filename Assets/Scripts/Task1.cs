using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.WSA;

public class Task1 : Story {    // open c1.txt

    private string password;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (taskStage >= 100 && taskStage < 200) {

            if (!initialised) {

                password = (string) info[0];

                Directory.CreateDirectory(dataPath + folder.Path.Substring(28));
                File.WriteAllText(dataPath + folder.Path.Substring(28) + "c1.txt", "Cannot access content.");   // create real c1.txt

                /* char[] passwordChars = password.ToCharArray();
                int replaceIndex = Random.Range(0, 8), charsIndex = Random.Range(26, 52);
                while (passwordChars[replaceIndex] == chars[charsIndex]) charsIndex = Random.Range(26, 52);
                passwordChars[replaceIndex] = chars[charsIndex];  */
                string wrongPassword = RandomSubstitute(password, out int replaceIndex);
                File.WriteAllText(dataPath + folder.Path.Substring(28) + ".pass", wrongPassword);     // create real .pass

                folder.readable = false;
                folder.cantReadMessage = "Password mismatch.";
                password = (string) info[0];
                folder.AddChild("c1.txt", "[log]\n\nMessage recovered.\n\nDeleting is irreversible and the \"confirm delete\" prompt is disabled. Delete with caution.\n\nDelete this file immediately after reading.");
                file = (TextFile) folder.Children["c1.txt"];        // create virtual c1.txt

                // PassParameters(1, 11, "", null, wrongPassword[replaceIndex]);

                initialised = true;
            }

            if (File.ReadAllText(dataPath + folder.Path.Substring(28) + ".pass").Equals(password)) {

                folder.readable = true;

                taskStage = 190;

            }

            if (CurrentWindow.currentWindow.File == file && story.deleteButton.activeSelf == false) {

                story.deleteButton.SetActive(true);

                PassParameters(1, 2, folderString, folder, null);
                taskStage = 200;

            }

        }

    }

}
