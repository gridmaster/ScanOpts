using System.Collections.Generic;

namespace Core.JsonKeyStatistics
{
    public class BaseObject
    {
        public class EnterpriseValue
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class ForwardPE
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ProfitMargins
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class FloatShares
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class SharesOutstanding
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class SharesShort
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class SharesShortPriorMonth
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class HeldPercentInsiders
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class HeldPercentInstitutions
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ShortRatio
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ShortPercentOfFloat
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class Beta
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class MorningStarOverallRating
        {
        }

        public class MorningStarRiskRating
        {
        }

        public class BookValue
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class PriceToBook
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class AnnualReportExpenseRatio
        {
        }

        public class YtdReturn
        {
        }

        public class Beta3Year
        {
        }

        public class TotalAssets
        {
        }

        public class Yield
        {
        }

        public class FundInceptionDate
        {
        }

        public class ThreeYearAverageReturn
        {
        }

        public class FiveYearAverageReturn
        {
        }

        public class PriceToSalesTrailing12Months
        {
        }

        public class LastFiscalYearEnd
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class NextFiscalYearEnd
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class MostRecentQuarter
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class EarningsQuarterlyGrowth
        {
        }

        public class RevenueQuarterlyGrowth
        {
        }

        public class NetIncomeToCommon
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class TrailingEps
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ForwardEps
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class PegRatio
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class LastSplitDate
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class EnterpriseToRevenue
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class EnterpriseToEbitda
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class __invalid_type__52WeekChange
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class SandP52WeekChange
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class LastDividendValue
        {
        }

        public class LastCapGain
        {
        }

        public class AnnualHoldingsTurnover
        {
        }

        public class DefaultKeyStatistics
        {
            public int maxAge { get; set; }
            public EnterpriseValue enterpriseValue { get; set; }
            public ForwardPE forwardPE { get; set; }
            public ProfitMargins profitMargins { get; set; }
            public FloatShares floatShares { get; set; }
            public SharesOutstanding sharesOutstanding { get; set; }
            public SharesShort sharesShort { get; set; }
            public SharesShortPriorMonth sharesShortPriorMonth { get; set; }
            public HeldPercentInsiders heldPercentInsiders { get; set; }
            public HeldPercentInstitutions heldPercentInstitutions { get; set; }
            public ShortRatio shortRatio { get; set; }
            public ShortPercentOfFloat shortPercentOfFloat { get; set; }
            public Beta beta { get; set; }
            public MorningStarOverallRating morningStarOverallRating { get; set; }
            public MorningStarRiskRating morningStarRiskRating { get; set; }
            public object category { get; set; }
            public BookValue bookValue { get; set; }
            public PriceToBook priceToBook { get; set; }
            public AnnualReportExpenseRatio annualReportExpenseRatio { get; set; }
            public YtdReturn ytdReturn { get; set; }
            public Beta3Year beta3Year { get; set; }
            public TotalAssets totalAssets { get; set; }
            public Yield yield { get; set; }
            public object fundFamily { get; set; }
            public FundInceptionDate fundInceptionDate { get; set; }
            public object legalType { get; set; }
            public ThreeYearAverageReturn threeYearAverageReturn { get; set; }
            public FiveYearAverageReturn fiveYearAverageReturn { get; set; }
            public PriceToSalesTrailing12Months priceToSalesTrailing12Months { get; set; }
            public LastFiscalYearEnd lastFiscalYearEnd { get; set; }
            public NextFiscalYearEnd nextFiscalYearEnd { get; set; }
            public MostRecentQuarter mostRecentQuarter { get; set; }
            public EarningsQuarterlyGrowth earningsQuarterlyGrowth { get; set; }
            public RevenueQuarterlyGrowth revenueQuarterlyGrowth { get; set; }
            public NetIncomeToCommon netIncomeToCommon { get; set; }
            public TrailingEps trailingEps { get; set; }
            public ForwardEps forwardEps { get; set; }
            public PegRatio pegRatio { get; set; }
            public string lastSplitFactor { get; set; }
            public LastSplitDate lastSplitDate { get; set; }
            public EnterpriseToRevenue enterpriseToRevenue { get; set; }
            public EnterpriseToEbitda enterpriseToEbitda { get; set; }
            public __invalid_type__52WeekChange __invalid_name__52WeekChange { get; set; }
            public SandP52WeekChange SandP52WeekChange { get; set; }
            public LastDividendValue lastDividendValue { get; set; }
            public LastCapGain lastCapGain { get; set; }
            public AnnualHoldingsTurnover annualHoldingsTurnover { get; set; }
        }

