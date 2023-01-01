using HelloCommander.Core.Exceptions.Base;
using HelloCommander.Core.Utils;

namespace HelloCommander.Core.Exceptions;

public class HcDirectoryNotFoundException : HcExceptionBase
{
    public HcDirectoryNotFoundException()
    {
    }

    public HcDirectoryNotFoundException(string directory)
        : base($"{LanguageResources.DirectoryNotExists}:{Environment.NewLine}{directory}")
    {
    }
}
