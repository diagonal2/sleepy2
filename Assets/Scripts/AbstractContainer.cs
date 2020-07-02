using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;

public class AbstractContainer : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

}

/* public interface IItem {

    string Name { get; }
    string Path { get; }

    FolderA Parent { get; }

    void Open();
    void Close();

} */

public abstract class Item {

    public string Name { get; protected set; }
    public string Path { get { return (Parent != null ? Parent.Path : "") + Name + (this is FolderA ? "/" : ""); } }
    public abstract string Extension { get; }
    
    public FolderA Parent { get; protected set; }

    public bool readable = true;
    public string cantReadMessage;
    public bool Deletable { get; private set; } = true;
    protected List<Item> deleteBlockers = new List<Item>();
    public string CantDeleteMessage { get; private set; }

    public abstract void Open();
    public abstract void Close();

    public int Rename(string name) {

        if (!name.EndsWith(Extension)) return 10;

        int check = Parent.CheckName(name);

        if (check == 0 || (!(this is FolderA) && check == 4)) {
            string oldName = Name;
            Name = name.EndsWith(Extension) ? name : name + Extension;
            Parent.UpdateChildKey(oldName);
            Parent.ReorderChildren();
        }

        return check;

    }

    public void SetDeletable(bool value) {

        SetDeletable(this, value);

    }

    private void SetDeletable(Item item, bool value) {

        if (value) {
            if (deleteBlockers.Contains(item)) deleteBlockers.Remove(item);
            if (deleteBlockers.Count == 0) Deletable = true;
            if (Parent != null) Parent.SetDeletable(item, true);
        } else {
            Deletable = false;
            deleteBlockers.Add(item);
            if (Parent != null) Parent.SetDeletable(item, false);
        }

        if (!Deletable) CantDeleteMessage = deleteBlockers[0].CantDeleteMessage;

    }

    public void SetCantDeleteMessage(string message) {

        SetCantDeleteMessage(this, message);

    }

    private void SetCantDeleteMessage(Item item, string message) {

        CantDeleteMessage = message;
        if (!Deletable)
            if (Parent != null) Parent.SetCantDeleteMessage(item, Parent.deleteBlockers[0].CantDeleteMessage);

    }

}

public interface IInputPromptUser {

    string PlaceHolder { get; set; }
    string Text { get; set; }

    GameObject GameObject { get; }

    void OnValueChanged(string text);
    void OnEndEdit(string text);
    void PromptExitCheck();

}

public interface IMessagePromptUser {

    string Title { get; set; }
    string Message { get; set; }

    GameObject GameObject { get; }

}
