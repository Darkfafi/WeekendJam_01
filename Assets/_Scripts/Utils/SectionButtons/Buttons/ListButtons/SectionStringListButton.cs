using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Ramses.SectionButtons
{
	public class SectionStringListButton : SectionListButton<string>
	{
		protected override List<string> allItems
		{
			get
			{
				return stringList;
			}
		}

		[SerializeField]
		private List<string> stringList;
	}
}