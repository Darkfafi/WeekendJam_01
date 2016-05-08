using UnityEngine;
using System.Collections;

namespace Ramses.SectionButtons
{
	/// <summary>
	/// The ButtonSection is a part of the ButtonSectionManager which holds a set of buttons in order.
	/// All the buttons linked into the ButtonSection is part of it.
	/// The order of the buttons is the order of the array. You can use the ButtonSection to scroll through the buttons one by one.
	/// The Section is automaticly Unselected by the ButtonSectionManager when another section is selected / used.
	/// For Mouse support the EventSystem is needed in the Scene. It is automaticly created along with the Canvas.
	/// </summary>
	public class ButtonSection : MonoBehaviour
	{
		public delegate void ButtonSectionHandler(ButtonSection section, SectionButton button);

		public event ButtonSectionHandler ButtonUnselectedEvent;
		public event ButtonSectionHandler ButtonSelectedEvent;
		public event ButtonSectionHandler ButtonReleasedEvent;
		public event ButtonSectionHandler ButtonPressedEvent;


		public const string DEFAULT_SECTION_NAME = "DEFAULT_SECTION";

		public SectionButton CurrentlySelectedButton { get { return currentlySelectedButtonIndex == -1 ? null : allButtonsOfSection[currentlySelectedButtonIndex]; }
			private set
			{
				if (value != null)
				{
					SetSelectedButton(allButtonsOfSection.GetIndexOf(value));
				}
				else
				{
					SetSelectedButton(-1);
				}
			}
		}

		public int CurrentButtonIndex { get { return currentlySelectedButtonIndex;  } }

		public bool Active { get { return active; } }
		private bool active = true;

		public string SectionName { get { return nameSection;  } }
		[SerializeField] private string nameSection = DEFAULT_SECTION_NAME;

		[SerializeField] private SectionButton[] allButtonsOfSection;
		private int currentlySelectedButtonIndex = -1;

		

		private void OnEnable()
		{
			ListenToButtons(false);
			ListenToButtons(true);
        }

		private void OnDisable()
		{
			ListenToButtons(false);
        }

		private void Start()
		{
			SetButtonsActiveState();
		}

		public void Selected()
		{
			if (CurrentlySelectedButton != null)
			{ 
				SetSelectedButton(currentlySelectedButtonIndex);
			}
		}

		public void Unselected()
		{
			if (CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton = null;
			}
		}

		public void SetActiveState(bool activeState)
		{
			active = activeState;
			SetButtonsActiveState(); 
		}

		public void SetButton(int index, bool loop = true)
		{
			if (loop)
			{
				index = allButtonsOfSection.GetLoopIndex(index);
			}
			else if(index > allButtonsOfSection.Length - 1 || index < 0)
			{
				index = (index < 0) ? 0 : allButtonsOfSection.Length - 1; 
			}
			SetSelectedButton(index);
		}

		public void NextButton()
		{
			SetButton(currentlySelectedButtonIndex + 1);
		}

		public void PreviousButton()
		{
			SetButton(currentlySelectedButtonIndex - 1);
		}

		public void PressSelectedButton()
		{
			if (CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton.Press();
			}
		}

		public void ReleaseSelectedButton()
		{
			if (CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton.Release();
			}
		}

		private void SetSelectedButton(int index)
		{
			if (CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton.Idle();
			}

			currentlySelectedButtonIndex = index;

			if (CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton.Select();
			}
		}

		private void ListenToButtons(bool listen)
		{
			foreach(SectionButton b in allButtonsOfSection)
			{
				if(listen)
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

		private void OnButtonPressedEvent(SectionButton button)
		{
			if(button != CurrentlySelectedButton)
			{
				CurrentlySelectedButton = button;
			}
			if(ButtonPressedEvent != null)
			{
				ButtonPressedEvent(this, button);
			}
		}

		private void OnButtonReleasedEvent(SectionButton button)
		{
			if (ButtonReleasedEvent != null)
			{
				ButtonReleasedEvent(this, button);
			}
		}

		private void OnButtonSelectedEvent(SectionButton button)
		{
			if(button != CurrentlySelectedButton)
			{
				CurrentlySelectedButton = button;
			}
			if(ButtonSelectedEvent != null)
			{
				ButtonSelectedEvent(this, button);
			}
		}

		private void OnButtonUnSelectedEvent(SectionButton button)
		{
			if(CurrentlySelectedButton != null)
			{
				CurrentlySelectedButton = null;
			}
			if (ButtonUnselectedEvent != null)
			{
				ButtonUnselectedEvent(this, button);
			}
		}

		private void SetButtonsActiveState()
		{
			foreach(SectionButton b in allButtonsOfSection)
			{
				b.SetActiveState(Active);
			}
		}
    }
}