        public class EarningsDate
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class EarningsAverage
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class EarningsLow
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class EarningsHigh
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class RevenueAverage
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class RevenueLow
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class RevenueHigh
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class Earnings
        {
            public List<EarningsDate> earningsDate { get; set; }
            public EarningsAverage earningsAverage { get; set; }
            public EarningsLow earningsLow { get; set; }
            public EarningsHigh earningsHigh { get; set; }
            public RevenueAverage revenueAverage { get; set; }
            public RevenueLow revenueLow { get; set; }
            public RevenueHigh revenueHigh { get; set; }
        }

        public class ExDividendDate
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class DividendDate
        {
            public int raw { get; set; }
            public string fmt { get; set; }
        }

        public class CalendarEvents
        {
            public int maxAge { get; set; }
            public Earnings earnings { get; set; }
            public ExDividendDate exDividendDate { get; set; }
            public DividendDate dividendDate { get; set; }
        }

        public class CurrentPrice
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class TargetHighPrice
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class TargetLowPrice
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class TargetMeanPrice
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class TargetMedianPrice
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class RecommendationMean
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class NumberOfAnalystOpinions
        {
            public int raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class TotalCash
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class TotalCashPerShare
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class Ebitda
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class TotalDebt
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class QuickRatio
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class CurrentRatio
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class TotalRevenue
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class DebtToEquity
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class RevenuePerShare
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ReturnOnAssets
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ReturnOnEquity
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class GrossProfits
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class FreeCashflow
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class OperatingCashflow
        {
            public long raw { get; set; }
            public string fmt { get; set; }
            public string longFmt { get; set; }
        }

        public class EarningsGrowth
        {
        }

        public class RevenueGrowth
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class GrossMargins
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class EbitdaMargins
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class OperatingMargins
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class ProfitMargins2
        {
            public double raw { get; set; }
            public string fmt { get; set; }
        }

        public class FinancialData
        {
            public int maxAge { get; set; }
            public CurrentPrice currentPrice { get; set; }
            public TargetHighPrice targetHighPrice { get; set; }
            public TargetLowPrice targetLowPrice { get; set; }
            public TargetMeanPrice targetMeanPrice { get; set; }
            public TargetMedianPrice targetMedianPrice { get; set; }
            public RecommendationMean recommendationMean { get; set; }
            public string recommendationKey { get; set; }
            public NumberOfAnalystOpinions numberOfAnalystOpinions { get; set; }
            public TotalCash totalCash { get; set; }
            public TotalCashPerShare totalCashPerShare { get; set; }
            public Ebitda ebitda { get; set; }
            public TotalDebt totalDebt { get; set; }
            public QuickRatio quickRatio { get; set; }
            public CurrentRatio currentRatio { get; set; }
            public TotalRevenue totalRevenue { get; set; }
            public DebtToEquity debtToEquity { get; set; }
            public RevenuePerShare revenuePerShare { get; set; }
            public ReturnOnAssets returnOnAssets { get; set; }
            public ReturnOnEquity returnOnEquity { get; set; }
            public GrossProfits grossProfits { get; set; }
            public FreeCashflow freeCashflow { get; set; }
            public OperatingCashflow operatingCashflow { get; set; }
            public EarningsGrowth earningsGrowth { get; set; }
            public RevenueGrowth revenueGrowth { get; set; }
            public GrossMargins grossMargins { get; set; }
            public EbitdaMargins ebitdaMargins { get; set; }
            public OperatingMargins operatingMargins { get; set; }
            public ProfitMargins2 profitMargins { get; set; }
        }

        public class Result
        {
            public DefaultKeyStatistics defaultKeyStatistics { get; set; }
            public CalendarEvents calendarEvents { get; set; }
            public FinancialData financialData { get; set; }
        }

        public class QuoteSummary
        {
            public List<Result> result { get; set; }
            public object error { get; set; }
        }

        public class RootObject
        {
            public QuoteSummary quoteSummary { get; set; }
        }
    }
}
