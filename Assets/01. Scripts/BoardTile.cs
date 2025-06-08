using UnityEngine;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour
{
    public int x, y;
    public ChessPiece pieceOnTile;

    public void Highlight(bool on)
    {
        GetComponent<Renderer>().material.color = on ? Color.yellow : Color.white;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // 보드 클릭 로직 실행
            if (pieceOnTile == null)
            {
                TowerPlacementUI.Instance.ShowUpgradeOptions(this); // ShowUI → ShowUpgradeOptions
            }
            else
            {
                TowerPlacementUI.Instance.ShowUpgradeOptions(this); // 업그레이드 UI 표시

            }
        }


    }
}
