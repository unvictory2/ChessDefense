using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPlacementUI : MonoBehaviour
{
    public static TowerPlacementUI Instance;

    [System.Serializable]
    public class TowerTypeInfo
    {
        public string type;      // ��: "Pawn", "Knight"
        public GameObject prefab;// ��: TowerPawn, TowerKnight
        public Sprite icon;      // ��: pawnIcon, knightIcon
    }
    [SerializeField] private TowerTypeInfo[] towerTypeInfos;

    [Header("UI Components")]
    public GameObject uiPanel;
    public Transform buttonContainer;
    public GameObject upgradeButtonPrefab;

    private BoardTile _selectedTile;
    private GameObject[] _activeButtons = new GameObject[2];

    void Awake()
    {
        Instance = this;
        uiPanel.SetActive(false);
    }

    public void ShowUpgradeOptions(BoardTile tile)
    {
        _selectedTile = tile;
        ClearButtons();

        Vector3 worldPos = tile.transform.position + Vector3.up * 2f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        uiPanel.transform.position = screenPos;

        if (tile.pieceOnTile == null)
        {
            CreateUpgradeButton("Pawn", 50, 0);
        }
        else
        {
            ChessPiece piece = tile.pieceOnTile;
            if (piece is Pawn)
            {
                CreateUpgradeButton("Knight", 100, 0);
                CreateUpgradeButton("Bishop", 100, 1);
            }
            else if (piece is Knight || piece is Bishop)
            {
                CreateUpgradeButton("Rook", 150, 0);
            }
            else if (piece is Rook)
            {
                CreateUpgradeButton("Queen", 200, 0);
            }
        }

        uiPanel.SetActive(true);
    }

    void CreateUpgradeButton(string pieceType, int cost, int index)
    {
        GameObject button = Instantiate(upgradeButtonPrefab, buttonContainer);
        button.GetComponentInChildren<TextMeshProUGUI>().text = $"{cost}G"; // �빮�� G�� ����

        // Grid Layout Group ����ϹǷ� ��ġ ���� �ڵ� ����

        // �ڽ� Image ������Ʈ ã�� (��ư�� �ڽ� �� Image ������Ʈ)
        Image childImage = null;
        foreach (Transform child in button.transform)
        {
            childImage = child.GetComponent<Image>();
            if (childImage != null && child != button.transform) // ��ư �ڽ��� Image�� �ƴ� �ڽ��� Image�� ã��
                break;
        }

        if (childImage != null)
        {
            TowerTypeInfo info = GetInfoByType(pieceType);
            if (info != null && info.icon != null)
                childImage.sprite = info.icon;
        }

        button.GetComponent<Button>().onClick.AddListener(() =>
            HandleUpgradeClick(pieceType, cost));

        _activeButtons[index] = button;
    }

    TowerTypeInfo GetInfoByType(string type)
    {
        foreach (var info in towerTypeInfos)
        {
            if (info.type.Equals(type, System.StringComparison.OrdinalIgnoreCase))
                return info;
        }
        return null;
    }

    void HandleUpgradeClick(string type, int cost)
    {
        Debug.Log($"���׷��̵� �õ�: {type}");
        if (!GoldManager.Instance.SpendGold(cost)) return;

        if (_selectedTile.pieceOnTile != null)
        {
            UpgradeTower(type);
        }
        else
        {
            PlaceTower(type);
        }
        HideUI();
    }

    void PlaceTower(string pieceType)
    {
        TowerTypeInfo info = GetInfoByType(pieceType);
        if (info == null || info.prefab == null)
        {
            Debug.LogError($"{pieceType} Ÿ�� ������ ����");
            return;
        }

        GameObject tower = Instantiate(
            info.prefab,
            _selectedTile.transform.position,
            Quaternion.identity,
            DynamicObjects.ChessPieces
        );

        _selectedTile.pieceOnTile = tower.GetComponent<ChessPiece>();
        _selectedTile.pieceOnTile.CurrentTile = _selectedTile;
    }

    void UpgradeTower(string pieceType)
    {
        Destroy(_selectedTile.pieceOnTile.gameObject);
        PlaceTower(pieceType);
    }

    public void HideUI()
    {
        ClearButtons();
        uiPanel.SetActive(false);
        _selectedTile = null;
    }

    void ClearButtons()
    {
        foreach (var button in _activeButtons)
        {
            if (button != null)
                Destroy(button);
        }
        System.Array.Clear(_activeButtons, 0, _activeButtons.Length);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && uiPanel.activeInHierarchy)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<BoardTile>() != _selectedTile)
                {
                    HideUI();
                }
            }
        }
    }
}
