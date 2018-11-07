using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCleanerSpace
{
  class ActivePathHolder
  {
    public string activeFilePath;
    public string tmpFilePath
    {
      get { return activeFilePath + "_tmp"; }
    }

    private static ActivePathHolder instance;

    private ActivePathHolder()
    { }

    public static ActivePathHolder getInstance()
    {
      if (instance == null)
        instance = new ActivePathHolder();
      return instance;
    }
  }
}
