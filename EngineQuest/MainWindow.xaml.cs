using EngineQuest.Logic;
using System.Windows;
using System.Windows.Controls;

namespace EngineQuest
{
    /// <summary>
    /// Класс MainWindow представляет собой основное окно приложения.
    /// Здесь реализована логика взаимодействия с пользователем и обновления интерфейса.
    /// </summary>
    public partial class MainWindow : Window
    {
        private NovelScript _script;
        private int _currentSceneIndex;

        public MainWindow()
        {
            InitializeComponent();
            _script = new NovelScript();
            _currentSceneIndex = 0;
        }

        // Загрузка сценария
        private void OnLoadScript(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text Files|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                _script.LoadScript(fileDialog.FileName);
                DisplayScene(_script.GetScene(_currentSceneIndex));
            }
        }

        // Отображение сцены
        private void DisplayScene(NovelScene scene)
        {
            DialogueText.Text = scene.Dialogue;
            ChoiceButton1.Content = scene.Choice1Text;
            ChoiceButton2.Content = scene.Choice2Text;

            if (!string.IsNullOrEmpty(scene.Music))
            {
                PlayMusic(scene.Music);
            }

            ChoiceButton1.Visibility = string.IsNullOrEmpty(scene.Choice1Text) ? Visibility.Collapsed : Visibility.Visible;
            ChoiceButton2.Visibility = string.IsNullOrEmpty(scene.Choice2Text) ? Visibility.Collapsed : Visibility.Visible;
        }

        // Проигрывание музыки
        private void PlayMusic(string musicFile)
        {
            // Используем MediaPlayer для проигрывания музыки
            var mediaPlayer = new System.Windows.Media.MediaPlayer();
            mediaPlayer.Open(new Uri(musicFile, UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }

        // Обработка выбора
        private void OnChoiceClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string choice = clickedButton?.Content.ToString();

            var nextSceneIndex = _script.GetNextSceneIndex(choice);
            if (nextSceneIndex != -1)
            {
                _currentSceneIndex = nextSceneIndex;
                DisplayScene(_script.GetScene(_currentSceneIndex));
            }
        }
    }
}