using System;
using System.Data;
using System.Web;
using System.Collections.Generic;


public class Category
{
    private string categoryID;
    private string categoryName;
    private string categoryType;
    private List<Product> productList = new List<Product>();
    fgenDB fgen = new fgenDB();
    String frm_cocd, frm_frm_uname;

    public string CategoryID
    {
        get { return this.categoryID; }
        set { this.categoryID = value; }
    }
    public string CategoryType
    {
        get { return this.categoryType; }
        set { this.categoryType = value; }
    }
    public string CategoryName
    {
        get { return this.categoryName; }
        set { this.categoryName = value; }
    }
    public List<Product> ProductList
    {
        get { return this.productList; }
        set { this.productList = value; }
    }
    public List<Category> GetCategories(string frm_qstr)
    {
        string mhd = "", frm_ulevel = "", tab_name = "", cond = "", frm_uname = "", frm_cocd = ""; DataTable dt, dt1; int i = -1;

        DataTable ddt = new DataTable();
        frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
        frm_frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        frm_uname = frm_frm_uname;
        frm_ulevel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");

        List<Category> categoryList = new List<Category>();
        List<Product> ProductList = new List<Product>();

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_MRSYS'", "tname");
        if (mhd == "0") fgen.execute_cmd(frm_qstr,frm_cocd, "create table FIN_MRSYS(USERID VARCHAR2(10),USERNAME VARCHAR2(30),BRANCHCD CHAR(2),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE,ID VARCHAR2(5),MLEVEL NUMBER(1),TEXT VARCHAR2(50),ALLOW_LEVEL NUMBER(2),WEB_ACTION  VARCHAR2(50),SEARCH_KEY  vARCHAR2(50),SUBMENU  CHAR(1),SUBMENUID CHAR(15),FORM VARCHAR2(10),PARAM  VARCHAR2(10),USER_COLOR VARCHAR(10) DEFAULT '414246',IDESC VARCHAR(50) DEFAULT '-')");

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select distinct * from FIN_MRSYS where trim(upper(USERNAME))='" + frm_uname + "'");

        if (dt.Rows.Count > 0 && frm_ulevel.ToString().Trim() == "0") { tab_name = "FIN_MRSYS"; cond = "and trim(upper(username))='" + frm_uname + "'"; }
        else if (dt.Rows.Count <= 0 && frm_ulevel.ToString().Trim() == "0") { tab_name = "FIN_MSYS"; cond = ""; }
        else if (dt.Rows.Count <= 0 && frm_ulevel.ToString().Trim() == "M" && frm_cocd == "LIVN") { tab_name = "FIN_MRSYS"; cond = ""; }
        else { tab_name = "FIN_MRSYS"; cond = "and trim(upper(username))='" + frm_uname + "'"; }


        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select distinct ID, trim(text) as text,form,SEARCH_KEY from " + tab_name + " where mlevel=1  " + cond + " ORDER BY ID,SEARCH_KEY");
        try
        {
            DataTableReader reader = dt.CreateDataReader();
            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryID = reader["ID"].ToString();
                category.CategoryName = reader["text"].ToString();
                category.CategoryType = reader["form"].ToString();
                string mtype = category.CategoryType;

                categoryList.Add(category);
                i++;
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, "Select trim(text) as text,trim(web_action) as web_action,id,SEARCH_KEY from " + tab_name + " where mlevel=2 AND form ='" + mtype + "' " + cond + " order by ID,SEARCH_KEY ");
                DataTableReader reader1 = dt1.CreateDataReader();
                while (reader1.Read()) categoryList[i].productList.Add(new Product(reader1["text"].ToString(), reader1["id"].ToString(), reader1["web_action"].ToString()));
                reader1.Close();
            }
        }
        catch { }
        return categoryList;
    }
    public Category()
    {
    }
}