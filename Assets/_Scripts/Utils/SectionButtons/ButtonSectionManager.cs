using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ramses.SectionButtons
{
	/// <summary>
	/// ButtonSectionManager is a class which manages all the ButtonSection classes it is given. 
	/// I choses 1 section to be selected and will deselect the section when a button in another section is selected. (In its manager of course)
	/// You can use a GameSpecific class to use your keybindings to controll the current section its buttons and switch sections.
	/// There is also automaticly Mouse support on the buttons themselves.
	/// For Mouse support the EventSystem is needed in the Scene. It is automaticly created along with the Canvas.
	/// </summary>
	public class ButtonSectionManager : MonoBehaviour
	{
		public delegate void ButtonSectionManagerHandler(ButtonSectionManager manager, ButtonSection section, SectionButton button);

		public event ButtonSectionManagerHandler ButtonPressedEvent;
		public event ButtonSectionManagerHandler ButtonReleasedEvent;
		public event ButtonSectionManagerHandler ButtonSelectedEvent;
		public event ButtonSectionManagerHandler ButtonUnselectedEvent;

		public int GroupID { get { return groupId; } }
		[SerializeField] private int groupId = 0;

		public ButtonSection CurrentlySelectedSection { get { return allSections[currentlySelectedSectionIndex]; } }
		private int currentlySelectedSectionIndex = 0;

		public bool Activated { get { return activated; } }
		[SerializeField] private bool activated = true;

		[SerializeField]
		private ButtonSection[] allSections;

		private void Start()
		{
			if (activated)
			{
				Activate();
			}
			else
			{
				Deactivate();
			}
		}

		public void Activate()
		{
			activated = true;
			ListenToSections(true);
			SetSection(currentlySelectedSectionIndex, false);
			SetSectionActiveState();
        }

		public void Deactivate()
		{
			activated = false;
			ListenToSections(false);
            CurrentlySelectedSection.Unselected();
			SetSectionActiveState();
		}

		public void SetCurrentSection(int index)
		{
			SetSection(allSections.GetLoopIndex(index));
		}

		public void SetCurrentSection(string name)
		{
			foreach(ButtonSection section in allSections)
			{
				if(section.SectionName == name)
				{
					SetSection(allSections.GetIndexOf(section));
					return;
				}
			}
			Debug.LogWarning("No section found with the name: " + name);
		}

		public void NextSection()
		{
			SetSection(allSections.GetLoopIndex(currentlySelectedSectionIndex + 1));
		}

		public void PreviousSection()
		{
			SetSection(allSections.GetLoopIndex(currentlySelectedSectionIndex - 1));
		}

		private void SetSection(int index, bool selectButton = true)
		{
			int selectedButtonIndex = CurrentlySelectedSection.CurrentButtonIndex;
            CurrentlySelectedSection.Unselected();
			currentlySelectedSectionIndex = index;
			if (selectButton && CurrentlySelectedSection.CurrentlySelectedButton == null)
			{
				CurrentlySelectedSection.SetButton(selectedButtonIndex, false);
			}
			CurrentlySelectedSection.Selected();
		}

		private void ListenToSections(bool listen)
		{
			foreach (ButtonSection s in allSections)
			{
				if (listen)
				{
					s.ButtonPressedEvent += OnButtonPressedEvent;
					s.ButtonSelectedEvent += OnButtonSelectedEvent;
					s.ButtonUnselectedEvent += OnButtonUnSelectedEvent;
					s.ButtonReleasedEvent += OnButtonReleasedEvent;
				}
				else
				{
					s.ButtonPressedEvent -= OnButtonPressedEvent;
					s.ButtonSelectedEvent -= OnButtonSelectedEvent;
					s.ButtonUnselectedEvent -= OnButtonUnSelectedEvent;
					s.ButtonReleasedEvent -= OnButtonReleasedEvent;
				}
			}
		}

		private void OnButtonPressedEvent(ButtonSection section, SectionButton button)
		{
			if(ButtonPressedEvent != null)
			{
				ButtonPressedEvent(this, section, button);
			}
		}

		private void OnButtonSelectedEvent(ButtonSection section, SectionButton button)
		{
			CheckCurrentSection(section);
            if (ButtonSelectedEvent != null)
			{
				ButtonSelectedEvent(this, section, button);
			}
		}

		private void OnButtonUnSelectedEvent(ButtonSection section, SectionButton button)
		{
			if (ButtonUnselectedEvent != null)
			{
				ButtonUnselectedEvent(this, section, button);
			}
		}

		private void OnButtonReleasedEvent(ButtonSection section, SectionButton button)
		{
			if (ButtonReleasedEvent != null)
			{
				ButtonReleasedEvent(this, section, button);
			}
		}

		private void CheckCurrentSection(ButtonSection section)
		{
			if (CurrentlySelectedSection != section)
			{
				SetSection(allSections.GetIndexOf(section));
			}
		}

		private void SetSectionActiveState()
		{
			foreach(ButtonSection section in allSections)
			{
				section.SetActiveState(Activated);
			}
		}
	}
}
