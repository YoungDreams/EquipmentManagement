using System.Collections.Generic;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Contract
{
    public class CreateViewModel : CreateContractCommand
    {
        public Entities.Customer Customer { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }
    }

    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public int? PreviousContractId { get; set; }
        public SignedType? SignedType { get; set; }
    }
}