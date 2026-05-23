using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private int totalLevel = 4;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Button resetButton;
    [SerializeField] private string[] levelSceneNames;
    public Image levelNotCleared;
    public Image isLevelCleared;

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false); // Hide at start
            Debug.Log("Hide Reset Button");
        }

        UnlockAllLevels();
        UpdateLevelSelectUI();
    }

    public void MarkLevelCleared(int levelIndex)
    {

        PlayerPrefs.SetInt("Level" + levelIndex + "_Cleared", 1);
        PlayerPrefs.Save();
        UpdateLevelSelectUI();

        if (AreAllLevelsCleared())
        {
            Debug.Log("🎉 All levels cleared!");
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            SceneManager.LoadScene("1.MainMenu");
        }
    }

    public void UpdateLevelSelectUI()
    {
        int clearedCount = 0;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;

            bool isCleared = PlayerPrefs.GetInt("Level" + levelIndex + "_Cleared", 0) == 1;
            if (isCleared) clearedCount++; 
            levelButtons[i].interactable = true;

            Image btnImage = levelButtons[i].GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = isCleared ? isLevelCleared.sprite : levelNotCleared.sprite;
            }
        }

        // Enable the reset button after all levels are unlocked
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(clearedCount == 4);
        }

    }

    private void UnlockAllLevels()
    {
        for (int i = 1; i <= totalLevel; i++)
        {
            PlayerPrefs.SetInt("Level" + i + "_Unlocked", 1);
        }
        PlayerPrefs.Save();
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }

    public void LoadLevelByName(string levelSceneNames)
    {

        SceneManager.LoadScene(levelSceneNames);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        UnlockAllLevels();
        UpdateLevelSelectUI();
    }

    public bool AreAllLevelsCleared()
    {
        for (int i = 1; i <= totalLevel; i++)
        {
            if (PlayerPrefs.GetInt("Level" + i + "_Cleared", 0) != 1)
            {
                return false;
            }
        }
        return true;


    }

}
