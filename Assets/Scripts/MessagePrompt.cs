using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MessagePrompt : MonoBehaviour {

    public Text title, message;
    public Button okButton;

    public GameObject promptUser;

    public static MessagePrompt messagePrompt;

    // Start is called before the first frame update
    void Start() {

        okButton.onClick.AddListener(ClosePrompt);

        messagePrompt = this;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OpenPrompt(string title, string message, GameObject promptUser) {

        this.title.text = title;
        this.message.text = message;
        this.promptUser = promptUser;
        foreach (MonoBehaviour m in promptUser.GetComponentsInChildren<MonoBehaviour>())
            if (m.GetType().Name != "Image" && m.GetType().Name != "Mask" && m.GetType().Name != "Text") m.enabled = false;
        gameObject.SetActive(true);

    }

    private void ClosePrompt() {

        gameObject.SetActive(false);
        foreach (MonoBehaviour m in promptUser.GetComponentsInChildren<MonoBehaviour>()) m.enabled = true;
        promptUser = null;

    }

}
