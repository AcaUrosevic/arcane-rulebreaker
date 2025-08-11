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
    public bool autoLabelNext = false;              // <- NOVO: default false
    public TextMeshProUGUI nextButtonLabelTMP;      // <- ostavi null ako ne želiš tekst
    public Text nextButtonLabelLegacy;

    int index = 0;

    void Awake()
    {
        if (nextButton) nextButton.onClick.AddListener(NextPage);
        if (playButton) playButton.onClick.AddListener(StartGame);
        if (backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

        // osiguraj da tekst ne blokira klik
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
    
    You take on the role of a young wizard trapped inside the Tower of Rules, where your only chance is to — carefully — break only three of them.
    Ahead of you lie three waves of enemies and just a little time to bring chaos under control.
    Master the rhythm: movement, dodging, and casting at the right moment."},
    
                new Page { body =
    @"Your powers:
    
    • Fireball (Left Click) — a fast projectile dealing solid damage.
    • Frost Orb (Right Click) — a projectile that explodes and SLOWS enemies in its radius.
    • Hollow Purple (Q) — the ultimate strike (damage + slow in AoE), AVAILABLE only when both Fireball and Frost Orb are on COOLDOWN and only 3× per round.
    
    Master the timing and combine arena space with your spells."},
    
                new Page { body =
    @"Rules (important for survival):
    • You may break EXACTLY 3 rules — the fourth one means Game Over.
    • Implemented restrictions:
      – Using Spell #2 (Frost) is forbidden → each use counts as a violation.
      – Do not stand still for more than 3 seconds (violation).
      – Do not step on red tiles (violation + damage).
      – Do not use dash more than 3 times per wave (violation).
    
    Goal:
    • Survive 3 waves for ~3 minutes. Clear the arena before time runs out and before breaking the 4th rule."
                }
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
            // ostani na poslednjoj
            Refresh();
        }
    }

    void Refresh()
    {
        // ispiši telo teksta
        string body = pages != null && pages.Count > 0 ? pages[index].body : "";
        if (bodyTMP) bodyTMP.text = body;
        if (legacyBody) legacyBody.text = body;

        // Play postaje dostupan na poslednjoj strani
        if (playButton) playButton.interactable = (index >= pages.Count - 1);

        // opcionalno: promena labela ako želiš (ostavi sve null = bez teksta)
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
