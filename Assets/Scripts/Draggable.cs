using UnityEngine;

/// <summary>
/// 유닛을 드래그 앤 드롭할 수 있게 해주는 스크립트입니다.
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

    /// <summary> 드래그 시작 시 sortingOrder를 조절하여 화면 가장 앞에 나오게 합니다. </summary>
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

    /// <summary>드래그 하는 유닛 아래의 타일에 대해 HIghlight 색상을 부여합니다.</summary>
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

    /// <summary>유닛 드롭 시 해당 위치가 가능한 위치면 유닛을 그 자리에 내리고, 아닐 경우 처음 자리로 돌립니다.</summary>
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

    /// <summary>해당 위치에 유닛을 두는 시도를 합니다.</summary>
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

    /// <summary>현재 충돌된 타일을 가져옵니다.</summary>
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
