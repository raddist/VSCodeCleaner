//------------------------------------------------------------------------------
// <copyright file="CodeCleanerCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using EnvDTE;

namespace CodeCleaner
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal sealed class CodeCleanerCommand
  {
    /// <summary>
    /// Command ID.
    /// </summary>
    public const int CommandId = 0x0100;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("4696d3fd-76ac-4626-8b32-2423acf9d5de");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly Package package;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeCleanerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    private CodeCleanerCommand(Package package)
    {
      if (package == null)
      {
        throw new ArgumentNullException("package");
      }

      this.package = package;

      OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
      if (commandService != null)
      {
        var menuCommandID = new CommandID(CommandSet, CommandId);
        var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
        commandService.AddCommand(menuItem);
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static CodeCleanerCommand Instance
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private IServiceProvider ServiceProvider
    {
      get
      {
        return this.package;
      }
    }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static void Initialize(Package package)
    {
      Instance = new CodeCleanerCommand(package);
    }

    private string GetActiveDocumentFilePath(IServiceProvider serviceProvider)
    {
      EnvDTE80.DTE2 applicationObject = serviceProvider.GetService(typeof(DTE)) as EnvDTE80.DTE2;
      return applicationObject.ActiveDocument.FullName;
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void MenuItemCallback(object sender, EventArgs e)
    {
      string activeDocumentPath = GetActiveDocumentFilePath(ServiceProvider);

      Microsoft.Scripting.Hosting.ScriptEngine pythonEngine = IronPython.Hosting.Python.CreateEngine();
      Microsoft.Scripting.Hosting.ScriptScope scope = pythonEngine.CreateScope();

      pythonEngine.ExecuteFile("C:\\Users\\belyakov\\Documents\\Visual Studio 2015\\Projects\\RefactorFiles\\RefactorFiles\\RefactorFiles.py", scope);

      dynamic refactor = scope.GetVariable("Removets");

      //refactor("C:\\Users\\belyakov\\Documents\\Visual Studio 2015\\Projects\\RefactorFiles\\RefactorFiles\\text1.cpp");
      refactor(activeDocumentPath);
    }
  }
}
