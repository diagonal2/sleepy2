using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RenameButton : MonoBehaviour, IInputPromptUser {

    public string PlaceHolder { get; set; }
    public string Text { get; set; }
    private Item renamee;

    public GameObject GameObject { get { return transform.parent.gameObject; } }

    private Toggle toggle;


    // Start is called before the first frame update
    void Start() {

        PlaceHolder = "Enter new name...";
        Text = "";

        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnStateChanged);

    }

    // Update is called once per frame
    void Update() {

        if (toggle.isOn && Input.GetKeyDown(KeyCode.Escape)) {
            CurrentDirectory.currentDirectory.instructions.text = "";
            toggle.isOn = false;
        }

    }

    private void OnStateChanged(bool value) {

        ColorBlock colors = toggle.colors;

        if (value) {
            CurrentDirectory.currentDirectory.instructions.text = "Click on an item to rename, or press Esc to cancel.";
            colors.normalColor = new Color(0.784f, 0.784f, 0.784f);
            colors.selectedColor = new Color(0.784f, 0.784f, 0.784f);
            Icon.mode = 1;
        } else {
            CurrentDirectory.currentDirectory.instructions.text = "";
            colors.normalColor = new Color(1f, 1f, 1f);
            colors.selectedColor = new Color(1f, 1f, 1f);
            Icon.mode = 0;
            renamee = null;
        }

        toggle.colors = colors;

    }

    public void OnObjectSelected(Item item, bool isBack) {

        if (!isBack) {
            CurrentDirectory.currentDirectory.instructions.text = "Enter a valid name of your object and press Enter, or press Esc to cancel.";
            InputPrompt.promptManager.OpenPrompt(this);
            renamee = item;
        } else CurrentDirectory.currentDirectory.instructions.text = "Please select a valid item.";

    }

    public void OnValueChanged(string name) {

    }

    public void OnEndEdit(string name) {

        name = name.EndsWith(renamee.Extension) ? name : name + renamee.Extension;
        int check = renamee.Rename(name);
        
        if (check != 0 && (renamee is FolderA || check != 4)) {
            InputPrompt.promptManager.text.text = "";
            if (check == 1) InputPrompt.promptManager.placeholder.text = "Whitespace and slashes ['/' or '\\'] are not allowed.";
            else if (check == 2) InputPrompt.promptManager.placeholder.text = "Please enter a name.";
            else if (check == 3) InputPrompt.promptManager.placeholder.text = "The name cannot end with '.' (dot).";
            else if (check == 4) InputPrompt.promptManager.placeholder.text = "Folder names cannot contain '.' (dot).";
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
