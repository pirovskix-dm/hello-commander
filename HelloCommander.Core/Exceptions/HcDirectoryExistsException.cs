using HelloCommander.Core.Exceptions.Base;
using HelloCommander.Core.Utils;

namespace HelloCommander.Core.Exceptions;

public class HcDirectoryExistsException : HcExceptionBase
{
    public HcDirectoryExistsException()
    {
    }

    public HcDirectoryExistsException(string directory)
        : base($"{LanguageResources.DirectoryExists}:{Environment.NewLine}{directory}")
    {
    }
}
