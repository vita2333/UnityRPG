using UnityEngine;

namespace Character
{
    public class CharacterDNABlock
    {
        /**
         * Name of the animation frame, exclude direction.
         * eg: "body_female_light"
         */
        public string ModelKey;

        public Color ItemColor;

        /**
          * Controlling the availability of DNA blocks.
         */
        public bool Enabled;
        /**
         * Set to true to render a new animation.
         */
        public bool IsDirty;

        public CharacterDNABlock()
        {
            ModelKey = "UNKNOWN";
            ItemColor = new Color();
            Enabled = false;
            IsDirty = false;
        }

        public CharacterDNABlock(string itemKey)
        {
            Update(itemKey, new Color());
        }

        public CharacterDNABlock(string itemKey, Color itemColor)
        {
            Update(itemKey, itemColor);
        }

        public void Update(string modelKey, Color itemColor)
        {
            ModelKey = modelKey;
            Debug.Log("modelKey===" + modelKey);
            ItemColor = itemColor;
            IsDirty = true;
            // disable the character block if there is no model key
            Enabled = modelKey.Length > 0;
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