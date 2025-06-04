#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class PopupController : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the PopupController.
     /// </summary>
    public static PopupController Instance { get; private set; }

    [Header("Popup Configuration")]
    [Tooltip("Prefab used to create new popup instances.")]
    [SerializeField] private PopupView popupPrefab;

    [Tooltip("Parent transform where popups should be instantiated.")]
    [SerializeField] private Transform popupParent;

    [Tooltip("LazyFollow component to optionally follow the camera.")]
    [SerializeField] private LazyFollow lazyFollow;

    /// <summary>
    /// Unity callback used for validation in the editor.
    /// Displays warnings for unassigned serialized fields.
    /// </summary>
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (popupPrefab == null)
            Debug.LogWarning("[PopupController] Popup prefab is not assigned.", this);

        if (popupParent == null)
            Debug.LogWarning("[PopupController] Popup parent is not assigned.", this);

        if (lazyFollow == null)
            Debug.LogWarning("[PopupController] LazyFollow component is not assigned.", this);
#endif
    }


    /// <summary>
    /// Ensures that only one instance of this singleton exists in the scene.
    /// If a duplicate is found, it destroys the new instance.
    /// The valid instance is marked to persist across scenes.
    /// </summary>
    private void Awake()
    {
        // Check if another instance already exists and is not this one
        if (Instance != null && Instance != this)
        {
            // Destroy the duplicate GameObject to enforce singleton pattern
            Destroy(gameObject); // Destroy duplicate
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }

    /// <summary>
    /// Displays a popup with the specified configuration and optional callbacks.
    /// </summary>
    /// <param name="title">Title of the popup.</param>
    /// <param name="message">Main message of the popup.</param>
    /// <param name="type">Type of popup (OK, OKCancel, YesNo, etc.).</param>
    /// <param name="onConfirm">Callback invoked when user confirms.</param>
    /// <param name="onCancel">Callback invoked when user cancels.</param>
    /// <param name="RequireCloseBtn">used to add close button enable disable logic.</param>
    /// <param name="followCamera">If true, popup follows the camera using LazyFollow.(it depended on the XRTK Plugin)</param>
    /// <param name="Button1Text">Custom text for confirm button (used only for Custom type)</param>
    /// <param name="Button2Text">Custom text for cancel button (used only for Custom type)</param>
    /// 
    PopupView popupObj = null;
    public void ShowPopup(string title, string message, PopupType type, Action onConfirm = null, Action onCancel = null,bool RequireCloseBtn = false, bool followCamera = false, string Button1Text="", string Button2Text = "")
    {
        try
        {
            
            // Use cloned popup if available, otherwise instantiate a new one
            if (popupObj == null)
            {
                popupObj = Instantiate(popupPrefab, popupParent) ?? throw new Exception("Failed to instantiate popup prefab.");
            }
            else
            {
                popupObj.gameObject.SetActive(true);
            }

            // Set up the popup with data and callbacks
            popupObj.Setup(title, message, type, onConfirm, onCancel, RequireCloseBtn, Button1Text, Button2Text);

            // Control whether popup should follow the camera
            lazyFollow.positionFollowMode = followCamera ? LazyFollow.PositionFollowMode.Follow : LazyFollow.PositionFollowMode.None;

        }
        catch (Exception ex)
        {
            Debug.LogError($"[PopupController] Error showing popup: {ex.Message}\n{ex.StackTrace}");
        }
    }
}

/// <summary>
/// Represents the type of popup dialog to be displayed.
/// </summary>
public enum PopupType
{
    OK,
    OKCancel,
    YesNo,
    Custom, // for future expansion
    InputFieldCustom
}
