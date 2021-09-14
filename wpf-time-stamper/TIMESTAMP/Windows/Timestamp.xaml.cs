using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TIMESTAMP.Windows
{
    public partial class Timestamp : Window
    {
        private MediaPlayer mediaPlayer = new();
        private DispatcherTimer ticker = new();

        private bool _fileSelected { get; set; } = false;
        private bool _playing { get; set; } = false;


        public Timestamp()
        {
            InitializeComponent();
            RefreshButtons();
            InitTimer();
        }

        //MISC
        private void RefreshButtons()
        {
            playButton.IsEnabled = _fileSelected;
            recordButton.IsEnabled = _fileSelected && _playing;
        }
        private void InitTimer()
        {
            ticker = new DispatcherTimer();
            ticker.Interval = TimeSpan.FromMilliseconds(10);
            ticker.Tick += ticker_Tick_Second;
            ticker.Start();
        }
        private void ticker_Tick_Second(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                fileLengthLabel.Content = 
                    $"{mediaPlayer.Position.ToString(@"mm\:ss")} / " +
                    $"{mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
            }
            else if (fileNameBox.Text.Equals(String.Empty)) //not working
            {
                fileLengthLabel.Content = "00:00 / 00:00";
            }
        }

        //MP3 management
        private void selectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*",
                Title  = "Select a file to load"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
            }
            _fileSelected = !openFileDialog.FileName.Equals("");
            RefreshButtons();
            fileNameBox.Text = openFileDialog.SafeFileName;
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_playing)
            {
                _playing = true;
                mediaPlayer.Play();
                playButton.Content = "Pause";
                RefreshButtons();
                Keyboard.ClearFocus();
                Keyboard.Focus(recordButton);
            }
            else if (_playing)
            {
                _playing = false;
                mediaPlayer.Pause();
                playButton.Content = "Play";
                RefreshButtons();
            }
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _playing = false;
            mediaPlayer.Stop();
            timestampList.Items.Clear();
            timestampCount = 1;
            playButton.Content = "Play";

        }

        //Timestamping
        private int timestampCount = 1;
        private Label timestampLabel;
        private StringBuilder contentBuilder = new StringBuilder();

        private bool spaceToggled = false;
        private bool SpaceToggled
        {
            get => spaceToggled;
            set
            {
                if (spaceToggled != value)
                {
                    if (!spaceToggled)
                    {
                        contentBuilder
                            .Append($"{timestampCount.ToString()}\t|\t")
                            .Append(mediaPlayer.Position.ToString("c"));
                    }
                    spaceToggled = value;
                }
            }
        }

        private void UpdateListbox(ListBox listbox)
        {
            listbox.Items.Add(timestampLabel);
            listbox.Items.MoveCurrentToLast();
            listbox.ScrollIntoView(listbox.Items.CurrentItem);
        }
        
        private void recordButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SpaceToggled = true;
            }
        }

        private void recordButton_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SpaceToggled = false;
                timestampLabel = new Label
                {
                    Content = contentBuilder.ToString(),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Yellow
                };
                
                //_fullList += contentBuilder.ToString();   //join?

                UpdateListbox(timestampList);
                _ = contentBuilder.Clear();
                timestampCount++;
            }
        }

        //Method to serialize output into json
        private void ParseToJSON(Output output)
        {
            JsonSerializer serializer = new JsonSerializer();
            //serializer.Serialize(output);
        }
        private void findSaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            //choose a file to save
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //save data
        }
    }
}
