namespace Core.Business
{
    public static class ConvertStringToNumeric
    {
        public static decimal ConvertDecimalToNumber(string value)
        {
            decimal tempdec;
            string tempval = value;
            decimal.TryParse(tempval, out tempdec);
            return tempdec;
        }

        public static int ConvertIntegerToNumber(string value)
        {
            int tempdec;
            string tempval = value;
            int.TryParse(tempval, out tempdec);
            return tempdec;
        }
    }
}
