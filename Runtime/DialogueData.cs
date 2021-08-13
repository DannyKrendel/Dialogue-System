using System.Collections.Generic;

namespace DialogueSystem
{
    public struct DialogueData
    {
        public string Text { get; set; }
        public string CharacterName { get; set; }
        public string[] Choices { get; set; }
    }
}