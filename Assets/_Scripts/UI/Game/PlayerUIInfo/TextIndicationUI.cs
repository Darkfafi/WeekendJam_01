using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextIndicationUI : MonoBehaviour {

	public Text IndicationText { get { return text; } }
	public Image IndicationBackground { get {return background;}}

	[SerializeField]
	private Image background;
	[SerializeField]
	private Text text;
}
