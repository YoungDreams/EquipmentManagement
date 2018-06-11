using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.Account;
using System;
using System.Web.Mvc;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.CustomerFamily
{
    public class CustomerFamilyController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;

        public CustomerFamilyController(ICommandService commandService, IFetcher fetcher)
        {
            _commandService = commandService;
            _fetcher = fetcher;
        }

        /// <summary>
        /// xinzeng
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult Create(int customerId)
        {            
            var viewModel = new CreateViewModel
            {
                CustomerId = customerId,
            };
            return PartialView("~/Views/Customer/_CustomerFamily.CreateForm.cshtml", viewModel); 
        }

        /// <summary>
        /// 提交合同数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void Create(CreateCustomerFamilyCommand command)
        {
            _commandService.Execute(command);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult Edit(int id)
        {
            var customerFamily = _fetcher.Get<Entities.CustomerFamily>(id);

            var viewModel = new EditViewModel
            {
                Id = customerFamily.Id,
                CustomerId = customerFamily.Customer.Id,
                Name = customerFamily.Name,
                Sex = customerFamily.Sex,
                Ship = customerFamily.Ship,
                IDCardNo = customerFamily.IDCardNo,
                MobilePhone = customerFamily.MobilePhone,
                Tel = customerFamily.Tel,
                Mailbox = customerFamily.Mailbox,
                WorkUnit = customerFamily.WorkUnit,
                Address = customerFamily.Address,
            };
            return PartialView("~/Views/Customer/_CustomerFamily.EditForm.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void Edit(EditCustomerFamilyCommand command)
        {
            _commandService.Execute(command);
        }

        public PartialViewResult EditAvatar(int familyId)
        {
            var viewModel = new EditCustomerFamilyAvatarViewModel
            {
                CustomerFamilyId = familyId,
            };
            return PartialView("~/Views/Customer/_CustomerFamily.Avatar.cshtml", viewModel);
        }
        [HttpPost]
        public void EditAvatar(EditCustomerFamilyAvatarCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.CustomerFamily CustomerFamily = _fetcher.Get<Entities.CustomerFamily>(command.EntityId);
            if (CustomerFamily == null)
                throw new ApplicationException("CustomerFamily cannot be found");
            _commandService.Execute(DeleteEntityCommand.Of(CustomerFamily));
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }
    }
}