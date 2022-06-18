using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private float minTimeUntilWardenTrigger = 15;
    [SerializeField] private float maxTimeUntilWardenTrigger = 30;

    private float nextCheckupTime = 0;

    private void OnEnable() {
        GameEvents.OnWardenFinishedCheckup += HandleWardenFinishedCheckup;
        SetupNextCheckup();
    }

    private void OnDisable() {
        GameEvents.OnWardenFinishedCheckup -= HandleWardenFinishedCheckup;
    }

    private void Update() {
        if (Time.time > nextCheckupTime && nextCheckupTime != 0) {
            nextCheckupTime = 0;
            TriggerSlowWarden();
        }
    }

    private void HandleWardenFinishedCheckup() {
        SetupNextCheckup();
    }

    private void SetupNextCheckup() {
        nextCheckupTime = Time.time + UnityEngine.Random.Range(minTimeUntilWardenTrigger, maxTimeUntilWardenTrigger);
    }

    [ContextMenu("Trigger Slow Warden")]
    private void TriggerSlowWarden() {
        GameEvents.TriggerWardenSlow();
    }
}
