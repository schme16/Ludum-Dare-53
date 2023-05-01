using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class UIHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public PhoneUIScript phone;

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData) {
		phone.hovered = true;
	}
	
	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData) {
		phone.hovered = false;
	}
}
