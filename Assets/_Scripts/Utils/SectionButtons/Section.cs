using UnityEngine;
using System.Collections;
namespace Ramses.SectionButtons
{
	public class Section : MonoBehaviour
	{

		public delegate void ButtonSectionHandler(Section section, SectionButton button);
		public delegate void SectionHandler(Section section);

		public event ButtonSectionHandler ButtonUnselectedEvent;
		public event ButtonSectionHandler ButtonSelectedEvent;
		public event ButtonSectionHandler ButtonReleasedEvent;
		public event ButtonSectionHandler ButtonPressedEvent;
		public event SectionHandler SectionActiveStateChangedEvent;


		// Section Readers
		public bool Active { get { return activated && gameObject.activeSelf; } }
		public SectionButton CurrentButton { get { return (currentButtonIndex >= 0) ? allSecionButtons[currentButtonIndex] : null; } }
		public int CurrentButtonIndex { get { return currentButtonIndex; } }
		// Section Options
		private bool activated = true;

		// Section values
		[SerializeField]
		private SectionButton[] allSecionButtons;
		private int currentButtonIndex = -1;

		private void OnEnable()
		{
			ListenToButtons(false);
			ListenToButtons(true);
		}

		private void OnDisable()
		{
			ListenToButtons(false);
		}

		public void NextButton()
		{
			SetCurrentButton(allSecionButtons.GetLoopIndex(currentButtonIndex + 1));
		}

		public void PreviousButton()
		{
			if(currentButtonIndex < 0)
			{
				currentButtonIndex = 0;
			}

			SetCurrentButton(allSecionButtons.GetLoopIndex(currentButtonIndex - 1));
		}

		public virtual void SetActiveState(bool value)
		{
			activated = value;
			foreach (SectionButton button in allSecionButtons)
			{
				button.SetActiveState(value);
			}
		}

		public void SetCurrentButton(int index)
		{
			int lastIndex = currentButtonIndex < 0 ? 0 : currentButtonIndex;
			lastIndex = index >= 0 ? lastIndex : -1;

			if (CurrentButton != null)
			{
				CurrentButton.Unselect();
			}

			currentButtonIndex = allSecionButtons.GetClampedIndex(index);
			bool available = index >= 0;
			if (available)
			{
				if (CurrentButton == null || !CurrentButton.Active)
				{
					currentButtonIndex = lastIndex;
					available = false;
					int dir = (index - currentButtonIndex);
					dir = (int)Mathf.Sign(dir);
					SectionButton testbutton = null;
					for (int i = 1; i < allSecionButtons.Length; i++)
					{
						testbutton = allSecionButtons.GetLoop(currentButtonIndex + (dir * i));
						if (testbutton != null && testbutton.Active)
						{
							available = true;
							currentButtonIndex = allSecionButtons.GetIndexOf(testbutton);
							break;
						}
					}
				}
			}

			if (!available)
			{
				currentButtonIndex = lastIndex;
			}

			if (CurrentButton != null && CurrentButton.Active)
			{
				CurrentButton.Select();
			}
		}

		// Button Listenings

		private void ListenToButtons(bool listen)
		{
			foreach (SectionButton b in allSecionButtons)
			{
				if (listen)
				{
					b.ButtonPressedEvent += OnButtonPressedEvent;
					b.ButtonSelectedEvent += OnButtonSelectedEvent;
					b.ButtonUnselectedEvent += OnButtonUnSelectedEvent;
					b.ButtonReleasedEvent += OnButtonReleasedEvent;
				}
				else
				{
					b.ButtonPressedEvent -= OnButtonPressedEvent;
					b.ButtonSelectedEvent -= OnButtonSelectedEvent;
					b.ButtonUnselectedEvent -= OnButtonUnSelectedEvent;
					b.ButtonReleasedEvent -= OnButtonReleasedEvent;
				}
			}
		}


		protected virtual void OnButtonPressedEvent(SectionButton button)
		{
			if (button != CurrentButton)
			{
				SetCurrentButton(allSecionButtons.GetIndexOf(button));
			}
			if (ButtonPressedEvent != null)
			{
				ButtonPressedEvent(this, button);
			}
		}

		protected virtual void OnButtonReleasedEvent(SectionButton button)
		{
			if (ButtonReleasedEvent != null)
			{
				ButtonReleasedEvent(this, button);
			}
		}

		protected virtual void OnButtonSelectedEvent(SectionButton button)
		{
			if (button != CurrentButton)
			{
				SetCurrentButton(allSecionButtons.GetIndexOf(button));
			}
			if (ButtonSelectedEvent != null)
			{
				ButtonSelectedEvent(this, button);
			}
		}

		protected virtual void OnButtonUnSelectedEvent(SectionButton button)
		{
			if (CurrentButton != null)
			{
				SetCurrentButton(-1);
			}
			if (ButtonUnselectedEvent != null)
			{
				ButtonUnselectedEvent(this, button);
			}
		}
	}
}