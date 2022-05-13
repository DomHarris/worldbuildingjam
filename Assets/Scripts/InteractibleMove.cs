using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleMove : MonoBehaviour
{
    public enum WaypointType { WorldPos, LocalPos, CinemachineTrack, Transform } 
    [Serializable]
    public class MoveWaypoint
    {
        public Vector3 position;
        public Ease easing;
        public float time;
        public UnityEvent onStart;
        public UnityEvent onEnd;
        public WaypointType type;
        public CinemachinePath track;
        public Transform transform;
    }
    
    [SerializeField] private Transform thingToMove;
    [SerializeField] private List<MoveWaypoint> points;
    
    public void Interact()
    {
        var sequence = DOTween.Sequence();
        foreach (var waypoint in points)
        {
            switch (waypoint.type)
            {
                case WaypointType.WorldPos: 
                    sequence.Append(thingToMove.DOMove(waypoint.position, waypoint.time).SetEase(waypoint.easing).OnStart(() => waypoint.onStart.Invoke()).OnComplete(() => waypoint.onEnd.Invoke()));
                    break;
                case WaypointType.LocalPos:
                    sequence.Append(thingToMove.DOLocalMove(waypoint.position, waypoint.time).SetEase(waypoint.easing).OnStart(() => waypoint.onStart.Invoke()).OnComplete(() => waypoint.onEnd.Invoke()));
                    break;
                case WaypointType.CinemachineTrack:
                    sequence.Append(
                            DOVirtual.Float(0, waypoint.track.PathLength, waypoint.time, val =>
                            {
                                thingToMove.transform.position = waypoint.track.EvaluatePosition(val);
                                thingToMove.transform.rotation = waypoint.track.EvaluateOrientation(val);
                            }).SetEase(waypoint.easing).OnStart(() => waypoint.onStart.Invoke()).OnComplete(() => waypoint.onEnd.Invoke())
                        );
                    break;
                case WaypointType.Transform:
                    sequence.Append(thingToMove.DOMove(waypoint.transform.position, waypoint.time).SetEase(waypoint.easing).OnStart(() => waypoint.onStart.Invoke()).OnComplete(() => waypoint.onEnd.Invoke()));
                    break;
            }
        }
    }
}