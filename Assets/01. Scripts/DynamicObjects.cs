using UnityEngine;

public class DynamicObjects : MonoBehaviour
{
    public static Transform ChessPieces;
    public static Transform Enemies;

    void Awake()
    {
        // 하이라키에서 자식 오브젝트 찾기
        ChessPieces = transform.Find("ChessPieces");
        Enemies = transform.Find("Enemies");

        if (ChessPieces == null)
            Debug.LogError("ChessPieces 자식 오브젝트를 찾을 수 없습니다!");
        if (Enemies == null)
            Debug.LogError("Enemies 자식 오브젝트를 찾을 수 없습니다!");
    }
}
