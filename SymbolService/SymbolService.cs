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
            var nasdaq = "http://eoddata.com/stocklist/NASDAQ/0.htm";
            string sPage = WebPage.Get(String.Format(nasdaq));
            string sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));
            string pages = sub1.Substring(0, sub1.IndexOf("</table>") + "</table>".Length);

            List<string> symbolLookups = GetPagesLookup(pages);

            string sub3 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
            string symbols = sub3.Substring(0, sub3.IndexOf("</table>") + "</table>".Length);

            List<string> symbolss = GetSymbolLookup(symbols);


            //symbols.find('tr').each(function(i, el) {
            //    var $tds = $(this).find('td'),
            //productId = $tds.eq(0).text(),
            //product = $tds.eq(1).text(),
            //Quantity = $tds.eq(2).text();
            //    // do something with productId, product, Quantity
            //});
        }

        private List<string> GetPagesLookup(string pages)
        {
            List<string> myPages = null;
            string work = "";

            work = pages.Substring(pages.IndexOf("<tr>") + "<tr>".Length);
            work = work.Substring(0, work.IndexOf("</tr>"));

            work = work.Replace("<td ", "|");
            work = work.Substring(1);
            myPages = work.Split('|').ToList<string>();

            for(int i = 0; i < myPages.Count; i++)
            {
                myPages[i] = myPages[i].Replace("class=\"ls\">", "");
                myPages[i] = myPages[i].Replace("class=\"ld\"><a href=\"", "");
                myPages[i] = myPages[i].Replace("</td>", "").Replace("</a>", "");
                if( i>0)
                    myPages[i] = myPages[i].Substring(myPages[i].Length - 1);
            }

            return myPages;
        }

        private List<string> GetSymbolLookup(string pages)
        {
            List<string> myPages = null;
            string work = "";

            work = pages.Substring(pages.ToUpper().IndexOf("<TR "));

            work = work.Replace("<tr ", "|");
            work = work.Substring(1);
            myPages = work.Split('|').ToList<string>();

            for (int i = 0; i < myPages.Count; i++)
            {
                string symbol = myPages[i].Substring(myPages[i].IndexOf("<A href="));
                symbol = symbol.Substring(symbol.IndexOf(">")+1);
                myPages[i] = symbol;
                symbol = symbol.Substring(0, symbol.IndexOf("<"));

                string company = myPages[i].Substring(myPages[i].IndexOf("<td>") + 4);
                company = company.Substring(company.IndexOf("<") - 1);

                myPages[i] = myPages[i].Replace("class=\"ls\">", "");
                myPages[i] = myPages[i].Replace("class=\"ld\"><a href=\"", "");
                myPages[i] = myPages[i].Replace("</td>", "").Replace("</a>", "");
                if (i > 0)
                    myPages[i] = myPages[i].Substring(myPages[i].Length - 1);
            }

            return myPages;
        }
    }
}
