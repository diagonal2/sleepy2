using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFile : Item {

    public override string Extension { get { return ".txt"; } }

    public string text;

    private GameObject textFileObject;
    private TMPro.TMP_InputField inputField;

    public TextFile(string name, FolderA parent, string text) {

        Name = name.EndsWith(".txt") ? name : name + ".txt";
        Parent = parent;
        this.text = text;

    }

    public override void Open() {

        textFileObject = CurrentWindow.currentWindow.OpenWindow(this);
        textFileObject.transform.Find("Name").GetComponent<Text>().text = Name;

        if (inputField == null) inputField = textFileObject.transform.Find("InputField").GetComponent<TMPro.TMP_InputField>();
        inputField.text = text;
        
    }

    public override void Close() {

        text = inputField.text;
        CurrentWindow.currentWindow.CloseWindow();

    }

    public void Update() {

        inputField.text = text;
        inputField.Select();
        inputField.MoveTextEnd(false);
        inputField.ActivateInputField();

    }

}
