using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    
    public void Alert(string msg)
    {
        message.text = msg;
        DOTween.Sequence()
            .Append(message.DOFade(1, 1))
            .Append(message.DOFade(0, 1))
            .Play();
    }
}
