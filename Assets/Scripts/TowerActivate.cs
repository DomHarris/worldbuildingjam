using DG.Tweening;
using UnityEngine;

public class TowerActivate : MonoBehaviour
{
    public void Activate()
    {
        
        GetComponent<Renderer>().material.DOFloat(1f,"_CircuitEnabledAmount", 1f);
    }
}