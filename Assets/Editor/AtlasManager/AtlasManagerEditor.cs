using System.Collections.Generic;
using System.IO;
using Animation;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AtlasManager))]
// 配置AtlasManger的Inspector窗口,放在Assets/Editor文件夹下即可
public class AtlasManagerEditor : Editor
{
    private AtlasManager _am;
    private bool _dirty;

    private const string PathToCharacterTextures = "/Sprites/Standard/Character/";
    private const string PathToEquipmentTextures = "/Sprites/Standard/Equipment/";

    public override void OnInspectorGUI()
    {
        _am = target as AtlasManager;
        serializedObject.Update();
        _dirty = false;
        if (_am?.SpriteList == null)
        {
            _am = _am ?? new AtlasManager();
            _am.SpriteList = new List<Sprite>();
            _dirty = true;
        }

        // Button(): true when the users clicks the button.
        if (GUILayout.Button("Load"))
        {
            UnloadSprites();
            var characterSprites = Directory.GetFiles(Application.dataPath + PathToCharacterTextures, "*",
                SearchOption.AllDirectories);
            var equipmentSprites = Directory.GetFiles(Application.dataPath + PathToEquipmentTextures, "*",
                SearchOption.AllDirectories);
            LoadFiles(characterSprites);
            LoadFiles(equipmentSprites);
            
            // update the model-list text file
            using (StreamWriter outputFile=new StreamWriter("model-list.txt"))
            {
                foreach (string model in _am.ModelList)
                {
                    outputFile.WriteLine(model);
                }
            }
        }

        if (GUILayout.Button("Unload"))
        {
            UnloadSprites();
        }

        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }

    void LoadFiles(IEnumerable<string> files)
    {
        foreach (string file in files)
        {
            string filepath = file.Replace("\\", "/");
            if (!filepath.EndsWith(".png"))
            {
                continue;
            }

            filepath = filepath.Replace(Application.dataPath, "");
            UpdateModelList(filepath);
        }
    }

    void UpdateModelList(string filepath)
    {
        // Add the spritesheet (model) to the model list 
        var pathBranch = filepath.Split('/');
        string prefix = "";
        for (int i = 0; i < pathBranch.Length-1; i++)
        {
            string node = pathBranch[i];
            var splitNode = node.Split('.');
            prefix += $"{splitNode[0]}_";
        }

        prefix += $"{pathBranch[pathBranch.Length - 1]}";
        prefix = prefix.Replace(".png", "");
        _am.ModelList.Add(prefix);
        _am.ModelTotal++;
    }

    void UnloadSprites()
    {
        _am.ModelList.Clear();
        _am.SpriteList.Clear();
        _am.ModelsLoaded = 0;
        _am.ModelTotal = 0;
    }
}