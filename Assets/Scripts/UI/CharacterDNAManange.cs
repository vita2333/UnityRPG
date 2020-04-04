using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Animation;
using Character;
using Types;
using UnityEngine;

namespace UI
{
    public class CharacterDNAManange : MonoBehaviour
    {
        /**
         * Body => 1 . Start with index 0 , -1 means empty.
         */
        public Dictionary<string, int> ModelsIndex { get; private set; } =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => -1);

        public string Gender;

        public Dictionary<string, Color> ModelsColor { get; } =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => Color.clear);

        private string _colorModelKey = "";
        private string _colorBlockType = "";

        /**
         * Body => [ male => [ modelKey1, modelKey2 ... ]]
         */
        private Dictionary<string, Dictionary<string, List<string>>> _blockModelLookup =
            DNABlockType.TypeList.ToDictionary(bt => bt, v =>
                new Dictionary<string, List<string>>()
                {
                    {"male", new List<string>()},
                    {"female", new List<string>()},
                    {"both", new List<string>()}
                });

        private void Start()
        {
            InitializeBlockModelLookup();
            InitializeCharacterDNA();

            // set default
            Gender = "male";
            ModelsIndex[DNABlockType.Body] = 5;
            ModelsIndex[DNABlockType.Hair] = 0;
            ModelsIndex[DNABlockType.Chest] = 4;
            ModelsIndex[DNABlockType.Legs] = 1;
            ModelsIndex[DNABlockType.Feet] = 1;

            ModelsColor[DNABlockType.Hair] = Color.red;
            ModelsColor[DNABlockType.Feet] = Color.red;
            ModelsColor[DNABlockType.Chest] = Color.yellow;

            if (AtlasManager.Instance.ModelsLoaded <= 0)
            {
                Thread thread = new Thread(new AnimationManager().LoadAllAnimationsIntoCache);
                thread.Start();
            }
        }

        void InitializeBlockModelLookup()
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
                    string gender = line.Split('_')[1];
                    switch (gender)
                    {
                        case "male":
                            _blockModelLookup[blockType]["male"].Add(line);
                            break;
                        case "female":
                            _blockModelLookup[blockType]["female"].Add(line);
                            break;
                        case "both":
                            _blockModelLookup[blockType]["male"].Add(line);
                            _blockModelLookup[blockType]["female"].Add(line);
                            break;
                        default:
                            Debug.Log("===" + $"Unkonown gender: {gender} in {line}");
                            break;
                    }
                }
            }
        }

        void InitializeCharacterDNA()
        {
            Player.CharacterDNA = new CharacterDNA();
            Player.AnimationDNA = new AnimationDNA();
            var player = new GameObject("Player");
            player.AddComponent<PlayerController>();
        }

        private void OnGUI()
        {
            float x = 10;
            float y = 10;
            float w = 70;
            float h = 20;
            float margin = 20;

            // gender
            GUI.Label(new Rect(x, y, w, h), "GENDER:");
            foreach (var gender in new List<string> {"male", "female"})
            {
                if (GUI.Button(new Rect(x + w * (gender == "male" ? 1 : 2) + margin, y, w, h), gender))
                {
                    Gender = gender;
                    ResetModelIndex();
                }
            }

            y += 20;

            // DNA

            foreach (string blockType in DNABlockType.TypeList)
            {
                var genderModelLookup = _blockModelLookup[blockType][Gender];
                if (genderModelLookup.Count > 0)
                {
                    int index = ModelsIndex[blockType];

                    GUI.Label(new Rect(x, y, w, h), $"{blockType}:");
                    if (GUI.Button(new Rect(x + w + margin, y, w, h), "-"))
                    {
                        index--;
                        if (index < -1) { index = genderModelLookup.Count - 1; }
                    }


                    string modelText;
                    if (index > -1)
                    {
                        modelText = $"{index}_" + genderModelLookup[index]
                                        .Replace($"{blockType.ToLower()}_{Gender}_", "")
                                        .Replace($"{blockType.ToLower()}_both_", "");
                    }
                    else { modelText = "无"; }

                    GUI.Label(new Rect(x + w * 2 + margin, y, w, h), modelText);
                    if (GUI.Button(new Rect(x + w * 3 + margin, y, w, h), "+"))
                    {
                        index++;
                        if (index > genderModelLookup.Count - 1) { index = -1; }
                    }

                    if (GUI.Button(new Rect(x + w * 4 + margin, y, w, h), "Color")
                    )
                    {
                        _colorBlockType = blockType;
                        _colorModelKey = index > -1 ? genderModelLookup[index] : "";
                    }

                    ModelsIndex[blockType] = index;
                    y += 20;
                    Player.CharacterDNA.UpdateBlock(blockType, index > -1 ? genderModelLookup[index] : "",
                        ModelsColor[blockType]);
                }
            }

            // change color
            if (_colorBlockType.Length > 0)
            {
                Color color = new Color(
                    GUI.HorizontalSlider(new Rect(x, y, w, h), ModelsColor[_colorBlockType].r, 0, 1),
                    GUI.HorizontalSlider(new Rect(x, y + h, w, h), ModelsColor[_colorBlockType].g, 0, 1),
                    GUI.HorizontalSlider(new Rect(x, y + h * 2, w, h), ModelsColor[_colorBlockType].b, 0, 1),
                    ModelsColor[_colorBlockType].a);
                if (!color.Equals(ModelsColor[_colorBlockType]))
                {
                    color.a = 1; // disable transparent when change color
                    ModelsColor[_colorBlockType] = color;
                }
            }
        }


        void ResetModelIndex()
        {
            foreach (string blockType in DNABlockType.TypeList) { ModelsIndex[blockType] = -1; }
        }
    }
}