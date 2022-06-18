using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction Combo")]
public class InteractionCombo : ScriptableObject
{
    public InteractableData InteractableOne = default;
    public InteractableData InteractableTwo = default;
    public InteractableData Result = default;
    public AudioClip CombineSound = default;
    public List<Sprite> HintImageSequence = new List<Sprite>();
}
