using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject item;
	Vector3 startPos;
	Transform startParent;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		item = gameObject;
		startPos = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		item = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == startParent){
			transform.position = startPos;
		}
	}

	#endregion




}
