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
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = settingPanel.GetComponent<CanvasGroup>();
        _animator = settingPanel.GetComponent<Animator>();
        _animator.enabled = false;

    }

    void Start()
    {
        LoadVolume();

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

        float musicVol = Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume", 1.0f), 0.0001f, 1f);
        float ambientVol = Mathf.Clamp(PlayerPrefs.GetFloat("ambientVolume", 1.0f), 0.0001f, 1f);
        float sfxVol = Mathf.Clamp(PlayerPrefs.GetFloat("sfxVolume", 1.0f), 0.0001f, 1f);

        musicSlider.value = musicVol;
        ambientSlider.value = ambientVol;
        sfxSlider.value = sfxVol;

        Debug.Log($"Applying volumes - Music: {musicVol}, Ambient: {ambientVol}, SFX: {sfxVol}");

        myMixer.SetFloat("Music", Mathf.Log10(musicVol) * 20);
        myMixer.SetFloat("Ambient", Mathf.Log10(ambientVol) * 20);
        myMixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20);
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
        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

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

        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Enable interaction after animation
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Time.timeScale = 0;
    }

    public void Resume()
    {
        CloseSettingsPanel();
    }
}
