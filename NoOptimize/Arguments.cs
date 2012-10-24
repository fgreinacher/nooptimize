using CmdLine;

namespace NoOptimize
{
    [CommandLineArguments(Program = "NoOptimize", Title = "NoOptimize", Description = "A command line utility to disable JIT optimization for a given set of .NET assemblies")]
    class Arguments
    {
        [CommandLineParameter(Name = "directory", ParameterIndex = 1, Required = true, Description = "Specifies in which directory .NET assemblies should be searched.")]
        public string Directory
        {
            get;
            set;
        }
        
        [CommandLineParameter(Command = "recursive", Default = false, Description = "Specifies whether the directory search should recurse to sub directories.")]
        public bool Recursive
        {
            get;
            set;
        }

        [CommandLineParameter(Command = "filter", Default = "*", Description = "Specifies a filter that controls which files should be considered.")]
        public string Filter
        {
            get;
            set;
        }

        [CommandLineParameter(Command = "preview", Default = false, Description = "Specifies whether to show only a preview of the operations that will be performed.")]
        public bool Preview
        {
            get;
            set;
        }

        [CommandLineParameter(Command = "help", Default = false, Description = "Shows this help text.", Name = "Help", IsHelp = true)]
        public bool Help
        {
            get;
            set;
        }
    }
}