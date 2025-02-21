using System.IO;

namespace EngineQuest.Logic
{
    public class NovelScript
    {
        private List<NovelScene> _scenes;
        private int _currentSceneIndex; // Индекс текущей сцены

        public NovelScript()
        {
            _scenes = new List<NovelScene>();
        }

        // Загрузка сценария
        public void LoadScript(string filePath)
        {
            _scenes.Clear();
            var lines = File.ReadAllLines(filePath);
            NovelScene currentScene = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("#start"))
                {
                    currentScene = new NovelScene();
                }
                else if (line.StartsWith("MUSIC"))
                {
                    currentScene.Music = line.Replace("MUSIC ", "").Trim();
                }
                else if (line.StartsWith("choice"))
                {
                    var choices = line.Replace("choice ", "").Split(';');
                    currentScene.Choice1Text = choices[0].Split('-')[0].Trim().Replace("\"", "");
                    currentScene.Choice2Text = choices[1].Split('-')[0].Trim().Replace("\"", "");
                    currentScene.Scene1Jump = choices[0].Split('-')[1].Trim();
                    currentScene.Scene2Jump = choices[1].Split('-')[1].Trim();
                }
                else if (line.StartsWith("@"))
                {
                    var parts = line.Substring(1).Split(':');
                    var character = parts[0].Trim();
                    var dialogue = parts[1].Trim();
                    currentScene.AddDialogue(character, dialogue);
                }
                else if (line.StartsWith("jump"))
                {
                    var sceneName = line.Replace("jump ", "").Replace("\"", "").Trim();
                    currentScene.SceneJump = sceneName;
                    _scenes.Add(currentScene);
                    currentScene = null;
                }
            }
        }

        // Получение сцены по индексу
        public NovelScene GetScene(int index)
        {
            return _scenes.ElementAtOrDefault(index);
        }

        // Получение следующей сцены по выбору
        public int GetNextSceneIndex(string choice)
        {
            // Получаем текущую сцену
            var currentScene = _scenes[_currentSceneIndex];

            // Находим индекс следующей сцены, сравнивая имя сцены с названиями в Scene1Jump и Scene2Jump
            int nextSceneIndex = -1;

            if (choice == currentScene.Choice1Text)
            {
                nextSceneIndex = _scenes.FindIndex(scene => scene.SceneJump == currentScene.Scene1Jump);
            }
            else if (choice == currentScene.Choice2Text)
            {
                nextSceneIndex = _scenes.FindIndex(scene => scene.SceneJump == currentScene.Scene2Jump);
            }

            return nextSceneIndex;

        }
    }
}
