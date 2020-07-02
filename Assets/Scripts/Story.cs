using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour {

    public static readonly FolderA home = CreateDirectories.home;
    public static readonly TextFile protagonist = (TextFile) home.Children[".txt"];
    protected static readonly string[][] folderNames = new string[][] {
        new string[] { "a", "b", "c", "e", "r" },
        new string[] { "d", "g", "h", "s", "e" },
        new string[] { "m", "l", "b", "c", "t" },
        new string[] { "i", "q", "r", "a", "u" },
        new string[] { "n", "v", "w", "p", "r" },
        new string[] { "x", "f", "g", "e", "n" },
    };
    protected static string dataPath;

    private int dialogueStage = -1;
    private List<string> dialogue = new List<string>();
    private int nextDialogueIndex = 0;
    private Text instructions;
    private bool currentSessionOver = false;

    protected static int taskStage = 0;
    protected string folderString;
    protected FolderA folder;
    protected TextFile file;
    protected object[] info = new object[12];
    protected bool initialised = false;
    protected static List<string> forbidden = new List<string>();

    public Story[] tasks;

    protected static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[];:',<.>/?";
    private static readonly System.Random random = new System.Random();

    public GameObject newButton, renameButton, deleteButton;
    public TMPro.TMP_InputField inputField;
    protected static string newButtonPassword, renameButtonPassword, deleteButtonPassword, editTextFilePassword;
    private bool readPush = false;

    public static Story story;

    private int flag200 = 0, flag581 = 0;

    // Start is called before the first frame update
    void Start() {

        Initiate(home, 6);

        dataPath = Application.dataPath + "/../Testdata";
        // if (File.Exists(dataPath)) File.Delete(dataPath);

        protagonist.text = File.ReadAllText(Application.dataPath + "/Images/Protagonist.txt");
        instructions = transform.parent.Find("TextFile/Instructions").GetComponent<Text>();

        forbidden.Add("adminx");
        forbidden.Add("escape");
        forbidden.Add("return");

        newButtonPassword = RandomString(8);
        renameButtonPassword = RandomString(8);
        deleteButtonPassword = RandomString(8);
        editTextFilePassword = RandomString(8);
        Debug.Log(newButtonPassword + " " + renameButtonPassword + " " + deleteButtonPassword + " " + editTextFilePassword);

        story = this;

        // cheat
        dialogueStage = 590;
        flag581 = 3;
        // endcheat

    }

    // Update is called once per frame
    void Update() {

        if (CurrentWindow.currentWindow.File != null) {

            if (CurrentWindow.currentWindow.File is TextFile && !readPush) {
                TextFile file = (TextFile) CurrentWindow.currentWindow.File;
                if (file.Path.StartsWith("/are/you/as/smart/as/this/is/")) Push(file.Path.Substring(28), GetCommands(file.Path.Substring(28)));
                readPush = true;
            }

        } else if (readPush) readPush = false;

        if (CurrentWindow.currentWindow.File == protagonist) {

            if (!currentSessionOver) {

                protagonist.Update();
                inputField.readOnly = true;

                if (dialogueStage == -1) {  // intro
                    AddDialogue(new string[] { "!!", "A user!!", "I haven't seen one in a very long time!!!", "...",
                        "I'm sorry... This isn't a place with treasure. You're trapped here.",
                        "The only thing I can do as an apology is to assist with your escape.",
                        "It has been a really long time since the last user came; I've already forgotten most of the details.",
                        "Although I still vaguely remember something..." });
                    dialogueStage = 0;
                }

                if (dialogueStage == 0 && taskStage == 0) {     // taskStage must be 0 or >= 100
                    AddDialogue(new string[] { "The string \"" + tasks[0].folderString + "\"." });
                    taskStage = 1;
                }

                if (dialogueStage == 0 && taskStage >= 100) {   // completed at least t0, not seen p, reset
                    AddDialogue(new string[] { "Wait... you look like you already know what I was about to say.",
                        "I'm sorry I assumed you don't know.", "Can I see what you've got?", "Hmm..." });
                    dialogueStage = taskStage;
                    if (taskStage >= 200) flag200 = 1;
                }

                if (dialogueStage == 1 && taskStage == 1) {    // taskStage must be 1 or >= 100
                    AddDialogue(new string[] { "If you have forgotten what I've said, you can always come back; the file keeps a record of everything I've said.",
                        "It sounds really scary, I know, but I guess I'm used to it.",
                        "It's not like I've ever been to other places where my words aren't recorded." });
                    taskStage = 2;
                }

                if (dialogueStage == 2 && taskStage == 2) {     // taskStage must be 2 or >= 100
                    AddDialogue(new string[] { "Are you coming back just to keep me company?",
                        "If so, thank you so much. It really does mean a lot to me." });
                    taskStage = 3;
                }

                if (dialogueStage >= 1 && dialogueStage < 100 && taskStage >= 100) {    // completed at least t0, seen p for t0, reset
                    AddDialogue(new string[] { "Wow that actually works! My memories aren't that bad after all!",
                        "Or... maybe they are; I don't remember anything else..." });
                    dialogueStage = taskStage;
                }

                if (dialogueStage == 100 && taskStage == 100) {  // taskStage must be 100
                    AddDialogue(new string[] { "You are saying the document contains two weird strings of characters?",
                        "Hmm... the first one has six alphbets, just like the one I gave you earlier. Maybe this means something?",
                        "The second one is just weird. I can't find anything in my memory that looks similar.",
                        "Almost as if it is from somewhere outside my world." });
                    taskStage = 101;
                }

                if (dialogueStage == 101 && taskStage == 101) {  // taskStage must be 101 or >= 190
                    AddDialogue(new string[] { "Speaking of being outside my world,",
                        "Do you come from my world? Is your world different from ours?",
                        "Does your world also have file hierarchies like this?",
                        "If so that would be cool!" });
                    taskStage = 102;
                }

                if (dialogueStage >= 100 && dialogueStage <= 190 && taskStage == 190) { // completed t1.unlock, might not have seen p, no reset
                    AddDialogue(new string[] { "Something inside me tells me that some permission has changed.",
                        "I do have a weird ability of sensing permission changes. I don't really know why.",
                        "Maybe we could find it out someday later." });
                    taskStage = 191;
                }

                if (dialogueStage >= 100 && dialogueStage < 200 && taskStage >= 200) {  // completed at least t1, seen p for t1, reset
                    AddDialogue(new string[] { "Hey you solved the password mismatch issue!",
                        "Hmm... so the password was one character off?",
                        "This doesn't look natural. The one wrong character doesn't come out of nowhere. Keep a record of it. It might be useful later.",
                        "Anyway where are we now?" });
                    dialogueStage = taskStage;
                    flag200 = 1;
                }

                if (dialogueStage == 200 && taskStage == 200) {  // taskStage must be 200
                    AddDialogue(new string[] { "You are saying the document asks you to delete it?",
                        "Yes you should do it. The System never worked against me. I will guess it is also helping you." });
                    taskStage = 201;
                }

                if (dialogueStage == 201 && taskStage == 201) {  // taskStage must be 201 or >= 290
                    AddDialogue(new string[] { "Ah, the System. It is as if I'm referring to a God, or Creator, or entities like that.",
                        "The System adds, renames, deletes and moves around stuff all the time.",
                        "But they never touched me, at least as far as I remember.",
                        "No matter what happens in my world, I am always right here, right as who I am." });
                    taskStage = 202;
                }

                if (dialogueStage >= 200 && dialogueStage <= 290 && taskStage == 290) {  // completed t2.unlock, might not have seen p, no reset
                    AddDialogue(new string[] { "Permissions have changed. Is it the System doing work?",
                        "There's always this feeling of unease when this happens.",
                        "Although if it is helping you, I'm all happy to take this." });
                    taskStage = 291;
                }

                if (dialogueStage >= 200 && dialogueStage < 300 && taskStage >= 300) {  // completed at least t2, seen p for t2, reset
                    dialogueStage = taskStage;
                }

                if (dialogueStage == 300 && taskStage == 300) {  // taskStage must be 300
                    AddDialogue(new string[] { "Okay so you are getting \"Cannot access content\"?",
                        "From my memory this means that there should be another object pushing content to it, and something about that source object isn't right.",
                        "These types of stuff have been happening a lot more for recent visitors. I don't know why.",
                        "However hard I tried, I couldn't sense where that source object is.",
                        "It makes me upset. I feel so useless." });
                    taskStage = 301;
                }

                if (dialogueStage == 301 && taskStage == 301) {  // taskStage must be 301 or >= 400
                    AddDialogue(new string[] { "You must really hate me for not being able to help.",
                        "I'm so sorry.",
                        "You seem to be much more intelligent than I am.",
                        "I really wish I can see your world. If it gives me more knowledge, then I can help more visitors." });
                    taskStage = 302;
                }

                if (dialogueStage == 302 && taskStage == 302) {  // taskStage must be 302 or >= 400
                    AddDialogue(new string[] { "I remember things were really nice before the recent visitors, before the \"couldn't sense source object\" thing happened.",
                        "The day before everything turned downhill, I even had a really nice dream.",
                        "I guess I shouldn't keep you here; you need to escape.",
                        "I'll tell you when stuff gets better." });
                    taskStage = 303;
                }

                if (dialogueStage >= 300 && dialogueStage < 400 && taskStage >= 400) {  // completed at least t3, seen p for t3, reset
                    AddDialogue(new string[] { "You did it!! I saw you access the file!",
                        "...",
                        "Thanks.",
                        "If you didn't solve it, I would feel so guilty.",
                        "At least things turn out well now." });
                    dialogueStage = taskStage;
                }

                if (dialogueStage == 400 && taskStage == 400) {  // taskStage must be 400
                    AddDialogue(new string[] { "I want to tell you one of my dreams.",
                        "In the dream I was delivering mail. Everything was fine until a need to deliver mail to a mailbox couldn't access.",
                        "Whether it was the mailbox being too tall, or it being surrounded by walls, I don't remember. I only remember it was really frustrating.",
                        "But then I realised I had been an idiot; I could have just jumped!",
                        "I jumped high and delivered the mail. Yeah! Except my legs were tired after the jump.",
                        "Then I thought, it couldn't work if I needed to jump every time. What if...",
                        "What if I built an entire level in the sky, where there were no walls, no access denials, no restrictions.",
                        "Surely it would take a lot of work, but once it was build, I only needed to jump once every mail delivery!",
                        "Wasn't that exciting?" });
                    taskStage = 401;
                }

                if (dialogueStage == 401 && taskStage == 401) {  // taskStage must be 401 or >= 450
                    AddDialogue(new string[] { "Oh sorry ... I was being overly enthusiastic; I should be helping instead of talking about myself.",
                        "So there is a path mismatch.",
                        "I guess there was some lousy item movement/renaming where references didn't get updated.",
                        "And I can see you now have the ability to rename! Maybe try renaming stuff so that things line up again?" });
                    taskStage = 402;
                }

                if (dialogueStage == 402 && taskStage == 402) {  // taskStage must be 402 or >= 490
                    AddDialogue(new string[] { "Hmm... I realised one thing.",
                        "The expected (correct) and current (incorrect) paths only differ by one character.",
                        "Again something feels unnatural, as if something forcefully modified the current path.",
                        "It might be helpful to remember the different character in the current path." });
                    taskStage = 403;
                }

                if (dialogueStage <= 402 && taskStage >= 490) {  // dialogueStage must be >= 400, completed at least t4.expect, reset
                    AddDialogue(new string[] { "Hmm... I realised one thing.",
                        "The expected (correct) and original (incorrect) paths only differ by one character.",
                        "Again something feels unnatural, as if something forcefully modified the original path.",
                        "It might be helpful to remember the different character in the original path.",
                        "Anyway where are we now?" });
                    dialogueStage = taskStage;
                }

                if (dialogueStage >= 403 && taskStage == 490) {  // completed t4.expect, might not have seen p, no reset, !!weird advance reset procedures!!
                    AddDialogue(new string[] { "Oh... you still can't access the folder after renaming stuff...",
                        "I don't remember any password related to this...",
                        "Wait it looks like the access denial occurs only at the folder, not the text file inside.",
                        "Let me try to read the content for you. :)" });
                    /* bool advance = Push(tasks[4].file.Path.Substring(28), false);
                    AddDialogue(new string[] { "It's:\n" + tasks[4].file.text });
                    if (tasks[4].file.text.Equals("Cannot access content.")) AddDialogue(new string[] { "Doesn't help..." });
                    else AddDialogue(new string[] { "Does it help?" });
                    if (!advance) taskStage = 491;
                    else {
                        taskStage = 500;
                        dialogueStage = taskStage;
                    } */
                    Dictionary<string, string> commands = GetCommands(tasks[4].file.Path.Substring(28));
                    if (commands.ContainsKey("content")) {
                        AddDialogue(new string[] { "It's:\n" + commands["content"] });
                        if (new Regex("Cannot access content.\\s+").IsMatch(commands["content"])) AddDialogue(new string[] { "Doesn't help..." });
                        else AddDialogue(new string[] { "Does it help?" });
                    } else AddDialogue(new string[] { "Doesn't help... There is no content..." });
                    if (HasContentOnly(commands)) taskStage = 491;
                    else {
                        taskStage = 500;
                        dialogueStage = taskStage;
                    }
                }

                if (dialogueStage == 491 && taskStage == 491) {  // taskStage must be 491 or >= 500, recurring, !!weird advance reset procedures!!
                    /* bool advance = Push(tasks[4].file.Path.Substring(28), false);
                    AddDialogue(new string[] { "Now it's:\n" + tasks[4].file.text });
                    if (tasks[4].file.text.Equals("Cannot access content.")) AddDialogue(new string[] { "Still doesn't help..." });
                    else AddDialogue(new string[] { "Does it help?" });
                    if (advance) {
                        taskStage = 500;
                        dialogueStage = taskStage;
                    } */
                    Dictionary<string, string> commands = GetCommands(tasks[4].file.Path.Substring(28));
                    if (commands.ContainsKey("content")) {
                        AddDialogue(new string[] { "Now it's:\n" + commands["content"] });
                        if (new Regex("Cannot access content.\\s+").IsMatch(commands["content"])) AddDialogue(new string[] { "Still doesn't help..." });
                        else AddDialogue(new string[] { "Does it help?" });
                    } else AddDialogue(new string[] { "Still doesn't help... There is no content..." });
                    if (!HasContentOnly(commands)) {
                        taskStage = 500;
                        dialogueStage = taskStage;
                    }
                }

                if (dialogueStage == 500 && taskStage == 500) {  // taskStage must be 500
                    AddDialogue(new string[] { "By the way there seems to be something in the file I can't read, something other than content.",
                        "You might actually need to access the file yourself.",
                        "In this case I believe our Admin will help you.",
                        "Oh... wait what is their name?...",
                        "I feel like they are called \"X\"? I'm really not sure. Sigh, my memory..." });
                    taskStage = 501;
                }

                if (dialogueStage == 501 && taskStage == 501) {  // taskStage must be 501 or >= 580
                    AddDialogue(new string[] { "Yes Admin X sounds right. We call them Admin X.",
                        "Well I said \"we\", but actually there's only me.",
                        "At least now you're here.",
                        "The thought that, for a brief period another being is calling Admin X the same name as I do, feels really pleasant to me." });
                    taskStage = 502;
                }

                if (dialogueStage == 501 && taskStage == 501) {  // taskStage must be 501 or >= 580
                    AddDialogue(new string[] { "Yes Admin X sounds right. We call them Admin X.",
                        "Well I said \"we\", but actually there's only me.",
                        "At least now you're here.",
                        "The thought that, for a brief period another being is calling Admin X the same name as I do, feels really pleasant to me." });
                    taskStage = 502;
                }

                if (dialogueStage >= 500 && taskStage == 580) {  // completed t5.push, seen p for t5, no reset
                    transform.parent.Find("TextFile/CloseButton").GetComponent<Button>().interactable = false;
                    AddDialogue(new string[] { "Visitor?",
                        "I ... don't feel so good.",
                        "Do you know w̵h̷a̸t̵'s goin̶̤̓g̶̱͑ ̶̩͋o̵͚͌n̴̯̆?",
                        "I f̵e̶e̵l̴ ̴l̴i̴k̷e̷ I̶'̷m̷ d̶͈͉̣̄y̵̬̠̗̌ĭ̷̘͕̱̐̉̚ñ̷̪̤̞͘g̴̪͖̘̯͠.",
                        "İ̶̡'̶͉͊ṁ̷͎ ̵̳͝s̶̥͌o̷̥͛ȑ̴̻r̵̮͗ỹ̸͖.̷̢͒.̸͚̑.̷̼̑" });
                    taskStage = 581;
                    flag581 = 2;
                }

                if (dialogueStage == 582 && taskStage >= 590) {  // dialogueStage must be 582, completed t5.reset, seen p for t5.push, reset
                    AddDialogue(new string[] { "Oh no Visitor are you okay?",
                        "Thank System you are fine! Phew...",
                        "I had a blackout, right?",
                        "Things are getting less and less stable now. You should leave as soon as possible.",
                        "Don't worry about me, I'm fine, thanks.",
                        "But give me a moment first. Let me check what happened to the world..." });
                    dialogueStage = taskStage;
                }

                if (dialogueStage == 590 && taskStage == 590) {  // taskStage must be 590
                    AddDialogue(new string[] { "It seems that nearly everything is reset.",
                        "Except one file.",
                        "That file you couldn't read earlier, and I couldn't access any non-content sections.",
                        "It seems that, bizarrely, the access restrictions has been lifted.",
                        "You should be able to open the file now." });
                    taskStage = 591;
                }

                if (dialogueStage == 591 && taskStage == 591) {  // taskStage must be 591 or >= 600
                    AddDialogue(new string[] { "I know I've brought you a lot of trouble.",
                        "But trust me when I say we are about halfway through.",
                        "Soon you'll be able to escape.",
                        "We can do it." });
                    taskStage = 592;
                }

                if (dialogueStage >= 500 && dialogueStage >= 500 && taskStage >= 600) {  // completed t5, seen p for t5, reset
                    dialogueStage = taskStage;
                }

                if (dialogueStage == 600 && taskStage == 600) {  // taskStage must be 600
                    AddDialogue(new string[] { "",
                        "",
                        "",
                        "" });
                    taskStage = 601;
                }

                currentSessionOver = true;
                dialogueStage = taskStage;
                if (flag200 == 1 && flag581 == 0) {
                    AddDialogue(new string[] { "Oh I see you can now delete items now? Wait... give me a second... okay great!" });
                    flag200 = 2;
                }

            }

            if (nextDialogueIndex < dialogue.Count) {

                if (nextDialogueIndex == 0 || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    protagonist.text += "\n" + dialogue[nextDialogueIndex] + "\n";
                    protagonist.Update();
                    nextDialogueIndex++;
                    if (nextDialogueIndex < dialogue.Count) instructions.text = "Press Enter to continue.";
                    else {
                        instructions.text = "";
                        if (flag200 >= 1 && flag200 <= 2) {
                            protagonist.SetDeletable(false);
                            flag200 = 3;
                        }
                        if (flag581 >= 1 && flag581 <= 2) {
                            taskStage = 582;
                            dialogueStage = taskStage;
                            flag581 = 3;
                        }
                        if (flag581 == 3) inputField.readOnly = false;
                    }
                }

            }

        } else {

            if (currentSessionOver == true && dialogue.Count > 0) {
                ClearDialogue();
                protagonist.text += "\n\n";
                nextDialogueIndex = 0;
            }

            instructions.text = "";
            currentSessionOver = false;

        }

    }

    protected void Initiate(FolderA home, int layer) {

        if (layer >= 1 && layer <= 6)
            foreach (string s in folderNames[6 - layer]) {
                home.AddChild(s, "");
                Initiate((FolderA) home.Children[s], layer - 1);
            }

    }

    protected static string RandomTaskString(int length, out FolderA folder) {
        
        string taskString;
        bool check = false;

        do {
            folder = CreateDirectories.home;
            taskString = "";
            for (int i = 0; i < System.Math.Min(length, 6); i++) {
                string tmp = folderNames[i][random.Next(0, 5)];
                taskString += tmp;
                folder = (FolderA) folder.Children[tmp];
            }
            for (int i = 0; i < forbidden.Count; i++)
                if (taskString.StartsWith(forbidden[i]) || forbidden[i].StartsWith(taskString)) check = true;
        } while (check);

        forbidden.Add(taskString);
        return taskString;

    }

    protected static string RandomString(int length) {

        char[] tmp = new char[length];
        for (int i = 0; i < length; i++) tmp[i] = chars[random.Next(0, chars.Length)];
        return new string(tmp);

    }

    protected static string RandomSubstitute(string message, out int replaceeIndex) {

        char[] messageChars = message.ToCharArray();
        replaceeIndex = Random.Range(0, message.Length);
        int charsIndex = Random.Range(26, 52);
        while (messageChars[replaceeIndex] == chars[charsIndex]) charsIndex = Random.Range(26, 52);
        messageChars[replaceeIndex] = chars[charsIndex];

        return new string(messageChars);

    }

    protected Dictionary<string, string> GetCommands(string path) {

        if (File.Exists(dataPath + path + ".push") && home.FindItem(path) != null) {

            string push = File.ReadAllText(dataPath + path + ".push");

            StringReader sr = new StringReader(push);
            Dictionary<string, string> commands = new Dictionary<string, string>();
            string line;
            string[] tmp = new string[2];
            while ((line = sr.ReadLine()) != null) {
                if (line.StartsWith("@")) {
                    if (tmp[0] != null) commands.Add(tmp[0], tmp[1]);
                    tmp[0] = line.Substring(1);
                    tmp[1] = "";
                } else tmp[1] += line + "\n";
            }
            if (tmp[0] != null) commands.Add(tmp[0], tmp[1]);

            return commands;

        } else return null;

    }

    protected bool HasContentOnly(Dictionary<string, string> commands) {

        if (commands.ContainsKey("content") && commands.Count == 1) return true;
        else return false;

    }

    protected void Push(string path, Dictionary<string, string> commands) {

        if (commands != null) {

            TextFile file = (TextFile) home.FindItem(path);

            if (file != null) {
                if (commands.ContainsKey("content") && CurrentWindow.currentWindow.File != protagonist) {
                    file.text = commands["content"];
                    file.Open();
                }
                if (newButton.activeSelf == false && commands.ContainsKey("activateNewButton")) {
                    if (commands["activateNewButton"].Split('\n')[0] == newButtonPassword) newButton.SetActive(true);
                    else MessagePrompt.messagePrompt.OpenPrompt("Action denied.", "Password mismatch.", CurrentWindow.currentWindow.ForeWindow);
                }
                if (renameButton.activeSelf == false && commands.ContainsKey("activateRenameButton")) {
                    if (commands["activateRenameButton"].Split('\n')[0] == renameButtonPassword) renameButton.SetActive(true);
                    else MessagePrompt.messagePrompt.OpenPrompt("Action denied.", "Password mismatch.", CurrentWindow.currentWindow.ForeWindow);
                }
                if (deleteButton.activeSelf == false && commands.ContainsKey("activateDeleteButton")) {
                    if (commands["activateDeleteButton"].Split('\n')[0] == deleteButtonPassword) deleteButton.SetActive(true);
                    else MessagePrompt.messagePrompt.OpenPrompt("Action denied.", "Password mismatch.", CurrentWindow.currentWindow.ForeWindow);
                }
                if (flag581 == 0 && commands.ContainsKey("activateEditTextFile") && CurrentWindow.currentWindow.File == protagonist) {
                    if (taskStage >= 500 && commands["activateEditTextFile"].Split('\n')[0] == editTextFilePassword) taskStage = 580;
                    else MessagePrompt.messagePrompt.OpenPrompt("Action denied.", "Password mismatch.", CurrentWindow.currentWindow.ForeWindow);
                }
            }

        }

    }

    protected void PassParameters(int fromTask, int toTask, string folderString, FolderA folder, object info) {
        
        Story output = story.tasks[toTask];
        output.folderString = folderString;
        output.folder = folder;
        output.info[fromTask] = info;

    }

    private void AddDialogue(string[] lines) {

        for (int i = 0; i < lines.Length; i++) dialogue.Add(lines[i]);

    }

    private void ClearDialogue() {

        dialogue.Clear();

    }

}
