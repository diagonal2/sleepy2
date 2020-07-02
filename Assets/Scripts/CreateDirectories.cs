using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateDirectories : MonoBehaviour {

    public string[] directories;

    public static readonly FolderA master = new FolderA("", null);

    public static readonly FolderA home = Create("/are/you/as/smart/as/this/is/.txt");
    /* public static readonly string[][] folderNames = new string[][] {
        new string[] { "a", "b", "c", "e", "r" },
        new string[] { "d", "g", "h", "s", "e" },
        new string[] { "m", "l", "b", "c", "t" },
        new string[] { "i", "q", "r", "a", "u" },
        new string[] { "n", "v", "w", "p", "r" },
        new string[] { "x", "f", "g", "e", "n" },
    }; */

    void Awake() {

        // Init(home, 0);
        
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private static FolderA Create(string input) {

        string[] splitInput = input.Split(new char[] { ' ' }, 2);
        string[] splitDirectory = splitInput[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        string content = (splitInput.Length == 2) ? splitInput[1] : "";

        FolderA traversal = master;
        for (int i = 0; i < splitDirectory.Length; i++) {
            int check = (i != splitDirectory.Length - 1) ? traversal.AddChild(splitDirectory[i], "", out bool isFile) : traversal.AddChild(splitDirectory[i], content, out isFile);
            if (isFile || (check != 0 && check != 4 && check != 5)) break;
            else traversal = (FolderA) traversal.Children[splitDirectory[i]];
        }

        return traversal;

    }

    /* private void Init(FolderA dir, int layer) {

        if (layer >= 0 && layer <= 5)
            foreach (string s in folderNames[layer]) {
                dir.AddChild(s, "");
                Init((FolderA) dir.Children[s], layer + 1);
            }
        
    } */

}
