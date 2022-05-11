using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextRevealEffect : MonoBehaviour
{
    [SerializeField] private int revealSpeed = 4;
    [SerializeField] private Color fadeColor;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private GameObject next;
    [SerializeField] private CanvasGroup canvas;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator ShowText(string[] data, Action onComplete)
    {
        // show the canvas group
        canvas.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.2f);
        
        // for each page
        foreach (var page in data)
        {
            // reset all the data and objects
            _text.text = secondText.text = page;
            next.SetActive(false);
            _text.enabled = true;
            _text.color = Color.white;
            secondText.enabled = false;
            // wait a frame so the text info is correct
            yield return null;
            // grab the number of visible characters and start a counter
            var visibleCharacters = _text.textInfo.characterCount;
            var counter = 0;

            // while there are still characters to reveal, and no key was pressed
            while (counter < visibleCharacters && !KeyPressed())
            {
                // set the maximum amount of characters to see
                _text.maxVisibleCharacters = counter;
                counter += revealSpeed;
                // wait a frame
                yield return null;
            }

            // show all characters
            _text.maxVisibleCharacters = visibleCharacters;
            // fade to blue
            _text.DOColor(fadeColor, 0.2f);
            yield return new WaitForSeconds(0.1f);

            // shakey shakey
            transform.DOShakePosition(0.3f, 5, 20);
            yield return null;
            // hide the alien text
            _text.enabled = false;
            // show the human text 
            secondText.enabled = true;
            // show a button so the player knows there is more
            next.SetActive(true);
            
            // reset the alien text
            _text.maxVisibleCharacters = 0;

            // wait for a keypress
            while (!KeyPressed())
                yield return null;
            
            // ^ loop back round to the next page or exit if there are no more pages
        }

        // hide the canvas
        canvas.DOFade(0, 0.5f);
        onComplete?.Invoke();
    }

    private bool KeyPressed()
    {
        return !(!Keyboard.current[Key.Space].wasPressedThisFrame && !Keyboard.current[Key.Enter].wasPressedThisFrame && !Keyboard.current[Key.E].wasPressedThisFrame && !Keyboard.current[Key.X].wasPressedThisFrame);
    }
}