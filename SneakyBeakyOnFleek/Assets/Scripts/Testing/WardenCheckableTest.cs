using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenCheckableTest : MonoBehaviour, WardenCheckable
{
    public bool IsInBed => isInBed;

    [SerializeField] private bool isInBed;
}
