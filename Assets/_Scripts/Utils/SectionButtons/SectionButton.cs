using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Ramses.SectionButtons
{
	/// <summary>
	/// The button is part of the ButtonSection.
	/// All the methods are called by the ButtonSection if its linked in one its list. 
	/// Any interaction with the button is recommended to be done through the ButtonSection Class
	/// For Mouse support the EventSystem is needed in the Scene. It is automaticly created along with the Canvas.
	/// </summary>
	public abstract class SectionButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
	{
		public delegate void ButtonHandler(SectionButton button);
		public event ButtonHandler ButtonPressedEvent;
		public event ButtonHandler ButtonReleasedEvent;
		public event ButtonHandler ButtonSelectedEvent;
		public event ButtonHandler ButtonUnselectedEvent;

		public string ButtonName { get { return buttonName; } }
		[SerializeField] private string buttonName = "Button";
		public bool Selected { get; private set; }
		public bool Activated { get; private set; }

		protected virtual void Awake()
		{
			Selected = false;
			SetState();
		}

		public virtual void Press()
		{
			if (Activated)
			{
				if (ButtonPressedEvent != null)
				{
					ButtonPressedEvent(this);
				}
			}
		}

		public virtual void Release()
		{
			if (Activated)
			{
				SetState();
				if (ButtonReleasedEvent != null)
				{
					ButtonReleasedEvent(this);
				}
			}
		}

		public virtual void Idle()
		{
			if (Activated)
			{
				if (Selected)
				{
					Selected = false;
					if (ButtonUnselectedEvent != null)
					{
						ButtonUnselectedEvent(this);
					}
				}
			}
        }

		public virtual void Select()
		{
			if (Activated)
			{
				if (!Selected)
				{
					Selected = true;
					if (ButtonSelectedEvent != null)
					{
						ButtonSelectedEvent(this);
					}
				}
			}
		}

		public virtual void SetActiveState(bool activeState)
		{
			Activated = activeState;
			if (Activated)
			{
				SetState();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Activated)
			{
				Select();
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (Activated)
			{
				Idle();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (Activated)
			{
				Release();
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (Activated)
			{
				Press();
			}
		}

		private void SetState()
		{
			if (Selected)
			{
				Select();
			}
			else
			{
				Idle();
			}
		}
	}
}
