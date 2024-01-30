using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseEvent : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler,
    IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool interactible = true;

    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onLeftButtonPressed;
    public UnityEvent onLeftButtonReleased;
    public UnityEvent onLeftButtonBeginDragging;
    public UnityEvent onLeftButtonDragging;
    public UnityEvent onLeftButtonEndDraging;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    public OnMouseEvent TransferToOtherOnMouseEvent;

    public void OnPointerEnter(PointerEventData pointEventData)
    {
        if(interactible)
            onEnter.Invoke();

        TransferToOtherOnMouseEvent?.OnPointerEnter(pointEventData);
    }

    public void OnPointerExit(PointerEventData pointEventData)
    {
        if (interactible)
            onExit.Invoke();

        TransferToOtherOnMouseEvent?.OnPointerExit(pointEventData);
    }

    public void OnPointerDown(PointerEventData pointEventData)
    {
        if (pointEventData.button == PointerEventData.InputButton.Left)
        {
            if (interactible)
                onLeftButtonPressed.Invoke();

            TransferToOtherOnMouseEvent?.OnPointerDown(pointEventData);
        }
    }

    public void OnPointerUp(PointerEventData pointEventData)
    {
        if (pointEventData.button == PointerEventData.InputButton.Left)
        {
            if (interactible)
                onLeftButtonReleased.Invoke();

            TransferToOtherOnMouseEvent?.OnPointerUp(pointEventData);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (interactible)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                if (interactible)
                    onLeftClick.Invoke();

                TransferToOtherOnMouseEvent?.OnPointerClick(pointerEventData);
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                if (interactible)
                    onRightClick.Invoke();

                TransferToOtherOnMouseEvent?.OnPointerClick(pointerEventData);
            }
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (interactible)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                onLeftButtonBeginDragging.Invoke();
            }
            TransferToOtherOnMouseEvent?.OnBeginDrag(pointerEventData);
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (interactible)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                onLeftButtonDragging.Invoke();
            }
            TransferToOtherOnMouseEvent?.OnDrag(pointerEventData);
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (interactible)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                onLeftButtonEndDraging.Invoke();
            }
            TransferToOtherOnMouseEvent?.OnEndDrag(pointerEventData);
        }
    }
}
