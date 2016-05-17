using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
namespace Ramses.SectionButtons
{
	public abstract class SectionButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
	{
		public delegate void ButtonHandler(SectionButton button);

		public event ButtonHandler ButtonPressedEvent;
		public event ButtonHandler ButtonReleasedEvent;
		public event ButtonHandler ButtonUnselectedEvent;
		public event ButtonHandler ButtonSelectedEvent;
		public event ButtonHandler ButtonChangedActiveStateEvent;

		// Button Readings
		public bool MouseControllable { get { return Active && mouseControllable; } }
		public bool Active { get { return activated && gameObject.activeSelf; } }
		public string ButtonName { get { return buttonName; } }

		// Button Options
		private bool activated = true;
		private bool mouseControllable = true;
		[SerializeField] private string buttonName = "Button";
		// Button State 
		private bool selected = false;

		public virtual void Unselect()
		{
			if (Active)
			{
				if (selected)
				{
					selected = false;
					if (ButtonUnselectedEvent != null)
					{
						ButtonUnselectedEvent(this);
					}
				}
			}
		}

		public virtual void Select()
		{
			if (Active)
			{
				if (!selected)
				{
					selected = true;
					if (ButtonSelectedEvent != null)
					{
						ButtonSelectedEvent(this);
					}
				}
			}
		}

		public virtual void Press()
		{
			if (Active)
			{
				if (ButtonPressedEvent != null)
				{
					ButtonPressedEvent(this);
				}
			}
		}

		public virtual void Release()
		{
			if (Active)
			{
				SetCurrentState();
				if (ButtonReleasedEvent != null)
				{
					ButtonReleasedEvent(this);
				}
			}
		}

		protected virtual void SetCurrentState()
		{
			if (selected)
			{
				Select();
			}
			else
			{
				Unselect();
			}
		}

		public virtual void SetActiveState(bool value)
		{
			if (!value)
			{
				Unselect();
			}
			bool changedState = activated != value;
			activated = value;
			if (activated)
			{
				SetCurrentState();
			}
			if (changedState)
			{
				if (ButtonChangedActiveStateEvent != null)
				{
					ButtonChangedActiveStateEvent(this);
				}
			}
		}

		// Mouse Controll

		public void SetMouseControllable(bool value)
		{
			mouseControllable = value;
			if (!mouseControllable && selected)
			{
				Unselect();
			}
		}


		public void OnPointerDown(PointerEventData eventData)
		{
			if (MouseControllable)
			{
				Press();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (MouseControllable)
			{
				Select();
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (MouseControllable)
			{
				Unselect();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (MouseControllable)
			{
				Release();
			}
		}
	}
}