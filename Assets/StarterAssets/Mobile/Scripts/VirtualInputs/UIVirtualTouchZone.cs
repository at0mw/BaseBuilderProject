using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIVirtualTouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    [Header("Rect References")] public RectTransform containerRect;

    public RectTransform handleRect;

    [Header("Settings")] public bool clampToMagnitude;

    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    [Header("Output")] public Event touchZoneOutputEvent;

    private Vector2 currentPointerPosition;

    //Stored Pointer Values
    private Vector2 pointerDownPosition;

    private void Start() {
        SetupHandle();
    }

    public void OnDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out currentPointerPosition);

        var positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);

        var clampedPosition = ClampValuesToMagnitude(positionDelta);

        var outputPosition = ApplyInversionFilter(clampedPosition);

        OutputPointerEventValue(outputPosition * magnitudeMultiplier);
    }

    public void OnPointerDown(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out pointerDownPosition);

        if (handleRect) {
            SetObjectActiveState(handleRect.gameObject, true);
            UpdateHandleRectPosition(pointerDownPosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        pointerDownPosition = Vector2.zero;
        currentPointerPosition = Vector2.zero;

        OutputPointerEventValue(Vector2.zero);

        if (handleRect) {
            SetObjectActiveState(handleRect.gameObject, false);
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    private void SetupHandle() {
        if (handleRect) SetObjectActiveState(handleRect.gameObject, false);
    }

    private void OutputPointerEventValue(Vector2 pointerPosition) {
        touchZoneOutputEvent.Invoke(pointerPosition);
    }

    private void UpdateHandleRectPosition(Vector2 newPosition) {
        handleRect.anchoredPosition = newPosition;
    }

    private void SetObjectActiveState(GameObject targetObject, bool newState) {
        targetObject.SetActive(newState);
    }

    private Vector2 GetDeltaBetweenPositions(Vector2 firstPosition, Vector2 secondPosition) {
        return secondPosition - firstPosition;
    }

    private Vector2 ClampValuesToMagnitude(Vector2 position) {
        return Vector2.ClampMagnitude(position, 1);
    }

    private Vector2 ApplyInversionFilter(Vector2 position) {
        if (invertXOutputValue) position.x = InvertValue(position.x);

        if (invertYOutputValue) position.y = InvertValue(position.y);

        return position;
    }

    private float InvertValue(float value) {
        return -value;
    }

    [Serializable]
    public class Event : UnityEvent<Vector2> { }
}