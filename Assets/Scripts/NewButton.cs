using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class NewButton : MonoBehaviour, IInputPromptUser {

    public string PlaceHolder { get; set; }
    public string Text { get; set; }

    public GameObject GameObject { get { return transform.parent.gameObject; } }

    private Toggle toggle;

    // Start is called before the first frame update
    void Start() {

        PlaceHolder = "Enter name...";
        Text = "";

        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnStateChanged);

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnStateChanged(bool value) {

        if (value) {
            CurrentDirectory.currentDirectory.instructions.text = "Enter a valid name of your new object and press Enter, or press Esc to cancel.";
            InputPrompt.promptManager.OpenPrompt(GetComponent<NewButton>());
        }

    }

    public void OnValueChanged(string name) {

    }

    public void OnEndEdit(string name) {

        int check = CurrentDirectory.currentDirectory.me.AddChild(name, "");
        
        if (check != 0 && check != 4) {
            InputPrompt.promptManager.text.text = "";
            if (check == 1) InputPrompt.promptManager.placeholder.text = "Whitespace and slashes ['/' or '\\'] are not allowed.";
            else if (check == 2) InputPrompt.promptManager.placeholder.text = "Please enter a name.";
            else if (check == 3) InputPrompt.promptManager.placeholder.text = "The name cannot end with '.' (dot).";
            else if (check == 5) InputPrompt.promptManager.placeholder.text = "Another item with the same name already exists.";
        } else {
            CurrentDirectory.currentDirectory.UpdateDirectory(CurrentDirectory.currentDirectory.me);
            CurrentDirectory.currentDirectory.instructions.text = "";
            InputPrompt.promptManager.ClosePrompt();
            toggle.isOn = false;
        }

    }

    public void PromptExitCheck() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            InputPrompt.promptManager.ClosePrompt();
            CurrentDirectory.currentDirectory.instructions.text = "";
            toggle.isOn = false;
        }

    }

}
