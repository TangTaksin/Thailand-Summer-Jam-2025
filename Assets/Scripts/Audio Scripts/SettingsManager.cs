using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("===================Settings=================")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject settingPanel;

    private Animator _animator;
    private bool isPanelOpen = false;

    private void Awake()
    {
        _animator = settingPanel.GetComponent<Animator>();
        _animator.enabled = false;
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
    }

    private void Update()
    {
        // Toggle settings panel when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleSettingsPanel();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetAmbientVolume()
    {
        float volume = ambientSlider.value;
        myMixer.SetFloat("Ambient", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ambientVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        Debug.Log("Loading volume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1.0f);  // Default value 1.0f
        ambientSlider.value = PlayerPrefs.GetFloat("ambientVolume", 1.0f); // Default value 1.0f
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1.0f); // Default value 1.0f
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ToggleSettingsPanel()
    {
        if (isPanelOpen)
        {
            CloseSettingsPanel();
        }
        else
        {
            OpenSettingsPanel();
        }
    }

    public void OpenSettingsPanel()
    {
        StartCoroutine(DeactivatePanelBeforeAnimation());

    }

    public void CloseSettingsPanel()
    {
        _animator.SetTrigger("Close");
        StartCoroutine(DeactivatePanelAfterAnimation());
        Time.timeScale = 1;
    }

    private IEnumerator DeactivatePanelAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length);
        settingPanel.SetActive(false);
        _animator.enabled = false;
        isPanelOpen = false;
    }

    private IEnumerator DeactivatePanelBeforeAnimation()
    {
        settingPanel.SetActive(true);
        _animator.enabled = true;
        isPanelOpen = true;
        yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        CloseSettingsPanel();
    }
}
