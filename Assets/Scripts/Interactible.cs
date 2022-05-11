using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent pickedUp;
    [SerializeField] private string verb = "pick up";
    public string Verb => verb;
    public void PickUp()
    {
        pickedUp?.Invoke();
    }
}