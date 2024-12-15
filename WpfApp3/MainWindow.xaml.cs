using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media; 
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PathToFile = null;
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewFileClick(object sender, RoutedEventArgs e)
        {
            if (PromptToSaveChanges())
            {
                txtEditor.Clear();
                PathToFile = null;
                UpdateStatus("Создан новый файл");
            }
        }

        private void FileOpenClick(object sender, RoutedEventArgs e)
        {
            if (PromptToSaveChanges())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    PathToFile = openFileDialog.FileName;
                    txtEditor.Text = File.ReadAllText(PathToFile);
                    UpdateStatus($"Открыт файл: {PathToFile}");
                }
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PathToFile))
            {
                SaveFileAs_Click(sender, e);
            }
            else
            {
                SaveToFile(PathToFile);
            }
        }
        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                PathToFile = saveFileDialog.FileName;
                SaveToFile(PathToFile);
            }
        }
        private void PrintFile_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(txtEditor, "Печать документа");
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (PromptToSaveChanges())
            {
                Application.Current.Shutdown();
            }
        }

   
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if (txtEditor.CanUndo) txtEditor.Undo();
        }
        private void CutClick(object sender, RoutedEventArgs e)
        {
            txtEditor.Cut();
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            txtEditor.Copy();
        }

        private void PasteClick(object sender, RoutedEventArgs e)
        {
            txtEditor.Paste();
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            txtEditor.SelectedText = string.Empty;
        }
        private void FindClick(object sender, RoutedEventArgs e)
        {
            string searchText = Microsoft.VisualBasic.Interaction.InputBox("Введите текст для поиска:", "Найти");
            if (!string.IsNullOrEmpty(searchText))
            {
                int index = txtEditor.Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                {
                    txtEditor.Select(index, searchText.Length);
                    txtEditor.Focus();
                }
                else
                {
                    MessageBox.Show("Текст не найден.");
                }
            }
        }

        
        private void ReplaceClick(object sender, RoutedEventArgs e)
        {
            string findText = Microsoft.VisualBasic.Interaction.InputBox("Какой текст заменить:", "Заменить");
            if (!string.IsNullOrEmpty(findText))
            {
                string replaceText = Microsoft.VisualBasic.Interaction.InputBox("На что заменить:", "Заменить");
                txtEditor.Text = txtEditor.Text.Replace(findText, replaceText);
                UpdateStatus("Замена выполнена");
            }
        }
        private void GoToClick(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Укажите номер строки:", "Перейти");
            if (int.TryParse(input, out int lineNumber))
            {
                txtEditor.Focus();
                txtEditor.ScrollToLine(lineNumber - 1);
            }
        }
        private void SelectAllClick(object sender, RoutedEventArgs e)
        {
            txtEditor.SelectAll();
        }
        private void AddDateTimeClick(object sender, RoutedEventArgs e)
        {
            txtEditor.SelectedText = DateTime.Now.ToString();
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            txtEditor.FontSize += 2;
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            if (txtEditor.FontSize > 6) txtEditor.FontSize -= 2;
        }

        private void ResetZoom_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.FontSize = 12;
        }

      
        private void ToggleStatusBarClick(object sender, RoutedEventArgs e)
        {
            statusBar.Visibility = statusBar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void WordWrapClick(object sender, RoutedEventArgs e)
        {
            txtEditor.TextWrapping = txtEditor.TextWrapping == TextWrapping.Wrap ? TextWrapping.NoWrap : TextWrapping.Wrap;
        }

        private void ChangedText(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
            UpdateStatus("Текст изменен");
        }

        private void SaveToFile(string filePath)
        {
            File.WriteAllText(filePath, txtEditor.Text);
            isTextChanged = false;
            UpdateStatus($"Файл сохранен: {filePath}");
        }

        private bool PromptToSaveChanges()
        {
            if (isTextChanged)
            {
                var result = MessageBox.Show("Сохранить изменения?", "Блокнот", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile_Click(null, null);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private void UpdateStatus(string message)
        {
            statusBarText.Text = message;
        }
    }
}