using UnityEngine;

/// <summary>
/// ������ �巡�� �� ����� �� �ְ� ���ִ� ��ũ��Ʈ�Դϴ�.
/// </summary>
public class Draggable : MonoBehaviour
{
    public LayerMask releaseMask;
    public Vector3 dragOffset = new Vector3(0, -0.0f, 0);
    public UnitBasement parentUnit;

    private Camera cam;
    private SpriteRenderer spriteRenderer;

    private Vector3 oldPosition;
    private int oldSortingOrder;
    private Tile previousTile = null;

    private bool IsDragging = false;

    private void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary> �巡�� ���� �� sortingOrder�� �����Ͽ� ȭ�� ���� �տ� ������ �մϴ�. </summary>
    public void OnStartDrag()
    {
        if (GameManager.Instance.curState != GameState.Prepare)
            return;
        if (parentUnit.GetTeam() == Team.Team2) return;
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
        oldPosition = this.transform.position;
        oldSortingOrder = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = 20;
        IsDragging = true;
    }

    /// <summary>�巡�� �ϴ� ���� �Ʒ��� Ÿ�Ͽ� ���� HIghlight ������ �ο��մϴ�.</summary>
    public void OnDragging()
    {
        
        if (!IsDragging) return;

        Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        newPosition.z = 0;
        this.transform.position = newPosition;
        
        Tile tileUnder = GetTileUnder();
        if (tileUnder != null)
        {
            tileUnder.SetHighlight(true, !TileManager.Instance.GetNodeForTile(tileUnder).IsOccupied);
            if(previousTile != null && tileUnder != previousTile)
            {
                previousTile.SetHighlight(false, false);
            }
            previousTile = tileUnder;
        }
    }

    /// <summary>���� ��� �� �ش� ��ġ�� ������ ��ġ�� ������ �� �ڸ��� ������, �ƴ� ��� ó�� �ڸ��� �����ϴ�.</summary>
    public void OnEndDrag()
    {
        if (!IsDragging) return;

        if (!TryRelease())
        {
            this.transform.position = oldPosition;
        }

        if(previousTile != null)
        {
            previousTile.SetHighlight(false, false);
            previousTile = null;
        }
        
        spriteRenderer.sortingOrder = oldSortingOrder;
        IsDragging = false;
    }

    /// <summary>�ش� ��ġ�� ������ �δ� �õ��� �մϴ�.</summary>
    private bool TryRelease()
    {
        
         Tile t = GetTileUnder();
         if (t != null)
         {
             UnitBasement thisUnit = GetComponent<UnitBasement>();
             Node n = TileManager.Instance.GetNodeForTile(t);
             if (n != null && thisUnit != null)
             {
                 if (!n.IsOccupied)
                 {
                    thisUnit.CurrentNode.SetOccupied(false);
                    thisUnit.SetCurrentNode(n);
                    n.SetOccupied(true);
                    thisUnit.transform.position = n.worldPosition;
                    AudioManager.Instance.PlaySfx(AudioManager.Sfx.ButtonClick);
                    return true;
                 }
             }
         }
        

        return false;
    }

    /// <summary>���� �浹�� Ÿ���� �����ɴϴ�.</summary>
    public Tile GetTileUnder()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, releaseMask);
        if (hit.collider != null)
        {
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }
        return null;
    }
}
