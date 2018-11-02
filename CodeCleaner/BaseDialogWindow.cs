﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.PlatformUI;

namespace CodeCleanerSpace
{
  public class BaseDialogWindow : DialogWindow
  {
    public BaseDialogWindow()
    {
      this.HasMaximizeButton = true;
      this.HasMinimizeButton = true;
    }
  }
}
