using EngineQuest.Logic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EngineQuest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameEngine engine;

        public MainWindow()
        {
            InitializeComponent();

            engine = new GameEngine();

            var parser = new ScriptParser();
            var scriptContent = File.ReadAllText("scenarios.txt");
            var scenes = parser.ParseScript(scriptContent);

            engine.LoadScenes(scenes);
            engine.StartGame("start");
            UpdateUI();
        }

        private void UpdateUI()
        {
            var scene = engine.CurrentScene;

            // Обновляем фон
            if (!string.IsNullOrEmpty(scene.Background))
            {
                string programDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fullPath = Path.Combine(programDirectory, scene.Background);
                BackgroundImage.Source = new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute));
            }

            // Обновляем музыку
            if (!string.IsNullOrEmpty(scene.Music))
            {
                BackgroundMusic.Source = new Uri(scene.Music, UriKind.RelativeOrAbsolute);
                BackgroundMusic.Play();
            }

            // Обновляем текст и имя персонажа
            SpeakerLabel.Content = scene.Speaker;
            DialogueTextBlock.Text = scene.Text;

            // Очищаем панель кнопок выбора
            ChoicesPanel.Children.Clear();

            // Генерация кнопок выбора
            if (scene.Choices != null && scene.Choices.Count > 0)
            {
                for (int i = 0; i < scene.Choices.Count; i++)
                {
                    var choice = scene.Choices[i];
                    var button = new Button
                    {
                        Content = choice.Text,
                        Tag = i,
                        Margin = new Thickness(5),
                        Padding = new Thickness(10)
                    };

                    button.Click += ChoiceButton_Click;
                    ChoicesPanel.Children.Add(button);
                }
            }
            else
            {
                // Если выборов нет, добавить кнопку "Продолжить"
                var continueButton = new Button
                {
                    Content = "Продолжить",
                    Margin = new Thickness(5),
                    Padding = new Thickness(10)
                };

                continueButton.Click += ContinueButton_Click;
                ChoicesPanel.Children.Add(continueButton);
            }
        }

        private void ChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int choiceIndex = (int)button.Tag;

            engine.MakeChoice(choiceIndex);
            UpdateUI();
        }


        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Нет доступных выборов, и игра завершена!");
        }

    }
}