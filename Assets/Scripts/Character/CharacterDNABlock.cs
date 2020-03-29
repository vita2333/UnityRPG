using UnityEngine;

namespace Character
{
    public class CharacterDNABlock
    {
        public string ModelKey;
        public Color ItemColor;
        public bool Enabled;
        public bool IsDirty;

        public CharacterDNABlock()
        {
            ModelKey = "UNKNOWN";
            ItemColor = new Color();
            Enabled = false;
            IsDirty = false;  // todo ???
        }

        public CharacterDNABlock(string itemKey)
        {
            Update(itemKey, new Color());
        }

        public CharacterDNABlock(string itemKey, Color itemColor)
        {
            Update(itemKey, itemColor);
        }

        public void Update(string itemKey, Color itemColor)
        {
            ModelKey = itemKey;
            ItemColor = itemColor;
            IsDirty = true;
            // disable the character block if there is no model key
            Enabled = itemKey.Length > 0;
        }

        public void UpdateColor(Color itemColor)
        {
            ItemColor = itemColor;
            Enabled = true;
            IsDirty = true;
        }

        public void UpdateModel(string modelKey)
        {
            ModelKey = modelKey;
            IsDirty = true;
            // disable the character block if there is no model key
            Enabled = modelKey.Length > 0;           

        }
    }
}