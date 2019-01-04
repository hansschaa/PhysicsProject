using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CMushroomBehaviour : MonoBehaviour
{
    public delegate void PlaySound(int id, float pitch);
    public static event PlaySound OnPlaySound;
    
    
    public float pitch;
    
    public void InBounce()
    {
        print("InBounce");
        DOTween.Sequence().Append(transform.DOScaleY(0.3f, .5f).SetLoops(2, LoopType.Yoyo));

        if (OnPlaySound != null)
            OnPlaySound(3, pitch);

    }
}
