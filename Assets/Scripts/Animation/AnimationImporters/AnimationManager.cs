     using Animation.Actions;
     using Character;
     using Types;

     namespace Animation.AnimationImporters
    {
        public class AnimationManager
        {
   
             
            public static void UpdateDNAForAction(CharacterDNA characterDNA,AnimationDNA animationDNA,BaseAction actionAnimation,string newDirection){
                foreach (var blockType in DNABlockType.TypeList)
                {
                    CharacterDNABlock characterDnaBlock = characterDNA.DNABlocks[blockType];
                    if (characterDnaBlock.Enabled)
                    {
                        
                    }
                    else
                    {
                        // Disable the animation slot if the character slot isnt enabled
                        animationDNA.DNABlocks[blockType].Enabled = false;
                    }
                    
                    
                }
            }

//            private static AnimationDNABlock GetAnimation(string modelKey,BaseAction actionAnimation,string direction)
//            {
//                // Fetches an animation from the animation store/cache
//
//            }
        }
       
    }
