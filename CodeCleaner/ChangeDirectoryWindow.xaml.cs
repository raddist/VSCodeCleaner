using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Forms;

namespace CodeCleanerSpace
{
  /// <summary>
  /// Interaction logic for ChangeDirectoryWindow.xaml
  /// </summary>
  public partial class ChangeDirectoryWindow : BaseDialogWindow
  {
    public ChangeDirectoryWindow()
    {
      InitializeComponent();

     this.PathTextBox.Text = CodeCleaner.Default.ScriptDirectory;
    }

    private String directoryText;
    public string DirectoryText
    {
      get { return directoryText; }
      set { directoryText = value; }
    }

    //public static DependencyProperty DirectoryTextProperty =
    //    DependencyProperty.Register("DirectoryText", typeof(string), typeof(ChangeDirectoryWindow));

    private void OnCancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
      CodeCleaner.Default.ScriptDirectory = this.PathTextBox.Text;
      this.Close();
    }

    private void OnBrowse(object sender, RoutedEventArgs e)
    {
      // find script path
      string folderPath = "";
      FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
      if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        folderPath = folderBrowserDialog1.SelectedPath;
      }

      this.PathTextBox.Text = folderPath;
    }
  }
}
