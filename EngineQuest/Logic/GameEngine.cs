using System.Windows;

namespace EngineQuest.Logic
{
    public class GameEngine
    {
        public Dictionary<string, Scene> scenes;
        public Scene CurrentScene { get; set; }

        public void LoadScenes(Dictionary<string, Scene> loadedScenes)
        {
            scenes = loadedScenes;
        }

        public void StartGame(string startSceneId)
        {
            CurrentScene = scenes[startSceneId];
        }

        public void MakeChoice(int choiceIndex)
        {
            var nextSceneId = CurrentScene.Choices[choiceIndex].Next;

            try
            {
                CurrentScene = scenes[nextSceneId];
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show($"Сцена с id \"{nextSceneId}\" не найдена", "Ошибка!");
            }
        }
    }
}
