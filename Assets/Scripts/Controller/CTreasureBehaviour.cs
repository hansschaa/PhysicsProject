using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTreasureBehaviour : MonoBehaviour
{
    #region "Events" 
    public delegate void PlaySound(int id, float pitch);
    public static event PlaySound OnPlaySound;
    
    public delegate void ShowUIButton(EUIButton euiButton);
    public static event ShowUIButton OnShowUIButton;
    
    public delegate void HideUIButton();
    public static event HideUIButton OnHideUIButton;
    #endregion

    public GameObject closedTreasure;
    public GameObject openTraseure;
    
    public void Open()
    {
        if (closedTreasure.activeInHierarchy)
        {
            closedTreasure.SetActive(false);
            openTraseure.SetActive(true);

            if (OnPlaySound != null)
                OnPlaySound(6, 1);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && openTraseure.activeInHierarchy)
        {
            OnShowUIButton(EUIButton.E);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && openTraseure.activeInHierarchy)
        {
            OnHideUIButton();
        }
    }
}
