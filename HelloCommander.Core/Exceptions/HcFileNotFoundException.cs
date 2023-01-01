using HelloCommander.Core.Exceptions.Base;
using HelloCommander.Core.Utils;

namespace HelloCommander.Core.Exceptions;

public class HcFileNotFoundException : HcExceptionBase
{
    public HcFileNotFoundException()
    {
    }

    public HcFileNotFoundException(string file)
        : base($"{LanguageResources.FileNotExists}:{Environment.NewLine}{file}")
    {
    }
}
