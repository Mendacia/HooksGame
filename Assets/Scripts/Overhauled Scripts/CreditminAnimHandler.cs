using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditminAnimHandler : MonoBehaviour
{
    [SerializeField] private string requestedAnimationName = null;
    private void Awake()
    {
        gameObject.GetComponent<Animator>().Play(requestedAnimationName);
    }
}
