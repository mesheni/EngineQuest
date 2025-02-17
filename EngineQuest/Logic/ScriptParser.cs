namespace EngineQuest.Logic
{
    public class ScriptParser
    {
        public Dictionary<string, Scene> ParseScript(string scriptContent)
        {
            var scenes = new Dictionary<string, Scene>();
            string[] lines = scriptContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Scene currentScene = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

                if (line.StartsWith("#"))
                {
                    if (currentScene != null)
                    {
                        scenes[currentScene.Id] = currentScene;
                    }
                    currentScene = new Scene
                    {
                        Id = line.Substring(1).Trim(),
                        Choices = new List<Choice>()
                    };
                }
                else if (line.StartsWith("BG"))
                {
                    if (currentScene != null)
                    {
                        currentScene.Background = line.Substring(3).Trim();
                    }
                }
                else if (line.StartsWith("MUSIC"))
                {
                    if (currentScene != null)
                    {
                        currentScene.Music = line.Substring(6).Trim();
                    }
                }
                else if (line.StartsWith("@"))
                {
                    if (currentScene != null)
                    {
                        var split = line.Substring(1).Split(new[] { ':' }, 2);
                        if (split.Length == 2)
                        {
                            currentScene.Speaker = split[0].Trim();
                            currentScene.Text = split[1].Trim();
                        }
                        else
                        {
                            throw new FormatException($"Invalid format in line: {line}. Expected format: '@Speaker: Text'");
                        }
                    }
                }
                else if (line.Contains(" - "))
                {
                    if (currentScene != null)
                    {
                        var split = line.Split(new[] { " - " }, StringSplitOptions.None);
                        if (split.Length == 2)
                        {
                            currentScene.Choices.Add(new Choice
                            {
                                Text = split[0].Trim(),
                                Next = split[1].Trim()
                            });
                        }
                        else
                        {
                            throw new FormatException($"Invalid choice format in line: {line}. Expected format: 'Choice text - next_scene_id'");
                        }
                    }
                }
            }

            if (currentScene != null)
            {
                scenes[currentScene.Id] = currentScene;
            }

            return scenes;
        }
    }

}
