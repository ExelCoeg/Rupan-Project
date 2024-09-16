using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
{
    
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    private Queue<DialogueLine> lines;
    public UIDialog  uiDialog;
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;
 
    public Animator animator;
    public Dialogue currentDialogue;

    public PlayerInputActions playerInputActions;
    public InputAction nextDialogueAction;

    public static event Action onDialogueEnd;
    public override void Awake()
    {
        base.Awake();
        lines = new Queue<DialogueLine>();
        playerInputActions = new PlayerInputActions();
    }
    public void StartDialogue(Dialogue dialogue)
    {

        print("Starting Dialogue");
        FindObjectOfType<Player>().DisableControls();

        currentDialogue = dialogue;
    
        isDialogueActive = true;
    
        uiDialog.Show();
 
        lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    void EndDialogue()
    {
        if(currentDialogue.playableDirectorTrigger != null){
            print("Play playable director");
            currentDialogue.playableDirectorTrigger.Play();
        }
        isDialogueActive = false;
        uiDialog.Hide();
        FindObjectOfType<Player>().EnableControls();
        onDialogueEnd?.Invoke();
    }

    private void OnEnable() {
        EnableNextDialogueAction();
        nextDialogueAction = playerInputActions.UI.NextDialogue;
        nextDialogueAction.performed += ctx => {
            if(isDialogueActive){
                DisplayNextDialogueLine();
            }
        };
    }
    private void OnDisable() {
        DisableNextDialogueAction();
    }
    public void DisableNextDialogueAction(){
        nextDialogueAction.Disable();
    }
    public void EnableNextDialogueAction(){
        nextDialogueAction.Enable();
    }
    
    public void OnDialougeEnd(){
        print("OnDialougeEnd");
    }

    public void DisableImage(){
        characterIcon.gameObject.SetActive(false);
    }
    public void EnableImage(){
        characterIcon.gameObject.SetActive(true);
    }
}