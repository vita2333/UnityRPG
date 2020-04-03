using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Types;
using UnityEngine;

namespace UI
{
    public class CharacterDNAManange : MonoBehaviour
    {
        /**
         * Body => 1
         */
        public Dictionary<string, int> ModelIndex { get; } = DNABlockType.TypeList.ToDictionary(bt => bt, v => 1);
        public string Gender = "male";

        /**
         * Body => [ modelKey1, modelKey2 ... ]
         */
        private Dictionary<string, List<string>> _blockModelLookup =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => new List<string>());

        private void Start()
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader streamReader = new StreamReader("model-list.txt"))
            {
                // Read and display lines from the file until the end of  the file is reached.
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string blockType = line.Split('_')[0].ToUpper();
                    _blockModelLookup[blockType].Add(line);
                }
            }
        }

        private void OnGUI()
        {
            float x = 10;
            float y = 10;
            float w = 50;
            float h = 20;
            float margin = 20;

            // gender
            GUI.Label(new Rect(x, y, w, h), "GENDER:");
            if (GUI.Button(new Rect(x + w + margin, y, w, h), "male"))
            {
                Gender = "male";
                
            }

            if (GUI.Button(new Rect(x + w * 2 + margin, y, w, h), "female")) { Gender = "female"; }

            y += 20;

// DNA
            foreach (string blockType in DNABlockType.TypeList)
            {
                int index = ModelIndex[blockType];
                GUI.Label(new Rect(x, y, w, h), $"{blockType}:");
                if (GUI.Button(new Rect(x + w + margin, y, w, h), "-"))
                {
                    index--;
                    if (index < 1) { index = _blockModelLookup[blockType].Count ; }
                }

                GUI.Label(new Rect(x + w * 2 + margin, y, w, h), index.ToString());
                if (GUI.Button(new Rect(x + w * 3 + margin, y, w, h), "+"))
                {
                    index++;
                    if (index > _blockModelLookup[blockType].Count) { index = 1; }
                }

                ModelIndex[blockType] = index;
                y += 20;
            }
            
            GUI.Box(new Rect(300,10,300,500), ObjectDumper.Dump(_blockModelLookup));
        }
    }
}