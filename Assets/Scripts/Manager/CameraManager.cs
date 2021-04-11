using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : Singleton<CameraManager>
{
	public Camera m_Camera = null;
	public Camera Camera
	{
		get { return m_Camera; }
	}
	private Vector2 m_ShakeForce;

	protected override void Awake()
	{
		base.Awake();
		m_Camera = Camera.main;
		m_ShakeForce = Vector2.zero;
	}

	public void Shake(float _Time, float _Force)
	{
		StartCoroutine(CR_Shake(_Time, _Force));
	}

	public void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += PreRender;
		RenderPipelineManager.endCameraRendering += PostRender;
	}

	public void OnDisable()
	{
	}

	private void PreRender(ScriptableRenderContext _Context, Camera _Camera)
	{
		if (m_Camera != null)
		{
			m_Camera.transform.position += (Vector3)m_ShakeForce;
		}
	}

	private void PostRender(ScriptableRenderContext _Context, Camera _Camera)
	{
		if (m_Camera != null)
		{
			m_Camera.transform.position -= (Vector3)m_ShakeForce;
		}
	}

	private IEnumerator CR_Shake(float _Time, float _Force)
	{
		float ForcePerFime = _Force / _Time;
		while(_Force > 0)
		{
			_Force -= ForcePerFime * Time.unscaledDeltaTime;
			m_ShakeForce = new Vector2(Random.Range(-_Force, _Force), Random.Range(-_Force, _Force));
			yield return null;
		}
		m_ShakeForce = Vector2.zero;
	}
}
