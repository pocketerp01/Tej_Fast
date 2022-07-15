using System;
using System.Text;
using System.Data;
using System.Globalization;


    public static class myExt
    {
        public static string isNotInfinit(this string val)
        {
            if (val == "Infinity" || val == "∞") val = "0";
            return val;
        }
        /// <summary>
        /// will return a value with double 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double toDouble(this string val)
        {
            double result = 0;
            try
            {
                result = Convert.ToDouble(val.isNotInfinit());
                if (val.isNotInfinit().ToString().ToUpper() == "NAN")result = 0;
            }
            catch { result = 0; }
            return result;
        }
        public static double toDouble(this string val, int roundOff)
        {
            double result = 0;
            try
            {
                result = Convert.ToDouble(val.isNotInfinit());
                if (val.isNotInfinit().ToString().ToUpper() == "NAN") result = 0;
            }
            catch { result = 0; }
            result = Math.Round(result, roundOff);
            return result;
        }
        public static double toDouble(this double val, int roundOff)
        {
            double result = 0;
            try
            {
                result = Convert.ToDouble(val.ToString().isNotInfinit());
                if (val.ToString().ToUpper() == "NAN") result = 0;
            }
            catch { result = 0; }
            result = Math.Round(result, roundOff);
            return result;
        }
        public static Int32 toInt(this string val)
        {
            Int32 result = 0;
            try
            {
                result = Convert.ToInt32(val);
            }
            catch { result = 0; }
            return result;
        }
        public static string toDate(this string val, string format)
        {
            string result = "";
            try
            {
                result = Convert.ToDateTime(val).ToString(format);
            }
            catch { result = ""; }
            return result;
        }
        public static string toProper(this string val)
        {
            if (val == null) return val;
            string[] words = val.Trim().Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;
                char firstChar = char.ToUpper(words[i][0]);
                words[i] = firstChar + (words[i].Length > 1 ? words[i].Substring(1).ToLower() : "");
            }
            return string.Join(" ", words);
        }
        public static string find(this DataTable dtSeek, string condition, string field)
        {
            fgenDB fgen = new fgenDB();
            string result = fgen.seek_iname_dt(dtSeek, condition, field);
            return result;
        }
        public static string find(this DataTable dtSeek, string condition, string field, string orderSort)
        {
            fgenDB fgen = new fgenDB();
            string result = fgen.seek_iname_dt(dtSeek, condition, field, orderSort);
            return result;
        }
        public static string fill(this DataTable dtFill, string frmQstr, string frmCoCd, string tabname, string field, string condition, string orderSort)
        {
            fgenDB fgen = new fgenDB();
            StringBuilder squery = new StringBuilder();

            squery.Append("SELECT " + (field.Length > 0 ? field : "*") + " FROM " + tabname);
            squery.Append(" " + (condition.Length > 0 ? " WHERE " + condition + "" : ""));
            squery.Append(" " + (orderSort.Length > 0 ? " ORDER BY " + orderSort + "" : ""));
            dtFill = fgen.getdata(frmQstr, frmCoCd, squery.ToString());

            return squery.ToString();
        }
        public static DataTable fill(this DataTable dtFill, string frmQstr, string frmCoCd, string tabname, string field, string condition, string orderSort, string groupBy)
        {
            fgenDB fgen = new fgenDB();
            StringBuilder squery = new StringBuilder();

            squery.Append("SELECT " + (field.Length > 0 ? field : "*") + " FROM " + tabname);
            squery.Append(" " + (condition.Length > 0 ? " WHERE " + condition + "" : ""));
            squery.Append(" " + (groupBy.Length > 0 ? " GROUP BY  " + groupBy + "" : ""));
            squery.Append(" " + (orderSort.Length > 0 ? " ORDER BY " + orderSort + "" : ""));
            dtFill = fgen.getdata(frmQstr, frmCoCd, squery.ToString());

            return dtFill;
        }
        public static string Left(this string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }
        public static string Right(this string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }
        public static string Mid(this string input, int start)
        {
            return input.Substring(Math.Min(start, input.Length));
        }
    }

