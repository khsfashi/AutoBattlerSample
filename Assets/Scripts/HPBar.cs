using UnityEngine;

/// <summary>
/// ���� ���� ü���� ��Ÿ���� HPBar�� �����մϴ�.
/// </summary>
public class HPBar : MonoBehaviour
{
    public Transform bar;
    public Vector3 offset;

    private float maxHP;
    private Transform target;

    public void SetUp(Transform target, float maxHP)
    {
        this.maxHP = maxHP;
        this.target = target;
        UpdateBar(maxHP);
    }

    public void UpdateBar(float newHP)
    {
        float newScale = newHP / maxHP;
        Vector3 scale = bar.transform.localScale;
        scale.x = newScale;
        bar.transform.localScale = scale;

    }

    private void Update()
    {
        if(target != null)
        {
            this.transform.position = target.position + offset;
        }
    }
}
