using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWindow : MonoBehaviour {

    public GameObject ForeWindow { get; private set; }
    public Item File { get; private set; }
    private Button closeButton;
    // public PromptManager prompt;

    public static CurrentWindow currentWindow;

    // Start is called before the first frame update
    void Start() {

        currentWindow = this;

    }

    // Update is called once per frame
    void Update() {
        
    }

    public GameObject OpenWindow(Item file) {

        if (file.readable) {
            if (ForeWindow == null) {
                ForeWindow = transform.Find(file.GetType().Name).gameObject;
                this.File = file;
                foreach (MonoBehaviour m in CurrentDirectory.currentDirectory.GetComponentsInChildren<MonoBehaviour>())
                    if (m.GetType().Name != "Image" && m.GetType().Name != "Mask" && m.GetType().Name != "Text") m.enabled = false;
                ForeWindow.SetActive(true);
                closeButton = ForeWindow.transform.Find("CloseButton").GetComponent<Button>();
                closeButton.onClick.AddListener(delegate { file.Close(); });
            }
        } else MessagePrompt.messagePrompt.OpenPrompt("Access denied.", file.cantReadMessage, gameObject);

        return ForeWindow;

    }

    public GameObject CloseWindow() {

        ForeWindow.SetActive(false);

        foreach (MonoBehaviour m in CurrentDirectory.currentDirectory.GetComponentsInChildren<MonoBehaviour>()) m.enabled = true;

        ForeWindow = null;
        File = null;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.Invoke();

        return null;

    }

}
