using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractibleDialogue : MonoBehaviour
{
    private static TextRevealEffect _reveal;   
    [SerializeField, TextArea] private string[] data;
    private bool _reading = false;

    public event Action<InteractibleDialogue> OnCollected;

    private void OnEnable()
    {
        if (_reveal == null)
            _reveal = FindObjectOfType<TextRevealEffect>();
    }

    public void Interact()
    {
        if (_reading)
            return;
        
        _reading = true;
        var pos = transform.position;
        transform.position = new Vector3(0, -500, 0);
        _reveal.StartCoroutine(_reveal.ShowText(data, () =>
        {
            transform.position = pos;
            _reading = false;
            OnCollected?.Invoke(this);
        }));
    }
}