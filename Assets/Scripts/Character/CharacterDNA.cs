using System;
using System.Collections.Generic;
using System.Linq;
using Types;
using UnityEngine;

namespace Character
{
    public class CharacterDNA
    {
        // todo
        // This is the LPC Character DNA Block class! In a real use case, 
        // this information would be represented by both the character's 
        // stats (race, hair, etc..) as well as their armor/weapons rather 
        // than being lumped together into one class

        public Dictionary<string, CharacterDNABlock> DNABlocks { get; } =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => new CharacterDNABlock());

        public void UpdateBlock(string blockKey, string modelKey, Color color)
        {
            var dnaBlock = DNABlocks[blockKey];
            dnaBlock.Update(modelKey, color);
        }

        public bool IsDirty()
        {
            foreach (string blockKey in DNABlocks.Keys)
            {
                if (DNABlocks[blockKey].IsDirty) { return true; }
            }

            return false;
        }
    }
}