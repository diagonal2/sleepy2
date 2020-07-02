using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour {

    public Text placeholder;
    public InputField text;
    
    public IInputPromptUser promptUser;

    public static InputPrompt promptManager;

    // Start is called before the first frame update
    void Start() {

        // placeholder = transform.Find("Placeholder").GetComponent<Text>();
        // text = GetComponent<InputField>();

        promptManager = this;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {

        if (gameObject.activeSelf == true) promptUser.PromptExitCheck();

    }

    public void OpenPrompt(IInputPromptUser promptUser) {

        if (gameObject.activeSelf == false) {
            this.promptUser = promptUser;
            foreach (MonoBehaviour m in promptUser.GameObject.GetComponentsInChildren<MonoBehaviour>())
                if (m.GetType().Name != "Image" && m.GetType().Name != "Mask" && m.GetType().Name != "Text") m.enabled = false;
            gameObject.SetActive(true);
            placeholder.text = promptUser.PlaceHolder;
            text.text = promptUser.Text;
            text.onValueChanged.AddListener(x => promptUser.OnValueChanged(x));
            text.onEndEdit.AddListener(x => promptUser.OnEndEdit(x));
        }

    }

    public void ClosePrompt() {

        gameObject.SetActive(false);
        
        foreach (MonoBehaviour m in promptUser.GameObject.GetComponentsInChildren<MonoBehaviour>()) m.enabled = true;

        promptUser = null;

        text.onEndEdit.RemoveAllListeners();
        text.onValueChanged.RemoveAllListeners();

    }

}
