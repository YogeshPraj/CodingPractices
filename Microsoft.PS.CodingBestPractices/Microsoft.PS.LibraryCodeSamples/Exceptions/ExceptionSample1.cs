using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PS.LibraryCodeSamples.Exceptions
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/kcwalina/2005/03/16/design-guidelines-update-exception-throwing/
    /// </summary>
    public class ExceptionSample1
    {
        public void DoSomething_Before(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            using (var file = System.IO.File.OpenWrite(filePath))
            {

            }
        }

        public void DoSomething_After(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            using (var file = System.IO.File.OpenWrite("some path"))
            {


            }
        }
    }
}
