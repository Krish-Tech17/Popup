using UnityEngine;

public class PopupTest : MonoBehaviour
{
   

    public void ShowOKPopup()
    {
        PopupController.Instance.ShowPopup(
            title: "Information",
            message: "This is an OK popup.",
            type: PopupType.OK,
            onConfirm: () => OkClicked(),
            followCamera: true
        );
    }

    public void ShowOKCancelPopup()
    {
        PopupController.Instance.ShowPopup(
            title: "Confirmation",
            message: "Do you want to continue?",
            type: PopupType.OKCancel,
            onConfirm: () => OkClicked(),
            onCancel: () => CancelClicked(),
            followCamera: false
        );
    }

    public void ShowYesNoPopup()
    {
        PopupController.Instance.ShowPopup(
            title: "Exit",
            message: "Are you sure you want to exit?",
            type: PopupType.YesNo,
            onConfirm: () => YesClicked(),
            onCancel: () => NoClicked(),
            RequireCloseBtn: true
        );
    }

    public void ShowCustomPopup()
    {
        PopupController.Instance.ShowPopup(
            title: "Custom Choice",
            message: "Choose an option below.",
            type: PopupType.Custom,
            onConfirm: () => Option1Clicked(),
            onCancel: () => Option2Clicked(),
            RequireCloseBtn: true,
            Button1Text: "Option 1",
            Button2Text: "Option 2"
        );
    }

    void OkClicked()
    {
        Debug.Log("OK Clicked");
    }
    void YesClicked()
    {
        Debug.Log("Yes Clicked");
    }
    void NoClicked()
    {
        Debug.Log("No Clicked");
    }
    void CancelClicked()
    {
        Debug.Log("Cancel Clicked");
    }
    void Option1Clicked()
    {
        Debug.Log("Option 1 Clicked");
    }
    void Option2Clicked()
    {
        Debug.Log("Option 2 Clicked");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
