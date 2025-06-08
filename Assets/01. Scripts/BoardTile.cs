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
            // ���� Ŭ�� ���� ����
            if (pieceOnTile == null)
            {
                TowerPlacementUI.Instance.ShowUpgradeOptions(this); // ShowUI �� ShowUpgradeOptions
            }
            else
            {
                TowerPlacementUI.Instance.ShowUpgradeOptions(this); // ���׷��̵� UI ǥ��

            }
        }


    }
}
