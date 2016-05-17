using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Ramses.SectionButtons
{
	public class SectionHolder : MonoBehaviour
	{

		public delegate void ButtonSectionManagerHandler(SectionHolder holder, Section section, SectionButton button);

		public event ButtonSectionManagerHandler ButtonPressedEvent;
		public event ButtonSectionManagerHandler ButtonReleasedEvent;
		public event ButtonSectionManagerHandler ButtonSelectedEvent;
		public event ButtonSectionManagerHandler ButtonUnselectedEvent;

		// Holder Readers
		public bool Active { get { return activated && gameObject.activeSelf; } }
		public Section CurrentSection
		{
			get
			{
				return currentSection;
			}
		}
		private Section currentSection = null;
		public int CurrentLayer { get { return currentLayer; } }

		// Holder options
		[SerializeField]
		private bool activated = true;

		// Holder values
		public SectionInfo[] allSectionInfos;
		private Dictionary<int, List<Section>> allSectionsInLayers = new Dictionary<int, List<Section>>();
		private int currentSectionIndex = 0;
		private int currentLayer = 0;

		private void Awake()
		{
			foreach (SectionInfo info in allSectionInfos)
			{
				if (!allSectionsInLayers.ContainsKey(info.Layer))
				{
					allSectionsInLayers.Add(info.Layer, new List<Section>());
				}
				allSectionsInLayers[info.Layer].Add(info.Section);
			}
			SetCurrentSection(currentSectionIndex);
			SetSection(currentSectionIndex);
		}

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
			SetSectionsActiveState(true);
			ListenToSections(true);
		}

		public void Deactivate()
		{
			activated = false;
			SetSectionsActiveState(false);
			ListenToSections(false);
		}

		public void NextSection()
		{
			Section[] sections = allSectionsInLayers[currentLayer].ToArray();
			SetSection(sections.GetLoopIndex(currentSectionIndex + 1));
		}

		public void PreviousSection()
		{
			Section[] sections = allSectionsInLayers[currentLayer].ToArray();
			SetSection(sections.GetLoopIndex(currentSectionIndex - 1));
		}

		public void SwitchLayer(int layer, int buttonSelect = -1, int sectionIndex = 0)
		{
			currentLayer = layer;
			SetSection(sectionIndex);
			CurrentSection.SetCurrentButton(buttonSelect);

		}

		public int GetLayerOfSection(Section section)
		{
			foreach (SectionInfo info in allSectionInfos)
			{
				if (info.Section == section)
				{
					return info.Layer;
				}
			}
			Debug.LogError("Section not part of this holder, returning -404");
			return -404;
		}

		private void SetSection(int index)
		{
			int preButtonIndex = 0;
			Section[] sections = allSectionsInLayers[currentLayer].ToArray();
			int lastIndex = currentSectionIndex < 0 ? 0 : currentSectionIndex;
			lastIndex = index >= 0 ? lastIndex : -1;
			int targetIndex = currentSectionIndex;

			if (CurrentSection != null)
			{
				preButtonIndex = CurrentSection.CurrentButtonIndex;
				CurrentSection.SetCurrentButton(-1);
			}

			targetIndex = sections.GetClampedIndex(index);
			SetCurrentSection(targetIndex);
			bool available = index >= 0;
			if (available)
			{
				if (CurrentSection == null || !CurrentSection.Active)
				{
					targetIndex = lastIndex;
					available = false;
					int dir = (index - targetIndex);
					dir = (int)Mathf.Sign(dir);
					Section testSection = null;
					for (int i = 1; i < sections.Length; i++)
					{
						testSection = sections.GetLoop(targetIndex + (dir * i));
						if (testSection != null && testSection.Active)
						{
							available = true;
							targetIndex = sections.GetIndexOf(testSection);
							break;
						}
					}
				}
			}

			if (!available)
			{
				targetIndex = lastIndex;
			}

			SetCurrentSection(targetIndex);
			CurrentSection.SetCurrentButton(preButtonIndex);
		}

		private void SetCurrentSection(int index)
		{
			currentSectionIndex = index;
			currentSection = allSectionsInLayers[currentLayer][currentSectionIndex];
		}

		private void ListenToSections(bool listen)
		{
			foreach (SectionInfo info in allSectionInfos)
			{
				if (listen)
				{
					info.Section.ButtonPressedEvent += OnButtonPressedEvent;
					info.Section.ButtonSelectedEvent += OnButtonSelectedEvent;
					info.Section.ButtonUnselectedEvent += OnButtonUnSelectedEvent;
					info.Section.ButtonReleasedEvent += OnButtonReleasedEvent;
					info.Section.SectionActiveStateChangedEvent += OnSectionActiveStateChangedEvent;
				}
				else
				{
					info.Section.ButtonPressedEvent -= OnButtonPressedEvent;
					info.Section.ButtonSelectedEvent -= OnButtonSelectedEvent;
					info.Section.ButtonUnselectedEvent -= OnButtonUnSelectedEvent;
					info.Section.ButtonReleasedEvent -= OnButtonReleasedEvent;
					info.Section.SectionActiveStateChangedEvent -= OnSectionActiveStateChangedEvent;
				}
			}
		}

		private void SetSectionsActiveState(bool activeState)
		{
			foreach (SectionInfo info in allSectionInfos)
			{
				info.Section.SetActiveState(activeState);
			}
		}

		private void OnButtonPressedEvent(Section section, SectionButton button)
		{
			if (ButtonPressedEvent != null)
			{
				ButtonPressedEvent(this, section, button);
			}
		}

		private void OnButtonSelectedEvent(Section section, SectionButton button)
		{
			CheckCurrentSection(section);
			if (ButtonSelectedEvent != null)
			{
				ButtonSelectedEvent(this, section, button);
			}
		}

		private void OnButtonUnSelectedEvent(Section section, SectionButton button)
		{
			if (ButtonUnselectedEvent != null)
			{
				ButtonUnselectedEvent(this, section, button);
			}
		}

		private void OnButtonReleasedEvent(Section section, SectionButton button)
		{
			if (ButtonReleasedEvent != null)
			{
				ButtonReleasedEvent(this, section, button);
			}
		}

		private void OnSectionActiveStateChangedEvent(Section section)
		{
			if (section == CurrentSection && !section.Active)
			{
				SetSection(currentSectionIndex);
			}
			else if (CurrentSection == null)
			{
				SetSection(0);
			}
		}

		private void CheckCurrentSection(Section section)
		{
			if (CurrentSection != section)
			{
				if (currentLayer != GetLayerOfSection(section))
				{
					SwitchLayer(GetLayerOfSection(section));
				}

				SetSection(allSectionsInLayers[currentLayer].ToArray().GetIndexOf(section));
			}
		}
	}

	[Serializable]
	public class SectionInfo
	{
		public Section Section { get { return section; } }
		public int Layer { get { return layer; } }

		[SerializeField]
		private Section section;
		[SerializeField]
		private int layer = 0;
	}
}