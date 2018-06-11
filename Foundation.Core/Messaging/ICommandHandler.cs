namespace Foundation.Messaging
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TCommandResult>
    {
        TCommandResult Handle(TCommand command);
    }
}