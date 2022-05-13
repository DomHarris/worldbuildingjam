using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InteractibleMove.MoveWaypoint))]
public class MoveWaypointDrawer : PropertyDrawer
{
    private static Dictionary<string, float> _propertyHeights;
    private float height;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Math.Abs(position.height - 1) < Mathf.Epsilon)
            return;
        if (_propertyHeights == null) _propertyHeights = new Dictionary<string, float>();
        
        if (!_propertyHeights.ContainsKey(property.propertyPath))
            _propertyHeights.Add(property.propertyPath, 0f);
        
        var height = _propertyHeights[property.propertyPath];
        height = 18f;
        var currentPos = position;
        currentPos.height = height;
        
        var type = (InteractibleMove.WaypointType) property.FindPropertyRelative("type").enumValueIndex;
        var content = type switch
        {
            InteractibleMove.WaypointType.WorldPos => property.FindPropertyRelative("position").vector3Value.ToString(),
            InteractibleMove.WaypointType.LocalPos => property.FindPropertyRelative("position").vector3Value.ToString(),
            InteractibleMove.WaypointType.CinemachineTrack => property.FindPropertyRelative("track").objectReferenceValue == null ? "unset" : property.FindPropertyRelative("track").objectReferenceValue.name,
            InteractibleMove.WaypointType.Transform => property.FindPropertyRelative("transform").objectReferenceValue == null ? "unset" : property.FindPropertyRelative("transform").objectReferenceValue.name,
            _ => throw new ArgumentOutOfRangeException()
        };
        property.isExpanded = EditorGUI.Foldout(currentPos, property.isExpanded, $"{type} {content}");
        
        if (!property.isExpanded)
        {
            _propertyHeights[property.propertyPath] = 18f;
            height = 18f;
            return;
        }
        
        currentPos.y += 18f;

        AddRelativeProperty(ref property, "type", 3f, ref currentPos, ref height);

        type = (InteractibleMove.WaypointType) property.FindPropertyRelative("type").enumValueIndex;
        switch (type)
        {
            case InteractibleMove.WaypointType.WorldPos:
                AddRelativeProperty(ref property, "position", 3f, ref currentPos, ref height);
                break;
            case InteractibleMove.WaypointType.LocalPos:
                AddRelativeProperty(ref property, "position", 3f, ref currentPos, ref height);
                break;
            case InteractibleMove.WaypointType.CinemachineTrack:
                AddRelativeProperty(ref property, "track", 3f, ref currentPos, ref height);
                break;
            case InteractibleMove.WaypointType.Transform:
                AddRelativeProperty(ref property, "transform", 3f, ref currentPos, ref height);
                break;
        }
        
        
        AddRelativeProperty(ref property, "time", 3f, ref currentPos, ref height);
        AddRelativeProperty(ref property, "easing", 3f, ref currentPos, ref height);
        
        AddRelativeProperty(ref property, "onStart", 3f, ref currentPos, ref height);
        AddRelativeProperty(ref property, "onEnd", 3f, ref currentPos, ref height);
        _propertyHeights[property.propertyPath] = height;
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (_propertyHeights == null)
            _propertyHeights = new Dictionary<string, float>();
        if (!_propertyHeights.ContainsKey(property.propertyPath))
            _propertyHeights.Add(property.propertyPath, 18f);

        return _propertyHeights[property.propertyPath];
    }
    
    public static void AddRelativeProperty(
        ref SerializedProperty property,
        string name,
        float padding,
        ref Rect currentPos,
        ref float height)
    {
        var propertyHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(name));
        EditorGUI.PropertyField(currentPos, property.FindPropertyRelative(name), true);
        currentPos.y += propertyHeight + padding;
        height += propertyHeight + padding;
    }

}