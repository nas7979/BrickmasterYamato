using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
	private int m_IndexInPool;
	private ObjectPool m_Pool;

	public void Deactivate()
	{
		m_Pool.DeactivatedObjects.Enqueue(m_IndexInPool);
		gameObject.SetActive(false);
	}

	public void SetPool(ObjectPool _Pool, int _Index)
	{
		m_Pool = _Pool;
		m_IndexInPool = _Index;
	}
}
