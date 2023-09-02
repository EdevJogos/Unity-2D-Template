using System.Collections.Generic;
using UnityEngine;
using static CharactersDatabase;

namespace ETemplate.Manager
{
    public class AgentsManager : Manager
    {
        [SerializeField] private Character _charaterPrefab;

        public List<int> CharacterIndexes { get; private set; } = new() { 0 };

        public override void Initiate()
        {

        }

        public override void Initialize()
        {

        }

        public void CreateCharacters()
        {
            for (int __i = 0; __i < CharacterIndexes.Count; __i++)
            {
                InputListener __inputListener = InputManager.GetInputListener(__i);
                CharacterData __characterData = GetCharacterData(CharacterIndexes[__i]);
                //Instantiate(_charaterPrefab).Setup(__i, __characterData, __inputListener);
            }
        }
    }
}
