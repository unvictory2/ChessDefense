using System.Collections;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IChessPiece
{
    public BoardTile CurrentTile { get; set; }

    // 팀 구분 (예: 0=플레이어, 1=적)
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
            // XZ 평면은 선형 보간
            Vector3 pos = Vector3.Lerp(start, targetPos, t);
            // Y축은 아크(포물선)
            pos.y += Mathf.Sin(Mathf.PI * t) * arcHeight;
            transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }
        // 마지막 위치 보정
        transform.position = targetPos;
        // 타일 정보 갱신 등 추가 처리
    }

}
