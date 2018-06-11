namespace PensionInsurance.Web.Views.Finance.CustomerAccountTransfer
{
    public class CreateViewModel
    {
        public int TransferFrom { get; set; }
        public decimal TransferBalance { get; set; }
        public string TransferName { get; set; }
        public decimal TransferAmount { get; set; }
        public int ProjectId { get; set; }
        public string Reason { get; set; }
    }
}