using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanWithMouseBehaviour : MonoBehaviour
{
    //You can set it to whatever you want to transform
    [Tooltip("The transform of whatever we want to manipulate on X")]
    [SerializeField] private Transform _transX = null;
    [Tooltip("The transform of whatever we want to manipulate on Y")]
    [SerializeField] private Transform _transY = null;
    [SerializeField] private LeanBehaviour _leanBehaviourY = LeanBehaviour.UpDown;
    [Tooltip("The span in either direction before mouse clamps")]
    [SerializeField] private Vector2 _mouseSpan = Vector2.one;
    [SerializeField] private bool _flipX = true;
    [Tooltip("Mouse input scalar")]
    [SerializeField] private Vector2 _mouseSensitivity = Vector2.one;
    [SerializeField] private Vector3 _upDirection = new Vector3(0f, 1f, 0f);
    [SerializeField] private KeyCode _conditionalKey = KeyCode.LeftShift;

    //the current mouse input
    private Vector2 _mouseInputVec2 = Vector2.zero;
    private Vector3 _offsetY = Vector3.zero;

    private void Start()
    {
        _offsetY = _transY.localPosition;
    }

    void Update()
    {
        //We want to get input
        _mouseInputVec2 += GetRawMouseInputScaled(_conditionalKey);
        _mouseInputVec2 = ClampToMouseSpan(_mouseInputVec2);
        SnapBackWhenConditionalKeyUp(_conditionalKey);
        ApplyLean();
        Debug.Log(_transY.localPosition);
    }

    Vector2 GetRawMouseInputScaled(KeyCode conditionalKey = KeyCode.None)
    {
        //while we hold shift
        Vector2 rawInput = Vector2.zero;

        if (conditionalKey == KeyCode.None || Input.GetKey(conditionalKey))
        {
            //record mouse input
            rawInput.x = Input.GetAxisRaw("Mouse X");
            if (_flipX)
            {
                rawInput.x *= -1;
            }
            rawInput.y = Input.GetAxisRaw("Mouse Y");

            rawInput.Scale(_mouseSensitivity);
        }

        return rawInput;
    }

    Vector2 ClampToMouseSpan(Vector2 vector2)
    {
        vector2.x = Mathf.Clamp(vector2.x, -_mouseSpan.x, _mouseSpan.x);
        vector2.y = Mathf.Clamp(vector2.y, -_mouseSpan.y, _mouseSpan.y);

        return vector2;
    }

    void SnapBackWhenConditionalKeyUp(KeyCode conditionalKey = KeyCode.None)
    {
        if (conditionalKey != KeyCode.None && Input.GetKeyUp(conditionalKey))
        {
            _mouseInputVec2 = Vector2.zero;
        }
    }

    void ApplyLean()
    {
        _transX.localRotation = Quaternion.Euler(0f, 0f, _mouseInputVec2.x);
        _transY.localPosition = new Vector3(0f, _mouseInputVec2.y, 0f) + _offsetY;
    }

    enum LeanBehaviour
    {
        None,
        UpDown
    }
}
