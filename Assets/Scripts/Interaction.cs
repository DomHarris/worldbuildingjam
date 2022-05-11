using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private CanvasGroup ui;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Key key = Key.E;
    private List<Interactible> _currentInteractables;
    
    private void Awake()
    {
        _currentInteractables = new List<Interactible>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactible>();
        if (interactable == null)
            return;

        if (interactable.HasInteracted) return;
        
        ui.DOFade(1, 0.5f);
        text.text = $"[ press '{key}' to {interactable.Verb} ]";
        _currentInteractables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<Interactible>();
        if (interactable == null)
            return;

        ui.DOFade(0, 0.5f);
        _currentInteractables.Remove(interactable);
    }

    private void Update ()
    { 
        if (!Keyboard.current[key].wasPressedThisFrame)
            return;

        foreach (var interactable in _currentInteractables)
            if (interactable != null && !interactable.HasInteracted)
                interactable.PickUp();
    }
}