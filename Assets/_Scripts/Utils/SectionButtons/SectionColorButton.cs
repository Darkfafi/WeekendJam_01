using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ramses.SectionButtons
{
	public class SectionColorButton : SectionButton {

		[SerializeField] private Color activeColor = Color.white;
		[SerializeField] private Color selectedColor = Color.white;
		[SerializeField] private Color pressColor = Color.gray;
		[SerializeField] private Color inactiveColor = new Color(1, 1, 1, 0.5f);
		
		private Image backgroundImage;

		protected override void Awake()
		{
			backgroundImage = GetComponent<Image>();
			base.Awake();
		}

		public override void Idle()
		{
			if (Activated)
			{
				base.Idle();
				backgroundImage.color = activeColor;
			}
        }

		public override void Select()
		{
			if (Activated)
			{
				base.Select();
				backgroundImage.color = selectedColor;
			}
		}

		public override void Press()
		{
			if (Activated)
			{
				base.Press();
				backgroundImage.color = pressColor;
			}
        }

		public override void SetActiveState(bool activeState)
		{
			base.SetActiveState(activeState);
			if(!Activated)
			{
				backgroundImage.color = inactiveColor;
			}
		}
	}
}
