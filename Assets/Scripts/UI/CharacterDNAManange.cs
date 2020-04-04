using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Animation;
using Character;
using Core;
using Types;
using UnityEngine;

namespace UI
{
    public class CharacterDNAManange : MonoBehaviour
    {
        /**
         * Body => 1 . Notice that start with 1 rather than 0.
         */
        public Dictionary<string, int> ModelIndex { get; private set; } =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => -1);

        public string Gender = "male";

        /**
         * Body => [ male => [ modelKey1, modelKey2 ... ]]
         */
        private Dictionary<string, Dictionary<string, List<string>>> _blockModelLookup =
            DNABlockType.TypeList.ToDictionary(bt => bt, v => new Dictionary<string, List<string>>()
            {
                {"male", new List<string>()},
                {"female", new List<string>()},
                {"both", new List<string>()}
            });

        private void Start()
        {
            InitializeBlockModelLookup();
            InitializeCharacterDNA();

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
            float w = 50;
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
                    int index = ModelIndex[blockType];

                    GUI.Label(new Rect(x, y, w, h), $"{blockType}:");
                    if (GUI.Button(new Rect(x + w + margin, y, w, h), "-"))
                    {
                        index--;
                        if (index < -1) { index = genderModelLookup.Count - 1; }

                        Debug.Log(ObjectDumper.Dump(genderModelLookup));
                    }


                    string modelText;
                    if (index > -1)
                    {
                        modelText = genderModelLookup[index].Replace($"{blockType.ToLower()}_{Gender}_", "")
                            .Replace($"{blockType.ToLower()}_both_", "");
                    }
                    else { modelText = "无"; }

                    GUI.Label(new Rect(x + w * 2 + margin, y, w, h), modelText);
                    if (GUI.Button(new Rect(x + w * 3 + margin, y, w, h), "+"))
                    {
                        index++;
                        if (index > genderModelLookup.Count - 1) { index = -1; }

                        Debug.Log(ObjectDumper.Dump(genderModelLookup));
                    }


                    ModelIndex[blockType] = index;
                    y += 20;
                    Player.CharacterDNA.UpdateBlock(blockType, index > -1 ? genderModelLookup[index] : "", new Color());
                }
            }
        }


        void ResetModelIndex()
        {
            foreach (string blockType in DNABlockType.TypeList) { ModelIndex[blockType] = -1; }
        }
    }
}