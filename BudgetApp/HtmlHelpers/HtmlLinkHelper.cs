using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BudgetApp.WebUI.Models;
using System.Text;

namespace BudgetApp.WebUI.HtmlHelpers
{
    public static class HtmlLinkHelper
    {
        public static MvcHtmlString LinkHelper(this HtmlHelper helper, LedgerListModel listModel, Func<int, string> links)
        {
            StringBuilder builder = new StringBuilder();
            
            for (int i = 1; i <= listModel.NumberOfLinks; i++)
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", links(i));
                tagBuilder.InnerHtml = "" + i;
                
                builder.Append(tagBuilder.ToString());
                builder.Append("  ");
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}