using UnityEngine;

public class TestPickUpItem : PickUpItem
{
    [SerializeField] private Light _light;

    public override void OnUse()
    {
        _light.enabled = !_light.enabled;
    }
}
