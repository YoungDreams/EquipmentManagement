using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;

namespace Foundation.Windsor
{
    public class LifeStyleConvertor : IContributeComponentModelConstruction
    {
        private readonly LifestyleType _originaLifestyleType;
        private readonly LifestyleType _newLifestyleType;

        public LifeStyleConvertor()
            : this(LifestyleType.PerWebRequest, LifestyleType.Scoped)
        {
        }

        public LifeStyleConvertor(LifestyleType originaLifestyleType, LifestyleType newLifestyleType)
        {
            _originaLifestyleType = originaLifestyleType;
            _newLifestyleType = newLifestyleType;
        }

        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model.LifestyleType == _originaLifestyleType)
            {
                model.LifestyleType = _newLifestyleType;
            }
        }
    }
}