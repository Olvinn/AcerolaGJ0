using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private Image _color;
    [SerializeField] private TextMeshProUGUI _label;

    private Camera _camera;
    private Vector3 _pos;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void SetHint(Color color, string text, Vector3 pos)
    {
        _color.color = color;
        _label.text = text;
        _pos = pos;
    }

    private void LateUpdate()
    {
        transform.position = _camera.WorldToScreenPoint(_pos);
    }
}
