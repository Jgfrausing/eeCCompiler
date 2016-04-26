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
        public string GenerateListTypeHeader(string name, string type, bool isRefrenceType)
        {
            var sr = isRefrenceType ? new StreamReader(_location + "DefaultListHeaderWithPointers.c") : new StreamReader(_location + "DefaultListHeader.c");
            return sr.ReadToEnd().Replace("{type}", type).Replace("{name}", name);
        }
        public string GenerateListTypeCode(string name, string type, bool isRefrenceType)
        {
            var sr = isRefrenceType ? new StreamReader(_location + "DefaultListCodeWithPointers.c") : new StreamReader(_location + "DefaultListCode.c");
            return sr.ReadToEnd().Replace("{type}", type).Replace("{name}", name);
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
