using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDirectory : MonoBehaviour {

    private Transform items;
    public GameObject iconPrefab;
    public string[] imageNames;
    public Sprite[] imageSprites;
    private readonly Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();
    public FolderA me;

    public Text instructions;

    public static CurrentDirectory currentDirectory;

    // Start is called before the first frame update
    void Start() {

        currentDirectory = this;

        items = transform.Find("ItemMask/Items");
        for (int i = 0; i < imageNames.Length && i < imageSprites.Length; i++)
            images.Add(imageNames[i], imageSprites[i]);

        UpdateDirectory(CreateDirectories.master);

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void UpdateDirectory(FolderA folder) {

        if (folder.readable) {

            foreach (Transform child in items) Destroy(child.gameObject);

            me = folder;

            {
                GameObject childrenObject = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity, items);
                childrenObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                childrenObject.transform.Find("Image").GetComponent<Image>().sprite = images["FolderA"];
                childrenObject.transform.Find("Text").GetComponent<Text>().text = "..";
                childrenObject.GetComponent<Icon>().item = me.Parent;
                childrenObject.name = "..";
                items.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 90);
            }

            int i = 1;
            foreach (KeyValuePair<string, Item> kvp in me.Children) {
                Item children = kvp.Value;
                GameObject childrenObject = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity, items);
                childrenObject.GetComponent<RectTransform>().anchoredPosition = new Vector3((i % 5) * 90, -(i / 5) * 90, 0);
                childrenObject.transform.Find("Image").GetComponent<Image>().sprite = images[children.GetType().Name];
                childrenObject.transform.Find("Text").GetComponent<Text>().text = children.Name;
                childrenObject.GetComponent<Icon>().item = children;
                childrenObject.name = children.Name;
                items.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 90 + i / 5 * 90);
                i++;
            }

            GameObject.Find("Name").GetComponent<Text>().text = me.Name;
            GameObject.Find("Path").GetComponent<Text>().text = "Path: " + me.Path;

        } else MessagePrompt.messagePrompt.OpenPrompt("Access denied.", folder.cantReadMessage, gameObject);

    }

}
