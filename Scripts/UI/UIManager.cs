using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject currentMoney;
    [SerializeField] private GameObject timer;

    [SerializeField] private GameObject tutorialParent;
    [SerializeField] private GameObject tutorialEnabler;
    [SerializeField] private List<GameObject> tutorialPanels = new List<GameObject>();
    [SerializeField] private GameObject currentTutorialPanel = null;

    // Start is called before the first frame update
    void Awake()
    {
        tutorialEnabler.SetActive(true);

        TavernInventoryManager.instance.onMoneyChanged += UpdateCurrentMoney;
        MapManager.instance.onWorkTimeUpdated += UpdateTimer;
        MapManager.instance.onTutorialStatusChanged += SetTutorialPanelStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void SetTutorialPanelStatus(bool sts)
    {
        tutorialEnabler.SetActive(false);
        tutorialParent.SetActive(sts);
        LoadTutorialPanel(0);
    }

    public void LoadTutorialPanel(int panelIdx)
    {
        if(panelIdx < 0) // if given panel index is less than 0
            return; // do nothing

        if(currentTutorialPanel != null)
            currentTutorialPanel.SetActive(false); // disable the current panel;
        if(panelIdx >= tutorialPanels.Count) // check if given index is OUT of range
        {
            MapManager.instance.EnableTutorial(false); // disable the tutorial
            return;
        }
        tutorialPanels[panelIdx].SetActive(true); // else load tutorial panel with given index;
        currentTutorialPanel = tutorialPanels[panelIdx]; // set the current loaded panel to the panel with given index
    }

    public void LoadNextTutorialPanel()
    {
        int currPanelIdx = tutorialPanels.IndexOf(currentTutorialPanel); // get the index of the current panel
        LoadTutorialPanel(currPanelIdx + 1);
    }

    public void LoadPreviousTutorialPanel()
    {
        int currPanelIdx = tutorialPanels.IndexOf(currentTutorialPanel); // get the index of the current panel
        LoadTutorialPanel(currPanelIdx - 1);
    }

    public void Resume() // resume the game
    {
        pauseMenu.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        GameSceneManager.instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        GameSceneManager.instance.QuitGame();
    }

    private void UpdateCurrentMoney(int currCash)
    {
        currentMoney.GetComponent<TextMeshProUGUI>().text = $""; // reset the current cash displayed in the UI
        currentMoney.GetComponent<TextMeshProUGUI>().text = $"{currCash}"; // display the current cash to the UI
    }

    private void UpdateTimer(float progress)
    {
        timer.GetComponent<Image>().fillAmount = progress; // update the remaining work time
    }
}
