using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.HtmlControls;
using Foundation.Core;
using Foundation.Data;
using Newtonsoft.Json;
using PPM.Entities;
using PPM.Query;

namespace PPM.Web.Common
{
    public static class DropDownListExtensions
    {

        //public static IHtmlString DropDownList(this HtmlHelper html, string name, SettingType settingType)
        //{
        //    //        <select name="@nameof(Model.Client.Level)" class="form-control input-small">
        //    //               @foreach(var item in Model.ClientLevelList)
        //    //               {
        //    //                     < option > @item.Name </ option >
        //    //               }
        //    //        </select>
        //    var settings = ServiceLocator.Current.Resolve<ISettingQueryService>().GetSettingsByType(settingType);


        //    var selectList = settings.Select(x => new SelectListItem
        //    {
        //        Text = x.Name
        //    }).ToList();

        //    selectList[0].Selected = true;

        //    return html.DropDownList(name, selectList, new { @class = "form-control input-small" });
        //}

        //public static IHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, SettingType settingType, bool emptyOption = false)
        //{
        //    //        <select name="@nameof(Model.Client.Level)" class="form-control input-small">
        //    //               @foreach(var item in Model.ClientLevelList)
        //    //               {
        //    //                     < option > @item.Name </ option >
        //    //               }
        //    //        </select>
        //    var settings = ServiceLocator.Current.Resolve<ISettingQueryService>().GetSettingsByType(settingType);

        //    var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

        //    var selectList = settings.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Selected = (value == x.Name)
        //    }).ToList();

        //    if (emptyOption)
        //    {
        //        selectList.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
        //    }

        //    if (!selectList.Any(x => x.Selected) && selectList.Any())
        //        selectList[0].Selected = true;

        //    return htmlHelper.DropDownListFor(expression, selectList, new { @class = "form-control input-small" });
        //}

        //public static IHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, SettingType settingType, object htmlAttributes,bool emptyOption = false)
        //{
        //    //        <select name="@nameof(Model.Client.Level)" class="form-control input-small">
        //    //               @foreach(var item in Model.ClientLevelList)
        //    //               {
        //    //                     < option > @item.Name </ option >
        //    //               }
        //    //        </select>
        //    var settings = ServiceLocator.Current.Resolve<ISettingQueryService>().GetSettingsByType(settingType);

        //    var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

        //    var selectList = settings.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Selected = (value == x.Name)
        //    }).ToList();

        //    if (emptyOption)
        //    {
        //        selectList.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
        //    }

        //    if (!selectList.Any(x => x.Selected) && selectList.Any())
        //        selectList[0].Selected = true;

        //    return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        //}

        public static IHtmlString DropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool?>> expression, bool emptyOption = false)
        {
            //        <select name="@nameof(Model.Client.Level)" class="form-control input-small">
            //               @foreach(var item in Model.ClientLevelList)
            //               {
            //                     < option > @item.Name </ option >
            //               }
            //        </select>


            var value = (bool?)ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

            var selectList = new List<SelectListItem>
            {
                new SelectListItem {Text = "是",Value = true.ToString(), Selected = value == true},
                new SelectListItem {Text = "否",Value = false.ToString(),Selected = value == true},
            };

            if (emptyOption)
            {
                selectList.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
            }

            if (!selectList.Any(x => x.Selected) && selectList.Any())
                selectList[0].Selected = true;

            return htmlHelper.DropDownListFor(expression, selectList, new { @class = "form-control input-small" });
        }

