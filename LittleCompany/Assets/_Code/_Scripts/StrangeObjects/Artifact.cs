using System;
using UnityEngine;

public class Artifact : PickUpItem, IStrangeObjects
{
    private float InitialSpeedPlayer;
    private float SpeedPlayerFluenceArtifact; 

    public override void OnPickUp(Transform container)
    {
        base.OnPickUp(container);
        InitialSpeedPlayer = _player._walkSpeed;
        SpeedPlayerFluenceArtifact = 0.5f;
    }

    public override void OnUse()
    {
    }

    public void OnAffect()
    {
        _player._walkSpeed = SpeedPlayerFluenceArtifact;
    }

    public void OnUnAffect()
    {
        _player._walkSpeed = InitialSpeedPlayer;
    }
}
