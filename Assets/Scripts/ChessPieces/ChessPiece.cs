using System.Collections;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IChessPiece
{
    public BoardTile CurrentTile { get; set; }

    // �� ���� (��: 0=�÷��̾�, 1=��)
    public int Team;

    public abstract void ShowMoveOptions(BoardManager board);

    public virtual void MoveTo(BoardTile targetTile)
    {
        StartCoroutine(MoveArcRoutine(targetTile.transform.position, 0.5f, 1.5f));
    }

    private IEnumerator MoveArcRoutine(Vector3 targetPos, float duration, float arcHeight)
    {
        Vector3 start = transform.position;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            // XZ ����� ���� ����
            Vector3 pos = Vector3.Lerp(start, targetPos, t);
            // Y���� ��ũ(������)
            pos.y += Mathf.Sin(Mathf.PI * t) * arcHeight;
            transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }
        // ������ ��ġ ����
        transform.position = targetPos;
        // Ÿ�� ���� ���� �� �߰� ó��
    }

}
