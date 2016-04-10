using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Confactory;
public class NotificationBar : MonoBehaviour {
	[SerializeField]
	private Image background;
	[SerializeField]
	private Text notificationText;

	private ConCoroutines conCoroutines;
	private object fadeContext = new object();
	private object waitContext = new object();

	private enum AnimationType
	{
		Show,
		Hide
	}

	private float backgroundDefaultAlpha;
	private float notificationTextDefaultAlpha;

	void Awake()
	{
		conCoroutines = ConfactoryFinder.Instance.Give<ConCoroutines>();
		backgroundDefaultAlpha = background.color.a;
		notificationTextDefaultAlpha = notificationText.color.a;
		SetAlphaObject(0);
    }

	public void ShowNotification(string text, float showSpeed = 1)
	{
		ShowNotification(text, showSpeed, 0);
    }

	public void ShowNotification(string text,float showSpeed, float timeToShow, float hideSpeed = 1)
	{
		//if timeToShow != 0; then after x seconds the bar will fade away with the HideNotification() method
		if(timeToShow < 0)
		{
			timeToShow = 0;
		}
		StopActiveCoroutines();
		FadeAnimation(AnimationType.Show, showSpeed);
		notificationText.text = text;
		if(timeToShow != 0)
		{
			waitContext = new object();
            conCoroutines.StartCoroutine(WaitToHide(timeToShow, hideSpeed),waitContext);
        }
	}

	public void HideNotification(float hideSpeed = 1)
	{
		StopActiveCoroutines();
		FadeAnimation(AnimationType.Hide, hideSpeed);
	}

	private IEnumerator WaitToHide(float timeInSeconds, float hideSpeed)
	{
		yield return new WaitForSeconds(timeInSeconds);
		HideNotification(hideSpeed);
    }

	private void FadeAnimation(AnimationType animation,float speed)
	{
		StopActiveCoroutines();
		fadeContext = new object();
		if (animation == AnimationType.Show)
		{
			SetAlphaObject(0);
			conCoroutines.StartCoroutine(ShowAnimation(speed),fadeContext);
        }
		else
		{
			if(GetAlphaObject() == 0)
			{
				SetAlphaObject(1);
			}
			conCoroutines.StartCoroutine(HideAnimation(speed),fadeContext);
		}
		
	}
	
	private IEnumerator HideAnimation(float speed)
	{
		while (GetAlphaObject() >= 0)
		{
			yield return new WaitForEndOfFrame();
			SetAlphaObject(GetAlphaObject() - Time.deltaTime * speed);
		}
		conCoroutines.StopContext(fadeContext);
	}

	private IEnumerator ShowAnimation(float speed)
	{
		while (GetAlphaObject() <= 1)
		{
			yield return new WaitForEndOfFrame();
			SetAlphaObject(GetAlphaObject() + Time.deltaTime * speed);
		}
		conCoroutines.StopContext(fadeContext);
	}

	private void SetAlphaObject(float alphaRange)
	{
		Color bC = background.color;
		Color tC = notificationText.color;
		alphaRange = alphaRange > 1 ? 1 : alphaRange;
		alphaRange = alphaRange < 0 ? 0 : alphaRange;
		background.color = new Color(bC.r, bC.g, bC.b, alphaRange * backgroundDefaultAlpha);
		notificationText.color = new Color(tC.r, tC.g, tC.b, alphaRange * notificationTextDefaultAlpha);
		if(alphaRange == 0)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
	}
	private float GetAlphaObject()
	{
		float result = background.color.a / backgroundDefaultAlpha;
        return result;
	}
	private void OnDestroy()
	{
		StopActiveCoroutines();
    }
	private void StopActiveCoroutines()
	{
		if (conCoroutines.HasContext(waitContext))
		{
			conCoroutines.StopContext(waitContext);
		}
		if (conCoroutines.HasContext(fadeContext))
		{
			conCoroutines.StopContext(fadeContext);
		}	
	}
}
