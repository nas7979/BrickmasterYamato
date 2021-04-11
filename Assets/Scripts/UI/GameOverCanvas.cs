using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
	public float m_AlphaSpeed = 1.5f;

	private IEnumerator CR_Start(int _Index, float _TargetAlpha)
	{
		Image temp = transform.GetChild(_Index).GetComponent<Image>();
		Color color;
		while(temp.color.a <= _TargetAlpha)
		{
			color = temp.color;
			color.a += m_AlphaSpeed *Time.unscaledDeltaTime;
			temp.color = color;
			yield return null;
		}
		color = temp.color;
		color.a = _TargetAlpha;
		temp.color = color;
	}
}
