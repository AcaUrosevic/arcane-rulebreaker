using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialOverlayUI : MonoBehaviour
{
    [Serializable]
    public struct Page
    {
        [TextArea(2, 8)] public string body;
    }

    [Header("Pages")]
    public List<Page> pages = new List<Page>();

    [Header("UI Refs")]
    public TextMeshProUGUI bodyTMP;
    public Text legacyBody;
    public Button playButton;
    public Button backToMenuButton;
    public Button nextButton;

    [Header("Scene Names")]
    public string gameSceneName = "GameScene";
    public string mainMenuSceneName = "MainMenu";

    [Header("Options")]
    public bool enableKeyboardShortcuts = true;
    public bool autoLabelNext = false;
    public TextMeshProUGUI nextButtonLabelTMP;
    public Text nextButtonLabelLegacy;

    int index = 0;

    void Awake()
    {
        if (nextButton) nextButton.onClick.AddListener(NextPage);
        if (playButton) playButton.onClick.AddListener(StartGame);
        if (backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

        if (bodyTMP) bodyTMP.raycastTarget = false;
        if (legacyBody) legacyBody.raycastTarget = false;
    }

    void Start()
    {
        if (pages == null || pages.Count == 0)
        {
            pages = new List<Page>
            {
                new Page { body =
@"Welcome to Arcane Rulebreaker.

You are a young wizard trapped inside the Tower of Rules. The tower bends to the law of three:
• 3 waves to survive,
• 3 minutes on the clock,
• 3 rules you must not break — but you may violate up to 3 times in total.

Find the rhythm: move, dash, dodge and cast at the right moment."},

    new Page { body =
@"Your tools of survival:

• Fireball (Left Click) — quick projectile dealing solid damage.
• Frost Orb (Right Click) — projectile that explodes and SLOWS enemies in an area.
• Hollow Purple (Q) — a combined strike (AoE damage + slow), available only when BOTH Fireball and Frost are on cooldown, with 3 uses per round.
• Dash (Space) — a short burst of speed to reposition or evade. You get 3 dashes per wave before it counts as violations."},

    new Page { body =
@"The three rules (and your three allowed violations):

• Do NOT use more than 3 dashes in a single wave.
• Do NOT stand still for 3 seconds.
• Do NOT step on hazard tiles.

Goal:
• Clear 3 waves in ~3 minutes. You have 3 total violations to spend. A fourth violation, running out of time, or dying will end the run.

Good luck, Rulebreaker."}
            };

        }
    
        index = 0;
        Refresh();
    }


    void Update()
    {
        if (!enableKeyboardShortcuts) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightArrow))
            NextPage();

        if (Input.GetKeyDown(KeyCode.Escape))
            BackToMenu();
    }

    public void NextPage()
    {
        if (index < pages.Count - 1)
        {
            index++;
            Refresh();
        }
        else
        {
            Refresh();
        }
    }

    void Refresh()
    {
        string body = pages != null && pages.Count > 0 ? pages[index].body : "";
        if (bodyTMP) bodyTMP.text = body;
        if (legacyBody) legacyBody.text = body;

        if (playButton) playButton.interactable = (index >= pages.Count - 1);

        if (autoLabelNext)
        {
            string label = (index < pages.Count - 1) ? "Next ▶" : "Done";
            if (nextButtonLabelTMP) nextButtonLabelTMP.text = label;
            if (nextButtonLabelLegacy) nextButtonLabelLegacy.text = label;
        }
    }

    public void StartGame()
    {
        if (!playButton || !playButton.interactable) return;
        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
        else
            Debug.LogWarning("[TutorialOverlayUI] gameSceneName nije podešen.");
    }

    public void BackToMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Debug.LogWarning("[TutorialOverlayUI] mainMenuSceneName nije podešen.");
    }
}
