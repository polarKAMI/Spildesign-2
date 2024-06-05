using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LogMenu : MonoBehaviour
{
    [SerializeField] GameObject logMenu;
    [SerializeField] PauseMenu pauseMenu;
    public GameObject[] categoryBorders;

    private int selectedIndex = 0;
    private int entryIndex = 0;
    public bool entryList = false;
    public bool logSelected = false;

    [SerializeField] GameObject logEntryPrefab;
    [SerializeField] Transform viewportContent;
    [SerializeField] ScrollRect scrollRect;

    [Header("Description Box Elements")]
    public TMP_Text specsTXT;
    public TMP_Text descTXT;
    public TMP_Text logNameTXT;
    public Image itemImage;
    public Image logSelectedBorder;
    public float scrollSpeed = 0.1f;
    [SerializeField] private RectTransform viewport;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void Pause()
    {
        logMenu.SetActive(false);
        pauseMenu.Menu();
        Time.timeScale = 0;
    }

    public void LogOpen()
    {
        entryList = false;
        logSelected = false;
        logMenu.SetActive(true);
        ResetOptions();
        selectedIndex = 0; // Ensure the selected index is reset to 0
        SelectOption(selectedIndex);
        DestroyLogs();
        DescriptionBoxClear();
        DisplayLogs(LogManager.GetLogsForCategory(selectedIndex));
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.logInputMapping);
    }

    void SelectOption(int index)
    {
        ResetOptions();
        categoryBorders[index].SetActive(true);
    }

    void ResetOptions()
    {
        foreach (var border in categoryBorders)
        {
            border.SetActive(false);
        }
    }

    public void ChangeSelectedIndex(int changeAmount)
    {
        int newIndex = selectedIndex + changeAmount;

        if (newIndex < 0)
        {
            newIndex = categoryBorders.Length - 1;
        }
        else if (newIndex >= categoryBorders.Length)
        {
            newIndex = 0;
        }

        selectedIndex = newIndex;
        SelectOption(selectedIndex);
        DestroyLogs();
        DisplayLogs(LogManager.GetLogsForCategory(selectedIndex));
    }

    public void SelectCategory()
    {
        entryList = true;
        HighlightLogEntry(0); // Select the first log entry
    }

    private void DisplayLogs(List<LogSO> logs)
    {
        // Clear existing entries
        DestroyLogs();

        // Instantiate new log entries
        foreach (var log in logs)
        {
            GameObject logEntry = Instantiate(logEntryPrefab, viewportContent);
            TMP_Text logText = logEntry.GetComponentInChildren<TMP_Text>();
            logText.text = log.Name;
        }
    }

    private void DestroyLogs()
    {
        foreach (Transform child in viewportContent)
        {
            Destroy(child.gameObject);
        }
    }
    private void HighlightLogEntry(int index)
    {
        if (viewportContent.childCount > index)
        {
            Transform logEntry = viewportContent.GetChild(index);
            Transform background = logEntry.Find("Background");
            if (background != null)
            {
                GameObject border = background.Find("border")?.gameObject;
                if (border != null)
                {
                    border.SetActive(true);
                }
            }
        }
        DescriptionBoxDisplay(index);
    }

    private void DescriptionBoxDisplay(int index)
    {
        if (viewportContent.childCount > index)
        {
            // Get the list of logs for the selected category
            List<LogSO> logs = LogManager.GetLogsForCategory(selectedIndex);

            // Ensure the index is valid
            if (index >= 0 && index < logs.Count)
            {
                LogSO log = logs[index];

                // Display details in description box
                specsTXT.text = log.Specs;
                descTXT.text = log.Description;
                logNameTXT.text = log.Name;
                itemImage.sprite = log.LogImage;

                float contentHeight = LayoutUtility.GetPreferredHeight(descTXT.rectTransform);
                RectTransform descRT = descTXT.rectTransform;
                descRT.sizeDelta = new Vector2(descRT.sizeDelta.x, contentHeight);

                descRT.anchoredPosition = new Vector2(descRT.anchoredPosition.x, 0);
            }
            CanvasGroup canvasGroup = itemImage.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
    }

    public void SelectLog()
    {
        logSelected = true;
        if (logSelectedBorder != null)
        {
            logSelectedBorder.gameObject.SetActive(true);
        }
    }

    public void DeselectLog()
    {
        logSelected = false;
        if (logSelectedBorder != null)
        {
            logSelectedBorder.gameObject.SetActive(false);
        }
    }

    private void DescriptionBoxClear()
    {
        specsTXT.text = null; descTXT.text = null; logNameTXT.text = null; itemImage.sprite = null;
        CanvasGroup canvasGroup = itemImage.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void DeselectCategory()
    {
        entryList = false;
        DescriptionBoxClear();
        foreach (Transform child in viewportContent)
        {
            Transform background = child.Find("Background");
            if (background != null)
            {
                GameObject border = background.Find("border")?.gameObject;
                if (border != null)
                {
                    border.SetActive(false);
                }
            }
        }
    }

    public void ChangeSelectedEntry(int changeAmount)
    {
        int newIndex = entryIndex + changeAmount;

        newIndex = (newIndex + viewportContent.childCount) % viewportContent.childCount;

        DisableBorderOnEntry(entryIndex);

        entryIndex = newIndex;

        HighlightLogEntry(entryIndex);
        DescriptionBoxDisplay(entryIndex);

    }

    private void DisableBorderOnEntry(int index)
    {
        if (viewportContent.childCount > index)
        {
            Transform logEntry = viewportContent.GetChild(index);
            Transform background = logEntry.Find("Background");
            if (background != null)
            {
                GameObject border = background.Find("border")?.gameObject;
                if (border != null)
                {
                    border.SetActive(false);
                }
            }
        }
    }
    public void ScrollContent(int direction)
    {
        float scrollSpeed = 0.1f; // Adjust this value to control the speed of scrolling
        float newY = scrollRect.verticalNormalizedPosition + direction * scrollSpeed;
        newY = Mathf.Clamp01(newY); // Ensure the value stays between 0 and 1
        scrollRect.verticalNormalizedPosition = newY;
    }
}