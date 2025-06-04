using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections;

/// <summary>
/// Manages the visual and interactive behavior of a popup UI.
/// Handles different popup types (OK, OKCancel, YesNo, Custom) and supports fade-in/fade-out animations.
/// </summary>
public class PopupView : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button confirmButton;
    public Button cancelButton;
    public Button closeButton;
    public TMP_InputField InputField;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI cancelText;

    [Header("UI References :")]
    [SerializeField] private GraphicRaycaster uiCanvasGraphicRaycaster;
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [Header("Popup Fade Duration :")]
    [Range(.1f, .8f)][SerializeField] private float fadeInDuration = .3f;
    [Range(.1f, .8f)][SerializeField] private float fadeOutDuration = .3f;

    /// <summary>
    /// Initializes canvas references and binds close button functionality.
    /// </summary>
    private void Start()
    {
        //uiCanvasGraphicRaycaster = transform.parent.GetComponent<GraphicRaycaster>();
        //uiCanvasGroup = transform.parent.GetComponent<CanvasGroup>();
        closeButton.onClick.AddListener(() => { StartCoroutine(FadeOut(fadeOutDuration)); });
    }

    /// <summary>
    /// Configures the popup based on parameters and activates it.
    /// </summary>
    /// <param name="title">Popup title text</param>
    /// <param name="message">Popup message text</param>
    /// <param name="type">Type of popup (OK, OKCancel, etc.)</param>
    /// <param name="onConfirm">Callback when confirm is clicked</param>
    /// <param name="onCancel">Callback when cancel is clicked</param>
    /// <param name="Button1Text">Custom text for confirm button (used only for Custom type)</param>
    /// <param name="Button2Text">Custom text for cancel button (used only for Custom type)</param>
    /// <param name="RequireCloseBtn">Show/hide close (X) button</param>
    public void Setup(string title, string message, PopupType type, Action onConfirm, Action onCancel, bool RequireCloseBtn = false, string Button1Text="", string Button2Text="")
    {
        titleText.text = title;
        messageText.text = message;

        // Configure buttons and text based on popup type
        switch (type)
        {
            case PopupType.OK:
                confirmText.text = "OK";
                cancelButton.gameObject.SetActive(false);
                break;
            case PopupType.OKCancel:
                confirmText.text = "OK";
                cancelText.text = "Cancel";
                cancelButton.gameObject.SetActive(true);
                break;
            case PopupType.YesNo:
                confirmText.text = "Yes";
                cancelText.text = "No";
                cancelButton.gameObject.SetActive(true);
                break;
            case PopupType.Custom:
                confirmText.text = Button1Text;
                cancelText.text = Button2Text;
                cancelButton.gameObject.SetActive(true);
                break;
            case PopupType.InputFieldCustom:
                confirmText.text = Button1Text;
                cancelText.text = Button2Text;
                //input Feild
                
                cancelButton.gameObject.SetActive(true);
                break;
        }

        closeButton.gameObject.SetActive(RequireCloseBtn);

        StartCoroutine(FadeIn(fadeInDuration));

        confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            StartCoroutine(FadeOut(fadeOutDuration));
        });

        cancelButton.onClick.AddListener(() =>
        {
            onCancel?.Invoke();
            StartCoroutine(FadeOut(fadeOutDuration));
        });
    }


    #region FadeIn_Out

    /// <summary>
    /// Fades the popup in by gradually increasing its alpha.
    /// </summary>
    private IEnumerator FadeIn(float duration)
    {
        uiCanvasGraphicRaycaster.enabled = true;
        yield return Fade(uiCanvasGroup, 0f, 1f, duration);
        uiCanvasGroup.interactable = true;
    }

    /// <summary>
    /// Fades the popup out by gradually decreasing its alpha.
    /// </summary>
    private IEnumerator FadeOut(float duration)
    {
        yield return Fade(uiCanvasGroup, 1f, 0f, duration);
        uiCanvasGroup.interactable = false;
        uiCanvasGraphicRaycaster.enabled = false;
    }

    /// <summary>
    /// Performs a smooth fade transition on the specified CanvasGroup.
    /// </summary>
    private IEnumerator Fade(CanvasGroup cGroup, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float alpha = startAlpha;

        if (duration > 0f)
        {
            //Anim start
            while (alpha != endAlpha)
            {
                alpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / duration);
                cGroup.alpha = alpha;

                yield return null;
            }
        }

        cGroup.alpha = endAlpha;
    }
    #endregion
}
