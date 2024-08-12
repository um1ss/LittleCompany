public class TestInteractable : Interactable
{
    public override void OnInteract()
    {
        print("Я потрогал куб " + gameObject.name);
    }
}