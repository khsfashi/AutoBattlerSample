using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링을 위해 작성된 클래스입니다.
/// </summary>
public class PoolManager : Manager<PoolManager>
{
    public GameObject[] prefabs;
    private List<GameObject>[] pools;

    private new void Awake()
    {
        base.Awake();
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject GetArrow(Vector3 targetPos)
    {
        GameObject arrow = Get(0);

        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        if (arrowComponent != null)
        {
            arrowComponent.Setup(targetPos);
        }

        return arrow;
    }
}
