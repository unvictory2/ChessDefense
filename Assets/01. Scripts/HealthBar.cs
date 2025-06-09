using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;
    public Vector3 offset = new Vector3(0, 2f, 0); // �ν����Ϳ��� �� �����ո��� ���� ����

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // ��ġ ����
            transform.position = transform.parent.position + offset;
            // ī�޶� �ٶ󺸵��� ȸ��
            transform.LookAt(transform.position + _cameraTransform.forward, _cameraTransform.up);
        }
    }

    public void SetHealth(float current, float max)
    {
        fill.fillAmount = current / max;
    }
}
