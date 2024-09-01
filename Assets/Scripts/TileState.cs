using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tile State", order = 51)]
public class TileState : ScriptableObject
{
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private Color _textColor;
    [SerializeField] private Texture2D _image;

    public Color BackgroundColor { get { return _backgroundColor; } set { _backgroundColor = value; } }
    public Color TextColor { get { return _textColor; } set { _textColor = value; } }
    public Texture2D Image { get { return _image; } set { _image = value; } }
}
