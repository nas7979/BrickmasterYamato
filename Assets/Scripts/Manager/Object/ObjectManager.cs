using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool
{
	public GameObject Prefab;
	public List<GameObject> Pool;
	public Queue<int> DeactivatedObjects;
}

public class ObjectManager : Singleton<ObjectManager>
{

	private Dictionary<string, ObjectPool> m_Pools = new Dictionary<string, ObjectPool>();
	private Dictionary<string, GameObject> m_Prefabs = new Dictionary<string, GameObject>();

	protected override void Awake()
	{
		base.Awake();
		GameObject[] Temp;
		Temp = Resources.LoadAll<GameObject>("Prefab");
		for(int i = 0; i < Temp.Length; i++)
		{
			m_Prefabs.Add(Temp[i].name, Temp[i]);
		}

		ObjectPool Pool;

		Temp = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < Temp.Length; i++)
		{
			if (!m_Pools.TryGetValue(Temp[i].name, out Pool))
			{
				Pool = new ObjectPool();
				Pool.Pool = new List<GameObject>();
				Pool.DeactivatedObjects = new Queue<int>();
				Pool.Prefab = m_Prefabs[Temp[i].name];
				m_Pools.Add(Temp[i].name, Pool);
			}
			Pool.Pool.Add(Temp[i]);
		}

		Temp = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < Temp.Length; i++)
		{
			if (!m_Pools.TryGetValue(Temp[i].name, out Pool))
			{
				Pool = new ObjectPool();
				Pool.Pool = new List<GameObject>();
				Pool.DeactivatedObjects = new Queue<int>();
				Pool.Prefab = m_Prefabs[Temp[i].name];
				m_Pools.Add(Temp[i].name, Pool);
			}
			Temp[i].GetComponent<Object>().SetPool(Pool, Pool.Pool.Count - 1);
			Pool.Pool.Add(Temp[i]);
		}

		Temp = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < Temp.Length; i++)
		{
			if (!m_Pools.TryGetValue(Temp[i].name, out Pool))
			{
				Pool = new ObjectPool();
				Pool.Pool = new List<GameObject>();
				Pool.DeactivatedObjects = new Queue<int>();
				Pool.Prefab = m_Prefabs[Temp[i].name];
				m_Pools.Add(Temp[i].name, Pool);
			}
			Temp[i].GetComponent<Object>().SetPool(Pool, Pool.Pool.Count - 1);
			Pool.Pool.Add(Temp[i]);
		}

		Temp = GameObject.FindGameObjectsWithTag("HitBox");
		for (int i = 0; i < Temp.Length; i++)
		{
			if (!m_Pools.TryGetValue(Temp[i].name, out Pool))
			{
				Pool = new ObjectPool();
				Pool.Pool = new List<GameObject>();
				Pool.DeactivatedObjects = new Queue<int>();
				Pool.Prefab = m_Prefabs[Temp[i].name];
				m_Pools.Add(Temp[i].name, Pool);
			}
			Temp[i].GetComponent<Object>().SetPool(Pool, Pool.Pool.Count - 1);
			Pool.Pool.Add(Temp[i]);
		}
	}

	public GameObject AddObject(string _Name, Vector3 _Pos)
	{
		ObjectPool Pool = null;
		if(!m_Pools.TryGetValue(_Name, out Pool))
		{
			Pool = new ObjectPool();
			Pool.Pool = new List<GameObject>();
			Pool.DeactivatedObjects = new Queue<int>();
			Pool.Prefab = m_Prefabs[_Name];
			m_Pools.Add(_Name, Pool);
		}

		GameObject Temp;
		if (Pool.DeactivatedObjects.Count == 0)
		{
			Temp = Instantiate(Pool.Prefab);
			Temp.name = Pool.Prefab.name;
			Temp.GetComponent<Object>().SetPool(Pool, Pool.Pool.Count);
			Pool.Pool.Add(Temp);
		}
		else
		{
			Temp = Pool.Pool[Pool.DeactivatedObjects.Dequeue()];
			Temp.SetActive(true);
		}
		Temp.transform.position = _Pos;

		return Temp;
	}

	public void ClearPool(string _Name)
	{
		ObjectPool Pool = m_Pools[_Name];
		Pool.DeactivatedObjects.Clear();
		for(int i = 0; i < Pool.Pool.Count; i++)
		{
			Destroy(Pool.Pool[i]);
		}
		Pool.Pool.Clear();
	}

	public void Clear()
	{
		foreach(var iter in m_Pools)
		{
			ClearPool(iter.Key);
		}
	}
}
