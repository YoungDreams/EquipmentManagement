using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PPM.CommandHandlers.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseValidator()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {

        }
    }
}
