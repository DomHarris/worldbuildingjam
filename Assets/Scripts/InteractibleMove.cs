using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InteractibleMove : MonoBehaviour
{
    [Serializable]
    public struct MoveWaypoint
    {
        public Vector3 position;
        public Ease easing;
        public float time;
    }
    
    [SerializeField] private Transform thingToMove;
    [SerializeField] private List<MoveWaypoint> points;
    
    public void Interact()
    {
        var sequence = DOTween.Sequence();
        foreach (var waypoint in points)
            sequence.Append(thingToMove.DOMove(waypoint.position, waypoint.time).SetEase(waypoint.easing));
    }
}