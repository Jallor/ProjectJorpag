using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using static TileRuleCreatorWindowEditor;

[CreateAssetMenu(fileName = "TileRuleConfig", menuName = "Editor/Tile Rule Config")]
public class TileRuleCreatorConfigScriptObj : ScriptableObject
{
    [System.Serializable]
    public class TileConfig
    {
        public TileCornerType CornerTopLeft = TileCornerType.FULL_CORNER;
        public TileCornerType CornerTopRight = TileCornerType.FULL_CORNER;
        public TileCornerType CornerBottomLeft = TileCornerType.FULL_CORNER;
        public TileCornerType CornerBottomRight = TileCornerType.FULL_CORNER;

        [Tooltip("If true, it will generate 4 tile by rotating this config")]
        public bool RotateConfig = false;

        [Tooltip("Not used in code")] [ShowAssetPreview]
        [SerializeField] private Sprite _TemplateSprite;
    }

    public List<TileConfig> TileConfigList = new List<TileConfig>();
}
