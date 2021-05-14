namespace FInSearchAPI.Handlers
{
    public interface ICommandHandler<ICommand>
    {
        void Handle(ICommand command);
    }
}