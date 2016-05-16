using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Ramses.SectionButtons
{
	public class SectionColorButton : SectionButton
	{
		[SerializeField]
		private Color activeColor = Color.white;
		[SerializeField]
		private Color selectedColor = Color.white;
		[SerializeField]
		private Color pressColor = Color.gray;
		[SerializeField]
		private Color inactiveColor = new Color(1, 1, 1, 0.5f);

		[SerializeField]
		private Image backgroundImage;

		public override void Unselect()
		{
			if (Active)
			{
				base.Unselect();
				backgroundImage.color = activeColor;
			}
		}

		public override void Select()
		{
			if (Active)
			{
				base.Select();
				backgroundImage.color = selectedColor;
			}
		}

		public override void Press()
		{
			if (Active)
			{
				backgroundImage.color = pressColor;
				base.Press();
			}
		}

		public override void SetActiveState(bool activeState)
		{
			base.SetActiveState(activeState);
			if (!Active)
			{
				backgroundImage.color = inactiveColor;
			}
		}
	}
}