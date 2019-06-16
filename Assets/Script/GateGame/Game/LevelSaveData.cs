using System;

namespace GateGame.Game
{
   /// <summary>
    /// A calss to save level data
    /// </summary>
    [Serializable]
    public class LevelSaveData
    {
        public string id;

        public LevelSaveData(string levelId)
        {
            id = levelId;
        }
    }
}



