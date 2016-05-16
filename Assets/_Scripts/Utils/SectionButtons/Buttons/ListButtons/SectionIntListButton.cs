using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Ramses.SectionButtons
{
	public class SectionIntListButton : SectionListButton<int>
	{
		protected override List<int> allItems
		{
			get
			{
				return intList;
			}
		}

		[SerializeField]
		private List<int> intList;
	}
}