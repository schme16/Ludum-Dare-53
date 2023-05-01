using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.


public class UIHoveredClick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public PhoneUIScript phone;

	public void OnPointerUp(PointerEventData eventData) {
		if (phone.hovered && !phone.show) {
			phone.ShowUI(true);
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		//throw new NotImplementedException();
	}
}
