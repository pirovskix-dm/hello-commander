using HelloCommander.Core.Exceptions.Base;
using HelloCommander.Core.Utils;

namespace HelloCommander.Core.Exceptions;

public class HcFileExistsException : HcExceptionBase
{
    public HcFileExistsException()
    {
    }

    public HcFileExistsException(string file)
        : base($"{LanguageResources.FileExists}:{Environment.NewLine}{file}")
    {
    }
}
