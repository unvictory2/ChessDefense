using UnityEngine;

public interface IAttackStrategy
{
    void Attack(ChessPiece piece);
    void ShowAttackRange(ChessPiece piece);
    float GetAttackRange();
    float GetAttackRate();
}