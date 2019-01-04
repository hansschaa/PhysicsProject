using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCanvasBehaviour : MonoBehaviour
{

    public GameObject SavePointText;
    public Sprite[] buttonsImages;
    public Image buttonui;

    private void OnEnable()
    {
        CKnigthBehaviour.OnShowSaveMessage += ShowSaveMessage;
        CKnigthBehaviour.OnShowUIButton += ShowUiButton;
        CKnigthBehaviour.OnHideUIButton += HideUiButton;
        CTreasureBehaviour.OnHideUIButton += HideUiButton;
        CTreasureBehaviour.OnShowUIButton += ShowUiButton;
    }

    private void OnDisable()
    {
        CKnigthBehaviour.OnShowSaveMessage -= ShowSaveMessage;
        CKnigthBehaviour.OnShowUIButton -= ShowUiButton;
        CKnigthBehaviour.OnHideUIButton -= HideUiButton;
        CTreasureBehaviour.OnHideUIButton -= HideUiButton;
        CTreasureBehaviour.OnShowUIButton -= ShowUiButton;
    }

    private void ShowSaveMessage()
    {
        print("Guardando");
        DOTween.Sequence().Append(SavePointText.GetComponent<TextMeshProUGUI>().DOFade(1, 1)).SetEase(Ease.Linear)
            .SetLoops(2, LoopType.Yoyo);
    }

    private void ShowUiButton(EUIButton euiButton)
    {
        buttonui.gameObject.SetActive(true);
        buttonui.sprite = buttonsImages[(int) euiButton];
        buttonui.SetNativeSize();
    }

    private void HideUiButton()
    {
        buttonui.gameObject.SetActive(false);
    }

}
