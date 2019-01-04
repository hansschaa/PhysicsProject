using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using DG.Tweening;

public class CAxeController : MonoBehaviour
{
    public float duration;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Tween rotation = transform.DOLocalRotate(new Vector3(270, 90, 0), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);  
    }


}
