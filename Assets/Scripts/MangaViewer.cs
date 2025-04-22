using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MangaViewer : MonoBehaviour
{
    public Image[] displayImages;
    public float fadeDuration = 0.5f;

    private int index = 0;
    private bool isTransitioning = false;

    void Start()
    {
        foreach (Image img in displayImages)
        {
            img.gameObject.SetActive(false);
            EnsureCanvasGroup(img);
            img.GetComponent<CanvasGroup>().alpha = 0f;
        }

        if (displayImages.Length > 0)
        {
            displayImages[0].gameObject.SetActive(true);
            StartCoroutine(FadeIn(displayImages[0]));
        }
    }

    void Update()
    {
        if (!isTransitioning && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            ShowNextPanel();
        }
    }

    public void ShowNextPanel()
    {
        if (index + 1 < displayImages.Length)
        {
            index++;
            Image nextImage = displayImages[index];
            nextImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn(nextImage));
        }
        else
        {
            Debug.Log("All panels shown.");
            // Optional: Add a delay and auto load, or show a UI button
        }
    }

    public void OnPanelClick()
    {
        SceneManager.LoadScene("1.MainMenu");
    }

    private IEnumerator FadeIn(Image img)
    {
        isTransitioning = true;
        CanvasGroup cg = img.GetComponent<CanvasGroup>();
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1;
        isTransitioning = false;
    }

    private void EnsureCanvasGroup(Image img)
    {
        if (!img.TryGetComponent(out CanvasGroup cg))
        {
            img.gameObject.AddComponent<CanvasGroup>();
        }
    }
}
