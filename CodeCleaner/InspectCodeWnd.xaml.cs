using System;
using System.Diagnostics;
using System.IO;
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

namespace CodeCleanerSpace
{
  /// <summary>
  /// Interaction logic for InspectCodeWnd.xaml
  /// </summary>
  public partial class InspectCodeWnd : BaseDialogWindow
  {
    static private Dictionary<string, string> cmdMap = new Dictionary<string, string>()
    {
      ["rmTrailingSpaceBox"] = "RmTrailingSpaces",
      ["crctFuncHeadsBox"] = "FixFuncsHeads",
    };

    private List<string> modulesToRun = new List<string>() {};

    public InspectCodeWnd()
    {
      InitializeComponent();

      this.rmTrailingSpaceBox.Checked   += checkBox_Checked;
      this.rmTrailingSpaceBox.Unchecked += checkBox_UnChecked;

      this.crctFuncHeadsBox.Checked   += checkBox_Checked;
      this.crctFuncHeadsBox.Unchecked += checkBox_UnChecked;
 
      this.checkAllBox.Checked   += allBox_Checked;
      this.checkAllBox.Unchecked += allBox_UnChecked;
    }

    private void OnCancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void OnRun(object sender, RoutedEventArgs e)
    {
      // find document path
      ActivePathHolder theHolder = ActivePathHolder.getInstance();
      string activeDocumentPath = theHolder.activeFilePath;
      string tmpDocumentPath = theHolder.tmpFilePath;

      // To copy a file to another location and 
      // overwrite the destination file if it already exists.
      System.IO.File.Copy(activeDocumentPath, tmpDocumentPath, true);


      foreach ( var module in modulesToRun)
      {
        ExecuteFixCommand( module );
      }
      this.Close();

      var documentationControl = new OpenDiffToolAndSaveWnd();
      documentationControl.ShowModal();
    }

    private void ExecuteFixCommand( string moduleName)
    {
      string folderPath = "C:\\Users\\belyakov\\Documents\\Visual Studio 2015\\Projects\\RefactorFiles\\RefactorFiles";
      //string folderPath = CodeCleaner.Default.ScriptDirectory;

      // find document path
      ActivePathHolder theHolder = ActivePathHolder.getInstance();
      string activeDocumentPath = theHolder.activeFilePath;
      string tmpDocumentPath = theHolder.tmpFilePath;

      string strCmdText = "-c \"import sys; sys.path.insert(0, r'" + folderPath + "');"
        + "import ScriptsMgr as smgr; "
        + "smgr.fix_with_script(r'" + tmpDocumentPath + "', '" + moduleName + "');\" ";

      var procStIfo = new ProcessStartInfo("py", strCmdText);
      procStIfo.RedirectStandardOutput = true;
      procStIfo.UseShellExecute = false;
      procStIfo.CreateNoWindow = true;

      var proc = new Process();
      proc.StartInfo = procStIfo;
      proc.Start();
      proc.WaitForExit();
    }

    private void checkBox_Checked(object sender, RoutedEventArgs e)
    {
      CheckBox box = sender as CheckBox;
      modulesToRun.Add(cmdMap[box.Name]);
    }

    private void checkBox_UnChecked(object sender, RoutedEventArgs e)
    {
      CheckBox box = sender as CheckBox;
      modulesToRun.Remove(cmdMap[box.Name]);
    }

    private void allBox_Checked(object sender, RoutedEventArgs e)
    {
      var allBox = sender as CheckBox;
      string ownName = allBox.Name;

      setCheckedForAllBoxes(true, ownName);
    }

    private void allBox_UnChecked(object sender, RoutedEventArgs e)
    {
      var allBox = sender as CheckBox;
      string ownName = allBox.Name;

      setCheckedForAllBoxes(false, ownName);
    }

    private void setCheckedForAllBoxes(bool isChecked, string exceptName)
    {
      UIElementCollection elements = this.boxesStackPanel.Children;

      foreach (var element in elements)
      {
        CheckBox box = element as CheckBox;
        if (!box.Name.Equals(exceptName))
        {
          box.IsChecked = isChecked;
        }
      }
    }
  }
}
