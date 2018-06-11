using System;

namespace Foundation.Messaging
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; private set; }

        protected Command()
        {
            CommandId = Guid.NewGuid();
        }
    }

    public abstract class Command<TCommandResult> : ICommand
    {
        public TCommandResult Result { get; set; }
    }
}
