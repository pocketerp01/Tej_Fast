
using System;
using System.Collections.Generic;

using System.Linq;


namespace Models
{
    public class SingleModel
    {
        public string Guid { get; set; }
        public string VarName { get; set; }
        public string VarValue { get; set; }
        public DateTime date1 { get; set; }



    }
    public class Singleton
    {
        //read-only dictionary to track multitons
        private static IDictionary<string, Singleton> _Tracker = new Dictionary<string, Singleton> { };
        public string MyGuid = "";
        public List<SingleModel> list;
        private Singleton(string key)
        {
            MyGuid = key;
            list = new List<SingleModel>();
        }

        public static Singleton GetInstance(string key)
        {
            Singleton item = null;
            lock (_Tracker)
            {
                if (!_Tracker.TryGetValue(key, out item))
                {
                    item = new Singleton(key);
                    _Tracker.Add(key, item);
                }
            }
            return item;
        }
        public static void Fn_Set_Mvar(string UNIQ_ID, string P_VAR_NAME, string P_VAR_VALUE)
        {
            if (UNIQ_ID != null)
            {
                Singleton singleton = GetInstance(UNIQ_ID);
                singleton.list.RemoveAll(s => s.Guid == UNIQ_ID && s.VarName == P_VAR_NAME.Trim());
                SingleModel tmodel = new SingleModel();
                tmodel.Guid = UNIQ_ID.ToUpper();
                tmodel.VarName = P_VAR_NAME.Trim();
                tmodel.VarValue = P_VAR_VALUE == null ? "" : P_VAR_VALUE.Trim();
                tmodel.date1 = DateTime.Now;
                singleton.list.Add(tmodel);

            }
        }

        public static string Fn_Get_Mvar(string UNIQ_ID, string P_VAR_NAME)
        {
            string result = "";
            if (UNIQ_ID != null)
            {

                Singleton singleton = GetInstance(UNIQ_ID);
                try
                {
                    result = singleton.list.Where(s => s.Guid.ToUpper().Trim() == UNIQ_ID.ToUpper().Trim() && s.VarName.Trim().ToUpper() == P_VAR_NAME.Trim().ToUpper()).ToList()[0].VarValue.Trim();
                }
                catch(Exception err)
                {
                    result = "0";
                }
                if (P_VAR_NAME == "U_SYS_COM_QRY" && result.Trim() == "0")
                {
                    result = "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND,NVL(OBJ_CAPTION_REG,'-') AS OBJ_CAPTION_REG FROM SYS_CONFIG ";
                }
            }
            return result;
        }
    }
}