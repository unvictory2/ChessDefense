using UnityEngine;

public class DynamicObjects : MonoBehaviour
{
    public static Transform ChessPieces;
    public static Transform Enemies;

    void Awake()
    {
        // ���̶�Ű���� �ڽ� ������Ʈ ã��
        ChessPieces = transform.Find("ChessPieces");
        Enemies = transform.Find("Enemies");

        if (ChessPieces == null)
            Debug.LogError("ChessPieces �ڽ� ������Ʈ�� ã�� �� �����ϴ�!");
        if (Enemies == null)
            Debug.LogError("Enemies �ڽ� ������Ʈ�� ã�� �� �����ϴ�!");
    }
}