        public static IHtmlString InputDropDownEnum<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, bool emptyOption = false)
        {
            //        <select name="@nameof(Model.Client.Level)" class="form-control input-small">
            //               @foreach(var item in Model.ClientLevelList)
            //               {
            //                     < option > @item.Name </ option >
            //               }
            //        </select>
            var names = Enum.GetValues(enumType).OfType<object>();
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

            var selectList = names.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Selected = (value != null && value.ToString() == x)
            }).ToList();

            if (emptyOption)
            {
                selectList.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
            }

            if (!selectList.Any(x => x.Selected) && selectList.Any())
                selectList[0].Selected = true;

            return htmlHelper.DropDownListFor(expression, selectList, new { @class = "form-control input-small" });
        }

        public static IHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectListItems, bool emptyOption = false)
        {
            var selectList = new List<SelectListItem>(selectListItems);
            if (emptyOption)
            {
                selectList.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
            }

            if (!selectList.Any(x => x.Selected) && selectList.Any())
                selectList[0].Selected = true;

            return htmlHelper.DropDownListFor(expression, selectList, new { @class = "form-control input-small" });
        }

        public static IHtmlString InputText<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.TextBoxFor(expression, new { @class = "form-control" });
        }

        public static IHtmlString InputText<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string format)
        {
            return htmlHelper.TextBoxFor(expression, format, new { @class = "form-control" });
        }

        public static IHtmlString InputText<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression,object htmlAttributes)
        {
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlString InputDate<TModel>(this HtmlHelper<TModel> htmlHelper, string name, DateTime? value = null, object opts = null)
        {
            return htmlHelper.TextBox(name, value.DateString(), "{0:yyyy-MM-dd}", new
            {
                @class = "form-control form-control-inline date-picker",
                data_date_format = "yyyy-mm-dd",
                data_opts = JsonConvert.SerializeObject(opts)
            });
        }

        public static IHtmlString InputDate<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.TextBoxFor(expression, "{0:yyyy-MM-dd}", new
            {
                @class = "form-control form-control-inline date-picker",
                data_date_format = "yyyy-mm-dd"
            });
        }

        public static IHtmlString InputMonth<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.TextBoxFor(expression, "{0:yyyy-MM}", new
            {
                @class = "form-control form-control-inline month-picker",
                data_date_format = "yyyy-mm-dd"
            });
        }

        public static IHtmlString InputMonth<TModel>(this HtmlHelper<TModel> htmlHelper,
string name, DateTime? value = null)
        {
            return htmlHelper.TextBox(name, value, new
            {
                @class = "form-control form-control-inline month-picker",
                data_date_format = "yyyy-mm-dd"
            });
        }

        public static IHtmlString InputDropDown<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, bool emptyOption = false)
        {
            var items = new List<SelectListItem>(selectList);
            if (emptyOption)
            {
                items.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
            }
            return htmlHelper.DropDownListFor(expression, items, new { @class = "form-control" });
        }

        

        public static IHtmlString Select2DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            bool isMultiple, bool emptyOption = false)
        {
            //        < select id = "multi-append" class="form-control select2" multiple>
            //        </ select >
            var items = new List<SelectListItem>(selectList);
            if (emptyOption)
            {
                items.Insert(0, new SelectListItem { Text = "--未选择--", Value = "" });
            }

            if (isMultiple)
            {
                return htmlHelper.DropDownListFor(expression, items,
                    new { @class = "form-control input-small select2", multiple = "" });
            }
            return htmlHelper.DropDownListFor(expression, items,
                    new { @class = "form-control input-small select2"});
        }

        public static IHtmlString BooleanRadioButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

            var template = @"<div class='radio-list'>
                                    <label class='radio-inline'>
                                        <input type='radio' name='{0}' value='True' {1}> 是
                                      </label>
                                    <label class='radio-inline'>
                                        <input type='radio' name='{0}' value='False' {2}> 否
                                      </label>
                                </div>";
            var isTure = (value == null || (bool)value);



            var html = string.Format(template, ExpressionHelper.GetExpressionText(expression), GetCheckedValue(isTure), GetCheckedValue(!isTure));

            return new MvcHtmlString(html);
        }

        public static IHtmlString BooleanRadioButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool?>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;

            var template = @"<div class='radio-list'>
                                    <label class='radio-inline'>
                                        <input type='radio' name='{0}' value='True' {1}> 是
                                      </label>
                                    <label class='radio-inline'>
                                        <input type='radio' name='{0}' value='False' {2}> 否
                                      </label>
                                </div>";


            if (value == null)
            {
                return new MvcHtmlString(string.Format(template, ExpressionHelper.GetExpressionText(expression), GetCheckedValue(false), GetCheckedValue(false)));
            }

            var isTure = (bool)value;

            var html = string.Format(template, ExpressionHelper.GetExpressionText(expression), GetCheckedValue(isTure), GetCheckedValue(!isTure));

            return new MvcHtmlString(html);
        }

        private static string GetCheckedValue(bool isTure)
        {
            return isTure ? "checked=''" : "";
        }

        public static IHtmlString RenderConsultingStatus(this HtmlHelper html, string value)
        {
            var tag = new TagBuilder("span");
            tag.AddCssClass("label label-sm");

            if (value == "有效")
            {
                tag.AddCssClass("label-success");
            }
            else
            {
                tag.AddCssClass("label-danger");
            }
            tag.SetInnerText(value);
            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }


        public static IHtmlString RenderIsEnabled(this HtmlHelper html, bool value)
        {
            var tag = new TagBuilder("span");
            tag.AddCssClass("label label-sm");

            if (value)
            {
                tag.SetInnerText("有效");
                tag.AddCssClass("label-success");
            }
            else
            {
                tag.AddCssClass("label-danger");
                tag.SetInnerText("无效");
            }

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }

    }


}