using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackscreen : MonoBehaviour {

    private float timer = 0f;
    private int stage = 0;
    private bool activated = false;

    public Text text;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
        if (activated) {
            timer += Time.deltaTime;
            if (timer > 2f && stage == 0) {
                text.text = "Resetting";
                stage = 1;
            }
            if (timer > 2.7f && stage == 1) {
                text.text = "Resetting.";
                stage = 2;
            }
            if (timer > 3.4f && stage == 2) {
                text.text = "Resetting..";
                stage = 3;
            }
            if (timer > 4.1f && stage == 3) {
                text.text = "Resetting...";
                stage = 4;
            }
            if (timer > 4.8f && stage == 4) {
                text.text = "";
                gameObject.SetActive(false);
                timer = 0;
                stage = 0;
                activated = false;
            }
        }

    }

    public void Activate() {

        gameObject.SetActive(true);
        activated = true;

    }

}
