using Microsoft.Extensions.Logging;

namespace FInSearchAPI.Handlers
{
    public interface ICommandHandler<ICommand>
    {
        ILogger Logger { get; set; }
        void Handle(ICommand command);
    }
}