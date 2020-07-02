using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    public Item item;

    public static int mode = 0;
    private RenameButton renameButton;
    private DeleteButton deleteButton;

    // Start is called before the first frame update
    void Start() {

        // renameButton = FindObjectOfType<RenameButton>();
        // deleteButton = FindObjectOfType<DeleteButton>();

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {

        GetComponent<Image>().color = new Color(0.25f, 0.75f, 1f, 0.5f);

    }

    public void OnPointerExit(PointerEventData eventData) {

        GetComponent<Image>().color = new Color(0.25f, 0.75f, 1f, 0f);

    }

    public void OnPointerDown(PointerEventData eventData) {

        if (item != null) {
            GetComponent<Image>().color = new Color(0.25f, 0.75f, 1f, 0f);
            if (mode == 0) item.Open();
            else if (mode == 1) {
                if (renameButton == null) renameButton = FindObjectOfType<RenameButton>();
                renameButton.OnObjectSelected(item, gameObject.name == "..");
            } else if (mode == 2) {
                if (deleteButton == null) deleteButton = FindObjectOfType<DeleteButton>();
                deleteButton.OnObjectSelected(item, gameObject.name == "..");
            }
        }

    }

}
