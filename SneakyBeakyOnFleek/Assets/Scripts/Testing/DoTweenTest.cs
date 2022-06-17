using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(10, 5).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
