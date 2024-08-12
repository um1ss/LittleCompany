using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    public virtual void Awake()
    {
        gameObject.layer = 9;
    }

    public abstract void OnInteract();

    public virtual void OnFocus()
    {
        print("Нажмите 'Е' чтобы взаимодействовать с предметом " + gameObject.name);
    }

    public virtual void OnLoseFocus()
    {
        
    }
}
