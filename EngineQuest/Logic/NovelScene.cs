namespace EngineQuest.Logic
{
    public class NovelScene
    {
        public string Dialogue { get; set; }
        public string Music { get; set; }
        public string Choice1Text { get; set; }
        public string Choice2Text { get; set; }
        public string Scene1Jump { get; set; }
        public string Scene2Jump { get; set; }
        public string SceneJump { get; set; }

        public void AddDialogue(string character, string dialogue)
        {
            Dialogue += $"{character}: {dialogue}\n";
        }
    }
}
