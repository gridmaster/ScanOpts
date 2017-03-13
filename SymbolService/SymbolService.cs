using Core;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;

namespace DailySymbolService
{
    public class SymbolService : ISymbolService
    {
        public void LoadSymbols()
        {
            var nasdaq = "http://eoddata.com/stocklist/NASDAQ/A.htm";
            string sPage = WebPage.Get(String.Format(nasdaq));
            string sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));
            string pages = sub1.Substring(0, sub1.IndexOf("</table>") + "</table>".Length);
            string sub3 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
            string symbols = sub3.Substring(0, sub3.IndexOf("</table>") + "</table>".Length);

            

            //symbols.find('tr').each(function(i, el) {
            //    var $tds = $(this).find('td'),
            //productId = $tds.eq(0).text(),
            //product = $tds.eq(1).text(),
            //Quantity = $tds.eq(2).text();
            //    // do something with productId, product, Quantity
            //});
        }
    }
}
