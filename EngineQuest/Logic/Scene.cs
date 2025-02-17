namespace EngineQuest.Logic
{
    public class Scene
    {
        public string Id { get; set; }
        public string Speaker { get; set; }
        public string Text { get; set; }
        public string Background { get; set; }
        public string Music { get; set; }
        public List<Choice> Choices { get; set; }
    }
}
