using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static Action TriggerWardenSlow = null;
    public static Action OnWardenFinishedCheckup = null;
    public static Action OnWardenSpottedPlayer = null;
    public static Action<InteractionCombo> OnComboExecuted = null;
}
