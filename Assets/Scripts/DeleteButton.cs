using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour {

    private Toggle toggle;

    // Start is called before the first frame update
    void Start() {

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
            CurrentDirectory.currentDirectory.instructions.text = "Click on an item to delete, or press Esc to cancel.";
            colors.normalColor = new Color(0.784f, 0.784f, 0.784f);
            colors.selectedColor = new Color(0.784f, 0.784f, 0.784f);
            Icon.mode = 2;
        } else {
            CurrentDirectory.currentDirectory.instructions.text = "";
            colors.normalColor = new Color(1f, 1f, 1f);
            colors.selectedColor = new Color(1f, 1f, 1f);
            Icon.mode = 0;
        }

        toggle.colors = colors;

    }

    public void OnObjectSelected(Item item, bool isBack) {

        if (isBack) CurrentDirectory.currentDirectory.instructions.text = "Please select a valid item.";
        else if (item.Deletable == false) {
            MessagePrompt.messagePrompt.OpenPrompt("Access denied", item.CantDeleteMessage, CurrentDirectory.currentDirectory.gameObject);
            toggle.isOn = false;
        } else {
            item.Parent.RemoveChild(item.Name);
            CurrentDirectory.currentDirectory.UpdateDirectory(CurrentDirectory.currentDirectory.me);
            toggle.isOn = false;
        }

    }

}
