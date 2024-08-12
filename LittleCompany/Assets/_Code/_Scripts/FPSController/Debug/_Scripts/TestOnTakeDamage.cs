using UnityEngine;

public class TestOnTakeDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPlayerController.OnTakeDamage(15);
        }
    }
}
