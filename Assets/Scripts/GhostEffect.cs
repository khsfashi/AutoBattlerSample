using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEffect : MonoBehaviour
{
    [SerializeField] private int _numberOfGhosts = 10;
    [SerializeField] private Color _ghostColor = Color.white;
    [SerializeField] private float _lifetime = 1.0f;
    [SerializeField] private float _spawnInterval = 0.1f;

    private List<GhostRenderer> _ghostRenderers;
    private SpriteRenderer _objectSpriteRenderer;

    private Vector3 _lastPosition;

    void Start()
    {
        _objectSpriteRenderer = GetComponent<SpriteRenderer>();
        if( _objectSpriteRenderer == null)
        {
            Debug.LogError("GhostEffect Class requires SpriteRenderer Component on the GameObject");
            enabled = false;
            return;
        }
        _ghostRenderers = new List<GhostRenderer>();
        for (int i = 0; i < _numberOfGhosts; i++)
        {
            GameObject ghostObject = new GameObject("GhostRenderer_" + i);
            ghostObject.transform.SetParent(null);
            GhostRenderer ghostRenderer = ghostObject.AddComponent<GhostRenderer>();
            ghostRenderer.gameObject.SetActive(false);
            _ghostRenderers.Add(ghostRenderer);
        }

        _lastPosition = transform.position;
        StartCoroutine(CheckPositionCoroutine());
    }

    private IEnumerator CheckPositionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (transform.position != _lastPosition)
            {
                SpawnGhost();
                _lastPosition = transform.position; // 이동 후 현재 위치 저장
            }
        }
    }

    private void SpawnGhost()
    {
        GhostRenderer leastLifeGhost = GetLeastLifeGhost();

        if (leastLifeGhost != null)
        {
            leastLifeGhost.Initialize(
                transform.position,
                _objectSpriteRenderer,
                _ghostColor,
                _lifetime
            );
        }
    }

    private GhostRenderer GetLeastLifeGhost()
    {
        GhostRenderer leastLifeGhost = null;
        float minAlpha = float.MaxValue;

        foreach (var ghost in _ghostRenderers)
        {
            if (!ghost.gameObject.activeSelf)
            {
                return ghost;
            }

            SpriteRenderer sr = ghost.ghostSpriteRenderer;
            float currentAlpha = sr.color.a;

            if (currentAlpha < minAlpha)
            {
                minAlpha = currentAlpha;
                leastLifeGhost = ghost;
            }
        }

        return leastLifeGhost;
    }
}
