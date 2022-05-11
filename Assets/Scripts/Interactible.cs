using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent pickedUp;
    [SerializeField] private string verb = "pick up";
    [SerializeField] private bool onlyInteractsOnce;
    
    public string Verb => verb;

    private bool _hasInteracted = false;
    public bool HasInteracted => _hasInteracted;
    public void PickUp()
    {
        if (onlyInteractsOnce)
            _hasInteracted = true;
        pickedUp?.Invoke();
    }
}