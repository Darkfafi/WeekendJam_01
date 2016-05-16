using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Ramses.SectionButtons
{
	public abstract class SectionListButton<T> : SectionColorButton
	{

		public delegate void ListButtonHandler(SectionListButton<T> button, T lastItem, T newItem);
		public event ListButtonHandler NextButtonUsedEvent;
		public event ListButtonHandler PreviousButtonUsedEvent;


		[SerializeField]
		SectionButton NextButton;

		[SerializeField]
		SectionButton PreviousButton;

		// Readers
		public T CurrentValue { get { return allItems[currentValueIndex]; } }

		// Options
		[SerializeField]
		private bool looping = false;
		protected virtual List<T> allItems { get { return _allItems; } }
		private List<T> _allItems = new List<T>();

		// Values
		private int currentValueIndex = 0;

		private void Start()
		{
			NextButton.ButtonPressedEvent += NextButtonPressed;
			PreviousButton.ButtonPressedEvent += PreviousButtonPressed;
			SetLoopButtons();
		}

		public void AddItem(T item)
		{
			allItems.Add(item);
		}

		public void RemoveItem(T item)
		{
			allItems.Remove(item);
		}

		public void UseNextButton()
		{
			NextButton.Press();
		}

		public void UsePreviousButton()
		{
			PreviousButton.Press();
		}

		public void SetLoopable(bool value)
		{
			looping = value;
			SetLoopButtons();
		}

		private void NextButtonPressed(SectionButton button)
		{
			T[] all = allItems.ToArray();
			T oldValue = CurrentValue;
			currentValueIndex = all.GetLoopIndex(currentValueIndex + 1);
			SetLoopButtons();
			if (NextButtonUsedEvent != null)
			{
				NextButtonUsedEvent(this, oldValue, CurrentValue);
			}
		}

		private void PreviousButtonPressed(SectionButton button)
		{
			T[] all = allItems.ToArray();
			T oldValue = CurrentValue;
			currentValueIndex = all.GetLoopIndex(currentValueIndex - 1);
			SetLoopButtons();
			if (PreviousButtonUsedEvent != null)
			{
				PreviousButtonUsedEvent(this, oldValue, CurrentValue);
			}
		}

		private void SetLoopButtons()
		{
			NextButton.SetActiveState(Active);
			PreviousButton.SetActiveState(Active);
			if (!looping)
			{
				if (currentValueIndex == 0)
				{
					PreviousButton.SetActiveState(false);
				}
				if (currentValueIndex == allItems.Count - 1)
				{
					NextButton.SetActiveState(false);
				}
			}
		}

		public override void SetActiveState(bool value)
		{
			base.SetActiveState(value);
			SetLoopButtons();
		}
	}
}