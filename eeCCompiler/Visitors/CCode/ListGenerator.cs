using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eeCCompiler.Visitors.CCode
{
    class DefaultCCode
    {
        private string _location = @"..\..\C_code\";
        public string GenerateListTypeHeader(string typeName)
        {
            var sr = new StreamReader(_location + "DefaultListHeader.c");
            return sr.ReadToEnd().Replace("{type}", typeName);
        }
        public string GenerateListTypeCode(string typeName)
        {
            var sr = new StreamReader(_location + "DefaultListCode.c");
            return sr.ReadToEnd().Replace("{type}", typeName);
        }

        public string GetIncludes()
        {
            var sr = new StreamReader(_location + "Includes.c");
            return sr.ReadToEnd();
        }

        public string StandardFunctions()
        {
            var sr = new StreamReader(_location + "StandardFunctions.c");
            return sr.ReadToEnd();
        }

        public string IncludeMain(string main)
        {
            var sr = new StreamReader(main);
            return sr.ReadToEnd();
        }
    }
}
