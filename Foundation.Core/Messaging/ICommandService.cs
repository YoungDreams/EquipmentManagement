namespace Foundation.Messaging
{
    public interface ICommandService
    {
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;
        TCommandResult ExecuteFoResult<TCommandResult>(ICommand<TCommandResult> command);
        void ExecuteSingleCommand<TCommand>(TCommand command) where TCommand : ICommand;
        void ExecuteBatchCommands(params ICommand[] commands);
    }
}
