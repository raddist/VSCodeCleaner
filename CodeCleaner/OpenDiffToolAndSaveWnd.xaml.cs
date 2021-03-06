﻿using System;
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
  /// Interaction logic for ChangeDirectoryWindow.xaml
  /// </summary>
  public partial class OpenDiffToolAndSaveWnd : BaseDialogWindow
  {
    public OpenDiffToolAndSaveWnd()
    {
      InitializeComponent();
    }

    //public static DependencyProperty DirectoryTextProperty =
    //    DependencyProperty.Register("DirectoryText", typeof(string), typeof(ChangeDirectoryWindow));

    private void OnCancel(object sender, RoutedEventArgs e)
    {
      RemoveTmpFile();
      this.Close();
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
      RewriteActiveFile();

      RemoveTmpFile();
      this.Close();
    }

    private void OnOpenDiffTool(object sender, RoutedEventArgs e)
    {
      ActivePathHolder theHolder = ActivePathHolder.getInstance();
      string activeDocumentPath = theHolder.activeFilePath;
      string tmpDocumentPath = theHolder.tmpFilePath;

      string strCmdText;
      strCmdText = "\"" + activeDocumentPath + "\" \"" + activeDocumentPath + "\" \"" + tmpDocumentPath + "\"";// -o \"temp.cpp\"";

      //string diffToolDirectory = CodeCleaner.Default.DiffToolDirectory; TODO_uncomment
      string diffToolDirectory = "C:\\Program Files\\KDiff3\\kdiff3.exe";
      System.Diagnostics.Process.Start(diffToolDirectory, strCmdText);
    }

    private void RemoveTmpFile()
    {
      ActivePathHolder theHolder = ActivePathHolder.getInstance();
      string tmpDocumentPath = theHolder.tmpFilePath;

      if (File.Exists(tmpDocumentPath))
      {
        File.Delete(tmpDocumentPath);
      }
    }

    private void RewriteActiveFile()
    {
      ActivePathHolder theHolder = ActivePathHolder.getInstance();
      string activeDocumentPath = theHolder.activeFilePath;
      string tmpDocumentPath = theHolder.tmpFilePath;

      // To copy a file to another location and 
      // overwrite the destination file if it already exists.
      System.IO.File.Copy(tmpDocumentPath, activeDocumentPath, true);
    }
  }
}