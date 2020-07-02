using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class FolderA : Item {

    public override string Extension { get { return ""; } }

    private Dictionary<string, Item> children;
    public IReadOnlyDictionary<string, Item> Children { get { return children as IReadOnlyDictionary<string, Item>; } }

    private static readonly FileComparer fc = new FileComparer();

    public FolderA(string name, FolderA parent) {

        Name = name;
        Parent = parent;
        children = new Dictionary<string, Item>();

    }

    public int AddChild(string name, string content) {

        return AddChild(name, content, out _);

    }

    public int AddChild(string name, string content, out bool isFile) {

        isFile = false;
        int check = CheckName(name);

        if (check == 0 || check == 4) {
            string[] splitName = name.Split('.');
            if (splitName.Length == 1) children.Add(name, new FolderA(name, this));
            else {
                isFile = true;
                if (splitName[splitName.Length - 1].Equals("txt")) children.Add(name, new TextFile(name, this, content));
                //
            }
            ReorderChildren();
        }

        return check;

    }
    
    public void UpdateChildKey(string oldName) {

        Item child = children[oldName];
        if (child != null) {
            children.Remove(oldName);
            children.Add(child.Name, child);
        }

    }

    public void RemoveChild(string name) {

        children.Remove(name);

    }

    public int CheckName(string name) {

        if (children.ContainsKey(name)) return 5;
        else if (name.Any(char.IsWhiteSpace) || name.Contains("/") || name.Contains("\\")) return 1;
        else if (name.Equals("")) return 2;
        else if (name.Contains(".")) {
            if (name.EndsWith(".")) return 3;
            else return 4;
        } else return 0;

    }

    public void ReorderChildren() {

        children = children.OrderBy(x => x.Value, fc).ToDictionary(x => x.Key, x => x.Value);

    }

    public Item FindItem(string localPath) {

        string[] splitDirectory = localPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        
        FolderA traversal = this;
        for (int i = 0; i < splitDirectory.Length; i++) {
            if (!traversal.Children.ContainsKey(splitDirectory[i])) return null;
            else {
                Item tmp = traversal.Children[splitDirectory[i]];
                if (i == splitDirectory.Length - 1) return tmp;
                else if (tmp is FolderA) traversal = (FolderA) tmp;
                else return null;
            }
        }

        return traversal;

    }

    public override void Open() {

        CurrentDirectory.currentDirectory.UpdateDirectory(this);

    }

    public override void Close() {

        Debug.Log("How did you close me?!");

    }

}

class FileComparer : Comparer<Item> {
    public override int Compare(Item x, Item y) {

        if (!x.GetType().Equals(y.GetType())) {
            if (x is FolderA) return -1;
            else if (y is FolderA) return 1;
            else return x.GetType().Name.CompareTo(y.GetType().Name);
        } else return x.Name.CompareTo(y.Name);

    }

}
