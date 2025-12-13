using UnityEngine;
using TMPro;
using System.Collections;
public class sun : MonoBehaviour
{
    public SunDialogueUI dialogueUI;


    [TextArea]
    public string introDialogue =
        "Keep the dog alive. If you fail, something terrible will happen.";

    [TextArea]
    public string deathDialogue =
        "welp.";

    private bool introPlayed = false;

    void Start()
    {
        PlayIntro();
    }

    void PlayIntro()
    {
        if (introPlayed) return;
        introPlayed = true;

        dialogueUI.Speak(introDialogue);
    }

    public void OnDogDeath()
    {
        dialogueUI.Speak(deathDialogue);
    }
    public TextMeshProUGUI dialogueText;
    public float textDuration = 4f;

    void Awake()
    {
        dialogueText.gameObject.SetActive(false);
    }

    public void Speak(string line)
    {
        StopAllCoroutines();
        StartCoroutine(ShowText(line));
    }

    IEnumerator ShowText(string line)
    {
        dialogueText.text = line;
        dialogueText.gameObject.SetActive(true);

        yield return new WaitForSeconds(textDuration);

        dialogueText.gameObject.SetActive(false);
    }
}
