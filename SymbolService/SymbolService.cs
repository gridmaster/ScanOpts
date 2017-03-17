﻿using Core;
using Core.Interface;
using Core.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using DIContainer;
using ORMService;

namespace DailySymbolService
{
    public class SymbolService : ISymbolService
    {
        public void LoadSymbols()
        {
            LoadAllSymbolsFromAllExchanges();
        }

        public void LoadAllSymbolsFromAllExchanges()
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("LoadAllSymbolsFromAllExchanges - GetSymbols");
            List<string> exchanges = IOCContainer.Instance.Get<ExchangeORMService>().GetExchanges();
            LoadAllSymbolsFromAllExchanges(exchanges);
        }

        public void LoadAllSymbolsFromAllExchanges(List<string> exchanges)
        {
            IOCContainer.Instance.Get<ILogger>().InfoFormat("Start - LoadAllSymbolsFromAllExchanges");
            
            List<Symbols> allQuotes = new List<Symbols>();
            string exchangeSave = "";
            string itemSave = "";
            try
            {
                foreach (string exchange in exchanges)
                {
                    exchangeSave = exchange;
                    string sPage = WebPage.Get(String.Format(exchange, "0"));
                    string sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));
                    string pages = sub1.Substring(0, sub1.IndexOf("</table>") + "</table>".Length);

                    List<string> symbolLookups = GetPagesLookup(pages);

                    string sub3 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                    string symbols = sub3.Substring(0, sub3.IndexOf("</table>") + "</table>".Length);

                    //nasdaq = "http://eoddata.com/stocklist/NASDAQ/{0}.htm";
                    List<Symbols> symbolList = new List<Symbols>();

                    foreach (string item in symbolLookups)
                    {
                        itemSave = item;
                        sPage = WebPage.Get(string.Format(exchange, item));
                        sub1 = sPage.Substring(sPage.IndexOf("<table class=\"lett\""));

                        string sub2 = sub1.Substring(sub1.IndexOf("<table class=\"quotes\">"));
                        string symbolz = sub2.Substring(0, sub2.IndexOf("</table>") + "</table>".Length);

                        symbolList.AddRange(GetSymbolLookup(symbolz));
                    }                    

                    allQuotes.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                IOCContainer.Instance.Get<ILogger>().Fatal("LoadAllSymbolsFromAllExchanges: {0}", ex);
            }
            finally
            {
                IOCContainer.Instance.Get<ILogger>().Info("End - LoadAllSymbolsFromAllExchanges");
                IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
            }
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

        private List<Symbols> GetSymbolLookup(string pages)
        {
            List<string> myPages = null;
            string work = "";

            work = pages.Substring(pages.ToUpper().IndexOf("<TR "));

            work = work.Replace("<tr ", "|");
            work = work.Substring(1);
            myPages = work.Split('|').ToList<string>();
            List<Symbols> tempquote = new List<Symbols>();
            
            for (int i = 0; i < myPages.Count; i++)
            {
                Symbols dq = new Symbols();
                string symbol = myPages[i].Substring(myPages[i].IndexOf("<A href="));
                symbol = symbol.Substring(symbol.IndexOf(">")+1);
                myPages[i] = symbol;
                dq.Symbol = symbol.Substring(0, symbol.IndexOf("<"));

                string company = myPages[i].Substring(myPages[i].IndexOf("<td>") + 4);
                myPages[i] = company;
                if (company.IndexOf("<") > 0)
                    dq.CompanyName = company.Substring(0, company.IndexOf("<") - 1);
                else
                    dq.CompanyName = "";
                myPages[i] = myPages[i].Substring(myPages[i].IndexOf("</td>") + 5);
                myPages[i] = myPages[i].Replace("<td align=right>", "|");
                myPages[i] = myPages[i].Substring(1);
                var prices = myPages[i].Split('|');
                dq.Date = DateTime.Now;
                //name high low close volume
                //dq.high = Core.Business.ConvertStringToNumeric.ConvertDecimalToNumber(prices[0].Substring(0, prices[0].IndexOf("<")));
                //dq.low = Core.Business.ConvertStringToNumeric.ConvertDecimalToNumber(prices[1].Substring(0, prices[1].IndexOf("<")));
                //dq.close = Core.Business.ConvertStringToNumeric.ConvertDecimalToNumber(prices[2].Substring(0, prices[2].IndexOf("<")));
                //dq.volume = Core.Business.ConvertStringToNumeric.ConvertIntegerToNumber(prices[3].Substring(0, prices[3].IndexOf("<")).Replace(",", ""));
                tempquote.Add(dq);
            }

            return tempquote;
        }
    }
}
