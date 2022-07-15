using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;



public partial class om_ac_club : System.Web.UI.Page
{
    string btnmode, group_code, newcode, oldcode, query1, prefix, mhd;
    string ulvl, col1, cdt1, cdt2, Seeksql, popvar, scode, sname;
    int nflag;
    fgenDB fgen = new fgenDB();

    string btnval, SQuery, col2, vardate, fromdt, todt;

    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //btnnew.Focus();
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    if (frm_qstr.Contains("@"))
                    {
                        frm_formID = frm_qstr.Split('@')[1].ToString();
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                hfmode.Value = "";
                hfname.Value = "";

            }
            if (frm_uname == "FINTEAM" || frm_uname == "ABHAY" || frm_uname == "PUNEETG")
            {
                A3.Visible = true;
            }
            else A3.Visible = false;

            if (frm_cocd == "SGRP")
            {
                A1.Visible = false;
                if (frm_uname == "FINTEAM" || frm_uname == "ABHAY" || frm_uname == "PUNEETG")
                {
                }
                else
                {
                    gridDiv.Visible = false;
                    A1.Visible = false;
                    A2.Visible = false;
                    A3.Visible = false;
                }
            }
        }
    }
    protected void btnOKTarget_Click(object sender, EventArgs e)
    {

        fgen.ResetForm(this.Controls);

    }
    private void disp_list()
    {
        Seeksql = "";
        btnmode = hfname.Value;
        switch (btnmode)
        {
            case "ACD":
                Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp from famst where length(Trim(nvl(deac_by,'-')))<=1 order by aname";
                break;
            case "MG":
                Seeksql = "select type1 as fstr, Name ,type1 from type where id='Z' order by type1";
                break;
            case "SC":
                Seeksql = "Select type1 as fstr, Name,Type1 from typegrp where id='A' and substr(type1,1,2)='" + txtacgp.Text.Trim() + "' order by type1";
                break;
            case "ACD1":
                if (txtacode2.Text.Length > 1)
                {
                    Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp,deac_by,lbt_no as br_alias from famst  where trim(Acode)!='" + txtacode2.Text + "'  order by aname";
                }
                else
                {
                    Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp,deac_by,lbt_no as br_alias  from famst  where length(Trim(nvl(deac_by,'-')))<=1 order by aname";
                }

                break;
            case "ACD2":
                if (txtacode1.Text.Length > 1)
                {
                    Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp,deac_by,lbt_no as br_alias  from famst  where trim(Acode)!='" + txtacode1.Text + "' and length(Trim(nvl(deac_by,'-')))<=1 order by aname";
                }
                else
                {
                    Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp,deac_by,lbt_no as br_alias  from famst  where length(Trim(nvl(deac_by,'-')))<=1 order by aname";
                }
                break;
            case "ACD3":
                Seeksql = "select acode as fstr, ANAME,acode,addr1||','||addr2||','||addr2 Address,grp from famst  where length(Trim(nvl(deac_by,'-')))<=1 order by aname";
                break;

        }

        if (Seeksql == "") { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", Seeksql);
        }
    }
    public void showmsg(string oldcode, string newcode)
    {

        oldcode = ""; newcode = "";
        string[] mylist = hfmode.Value.Split('@');

        oldcode = mylist[0].ToString().Trim();
        newcode = mylist[1].ToString().Trim();

        btnmode = hfname.Value;

        update_Trans_Files();

        if (hf2.Value == "0")
        {
            if (txtscode.Text == "")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set BSSCH='" + newcode.Substring(0, 2).Trim() + "'||'00',GRP='" + newcode.Substring(0, 2).Trim() + "',acode='" + newcode + "' where TRIM(acode)='" + oldcode + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set BSSCH='" + newcode.Substring(0, 2).Trim() + "'||'00',GRP='" + newcode.Substring(0, 2).Trim() + "' where TRIM(acode)='" + newcode + "'");
            }
            else
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set BSSCH='" + txtscode.Text.Trim() + "',GRP='" + newcode.Substring(0, 2).Trim() + "',acode='" + newcode + "' where TRIM(acode)='" + oldcode + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set BSSCH='" + txtscode.Text.Trim() + "',GRP='" + newcode.Substring(0, 2).Trim() + "' where TRIM(acode)='" + newcode + "'");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update famstBAL set BR_aCODE=BRANCHCD||'" + newcode + "',GRP='" + newcode.Substring(0, 2).Trim() + "',acode='" + newcode + "' where TRIM(acode)='" + oldcode + "'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE upper(TNAME)='FAMSTADDL'", "TNAME");
            if (mhd != "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famstaddl set acode='" + newcode + "' where TRIM(acode)='" + oldcode + "'");

            }

        }
        else

            update_Famstbal_yrbal();

        delete_from_fam_Tables();

        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "confirm", "<script>$(document).ready(function(){ MyConfirm2('Old Code = <b>" + oldcode + "</b> New Code = <b>" + newcode + "</b> </br> " + " Are you Sure? '); });</script>", false);
    }
    public void chng_ac()
    {
        switch (hf2.Value)
        {
            case "0":
                string abcd;
                abcd = txtaname.Text.Substring(0, 1);
                group_code = txtacgp.Text.Trim();
                if (group_code == "02" || group_code == "04" || group_code == "05" || group_code == "06" || group_code == "14" || group_code == "16" || group_code == "17" || group_code == "15" || group_code == "08")
                {
                    group_code = group_code + abcd;
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP") == "SG_TYPE")
                    {
                        Seeksql = "SELECT MAX(SUBSTR(ACODE,4,4)) as acode FROM FAMST WHERE SUBSTR(ACODE,1,3)='" + group_code.Trim() + "'";
                        newcode = fgen.next_no(frm_qstr, frm_cocd, Seeksql, 4, "acode");
                    }
                    else
                    {
                        Seeksql = "SELECT MAX(SUBSTR(ACODE,4,3)) as acode FROM FAMST WHERE SUBSTR(ACODE,1,3)='" + group_code.Trim() + "'";
                        newcode = fgen.next_no(frm_qstr, frm_cocd, Seeksql, 3, "acode");
                    }
                    // newcode = next_no(group_code, 3, Seeksql);
                }
                else
                {
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP") == "SG_TYPE")
                    {
                        Seeksql = "SELECT MAX(SUBSTR(ACODE,4,4)) as acode FROM FAMST WHERE trim(nvl(GRP,'-'))='" + group_code.Trim() + "'";
                        newcode = fgen.next_no(frm_qstr, frm_cocd, Seeksql, 4, "acode");
                    }
                    else
                    {
                        Seeksql = "SELECT MAX(SUBSTR(ACODE,4,3)) as acode FROM FAMST WHERE trim(nvl(GRP,'-'))='" + group_code.Trim() + "'";
                        newcode = fgen.next_no(frm_qstr, frm_cocd, Seeksql, 3, "acode");
                    }

                    // newcode = next_no(group_code, 4, Seeksql);
                }
                newcode = group_code + newcode;
                oldcode = txtacode.Text.ToString().Trim();
                break;
            case "1":
                if ((txtacode1.Text.Trim() != "" && txtacode2.Text.Trim() != "") && (txtacode1.Text.Trim() != txtacode2.Text.Trim()))
                {
                    oldcode = txtacode1.Text.Trim();
                    newcode = txtacode2.Text.Trim();
                }
                else
                {
                    if ((txtfixcode.Text.Trim() != "") && (txtfixcode.Text.Length == 6))
                    {
                        oldcode = txtacode1.Text.Trim();
                        newcode = txtfixcode.Text.Trim();



                        mhd = "";
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(acode) as acode from famst where TRIM(ACODE)='" + txtfixcode.Text.Trim() + "'", "Acode");
                        if (mhd == "0" || mhd == "")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "insert into famst (grp,branchcd,acode,aname,balop,balcb,prevyr) values ( '" + txtfixcode.Text.Substring(0, 2).Trim() + "','00','" + txtfixcode.Text.Trim() + "','" + txtaname1.Text.Trim() + "',0,0,0)");
                            //cmd = new OracleCommand("insert into famst (grp,branchcd,acode,aname,balop,balcb,prevyr) values ( '" + txtfixcode.Text.Substring(0, 2).Trim() + "','00','" + txtfixcode.Text.Trim() + "','" + txtaname1.Text.Trim() + "',0,0,0)", consql);
                            //cmd.ExecuteNonQuery();

                        }
                        mhd = "";

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(acode) as acode from famstbal where TRIM(ACODE)='" + txtfixcode.Text.Trim() + "'", "Acode");
                        if (mhd == "0" || mhd == "")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "insert into famstbal (BR_aCODE,grp,branchcd,acode) values ('00" + txtfixcode.Text.Trim() + "', '" + txtfixcode.Text.Substring(0, 2).Trim() + "','" + frm_mbr + "','" + txtacode1.Text.Trim() + "')");
                            //cmd = new OracleCommand("insert into famstbal (BR_aCODE,grp,branchcd,acode) values ('00" + txtfixcode.Text.Trim() + "', '" + txtfixcode.Text.Substring(0, 2).Trim() + "','" + mbr + "','" + txtacode1.Text.Trim() + "')", consql);
                            //cmd.ExecuteNonQuery();

                        }
                        //consql.Close();
                    }
                }
                break;
            case "3":
                oldcode = TextBox1.Text.Trim();
                newcode = TextBox3.Text.Trim();
                break;
        }
        hfmode.Value = oldcode + "@" + newcode;
        string oldBuyCode = "", newBuyCode = "", oldLBTNO = "", newLBTNO = "", oldAname = "", newAname = "";
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BUYCODE||'~'||lbt_no||'~'||aname as col1 from famst where trim(Acode)='" + oldcode + "'", "COL1");
        if (mhd.Contains("~"))
        {
            oldBuyCode = mhd.Split('~')[0];
            oldLBTNO = mhd.Split('~')[1];
            oldAname = mhd.Split('~')[2];
        }
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BUYCODE||'~'||lbt_no||'~'||aname as col1 from famst where trim(Acode)='" + newcode + "'", "COL1");
        if (mhd.Contains("~"))
        {
            newBuyCode = mhd.Split('~')[0];
            newLBTNO = mhd.Split('~')[1];
            newAname = mhd.Split('~')[2];
        }

        string cmdInsertQry = "INSERT INTO WB_ACC_CLUB_LOG (BRANCHCD,ENT_BY,ENT_dT,OLD_ACODE,NEW_ACODE,OLD_BUYCODE,NEW_BUYCODE,OLD_LBTNO,NEW_LBTNO,old_aname,NEW_ANAME) VALUES" +
            "('" + frm_mbr + "','" + frm_uname + "',SYSDATE,'" + oldcode + "','" + newcode + "','" + oldBuyCode + "','" + newBuyCode + "','" + oldLBTNO + "','" + newLBTNO + "','" + oldAname + "','" + newAname + "' )";
        fgen.execute_cmd(frm_qstr, frm_cocd, cmdInsertQry);


        showmsg(oldcode, newcode);

        fgen.msg("-", "AMSG", "Accounts have been clubbed successfully!!'13'Old Code was " + oldcode.Trim() + "'13'New Code is " + newcode.Trim() + "");
        fgen.ResetForm(this.Controls);
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 == "N")
            return;
        else
        {
            chng_ac();
        }
    }

    public void update_Famstbal_yrbal()
    {
        double yr = 0;

        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();

        dt1 = fgen.getdata(frm_qstr, frm_cocd, "Select type1 from type where id='B' order by type1");

        dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select code from co where code like '" + frm_cocd + "%' order by code");



        if (dt2.Rows.Count > 0)
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                foreach (DataRow dr1 in dt1.Rows)
                {
                    mhd = "";
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select acode from famstbal where branchcd='" + dr1["type1"] + "' and trim(acode)='" + newcode + "'", "acode");
                    if (mhd == "" || mhd == "0")
                    {
                        Seeksql = "insert into famstbal (BR_aCODE,grp,branchcd,acode,bssch) values ('" + dr1["type1"].ToString().Trim() + newcode + "', '" + newcode.Substring(0, 2) + "','" + dr1["type1"].ToString().Trim() + "','" + newcode + "','" + newcode.Substring(0, 2) + "00')";
                        fgen.execute_cmd(frm_qstr, frm_cocd, Seeksql);
                        //cmd = new OracleCommand(Seeksql, consql);
                        //cmd.ExecuteNonQuery();

                    }
                    ////Seeksql = "Select yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + " as Op from famstbal where branchcd='" + dr1["type1"].ToString().Trim() + "' and trim(acode)='" + oldcode + "'";
                    ////yr = Convert.ToDouble(Convert.ToString(fgen.seek_iname(frm_qstr, frm_cocd, Seeksql , "op")));
                    ////if (yr != 0)
                    ////{
                    ////    Seeksql = "update famstBAL set yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + " =yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + " + " + yr + " where TRIM(acode)='" + newcode + "' and branchcd='" + dr1["type1"].ToString().Trim() + "'";
                    ////    fgen.execute_cmd(frm_qstr, frm_cocd, Seeksql);
                    ////    //cmd = new OracleCommand(Seeksql, consql);
                    ////    //cmd.ExecuteNonQuery();

                    ////}

                }
            }
        }

        if (dt2.Rows.Count > 0)
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                foreach (DataRow dr1 in dt1.Rows)
                {
                    //////mhd = "";
                    //////mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select acode from famstbal where branchcd='" + dr1["type1"] + "' and trim(acode)='" + newcode + "'", "acode");
                    //////if (mhd == "" || mhd == "0")
                    //////{
                    //////    Seeksql = "insert into famstbal (BR_aCODE,grp,branchcd,acode,bssch) values ('" + dr1["type1"].ToString().Trim() + newcode + "', '" + newcode.Substring(0, 2) + "','" + dr1["type1"].ToString().Trim() + "','" + newcode + "','" + newcode.Substring(0, 2) + "00')";
                    //////    fgen.execute_cmd(frm_qstr, frm_cocd, Seeksql);
                    //////    //cmd = new OracleCommand(Seeksql, consql);
                    //////    //cmd.ExecuteNonQuery();

                    //////}
                    Seeksql = "Select yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + " as Op from famstbal where branchcd='" + dr1["type1"].ToString().Trim() + "' and trim(acode)='" + oldcode + "'";
                    yr = Convert.ToDouble(Convert.ToString(fgen.seek_iname(frm_qstr, frm_cocd, Seeksql, "op")));
                    if (yr != 0)
                    {
                        Seeksql = "update famstBAL set yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + " =nvl(yr_" + dr2["CODE"].ToString().Substring(4, 4).Trim() + ",0) + " + yr + " where TRIM(acode)='" + newcode + "' and branchcd='" + dr1["type1"].ToString().Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, Seeksql);
                        //cmd = new OracleCommand(Seeksql, consql);
                        //cmd.ExecuteNonQuery();

                    }

                }
            }
        }



    }

    private void delete_from_fam_Tables()
    {

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE upper(TNAME)='FAMSTADDL'", "TNAME");


        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from famst where trim(acode)='" + oldcode + "'");
        //cmd = new OracleCommand("delete from famst where trim(acode)='" + oldcode + "'", consql);
        //cmd.ExecuteNonQuery();


        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from famstBAL where trim(acode)='" + oldcode + "'");
        //cmd = new OracleCommand("delete from famstBAL where trim(acode)='" + oldcode + "'", consql);
        //cmd.ExecuteNonQuery();

    }

    private void update_Trans_Files()
    {
        DataTable dtTList = new DataTable();
        dtTList = fgen.getdata(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TABtYPE='TABLE' and TNAME NOT IN ('FAMST','FAMSTBAL') ORDER BY TNAME");

        foreach (DataRow drTname in dtTList.Rows)
        {
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, drTname["TNAME"].ToString().ToUpper(), "ACODE");
            if (mhd != "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET acode='" + newcode.Trim() + "' WHERE TRIM(acode)='" + oldcode + "' ");
            }


            if (drTname["TNAME"].ToString().ToUpper() == "SCRATCH")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, drTname["TNAME"].ToString().ToUpper(), "COL24");
                if (mhd != "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET COL24='" + newcode.Trim() + "' WHERE TRIM(COL24)='" + oldcode + "' ");
            }
            if (drTname["TNAME"].ToString().ToUpper() == "CONTROLS")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET PARAMS='" + newcode.Trim() + "' WHERE TRIM(PARAMS)='" + oldcode + "' ");
            }
            if (drTname["TNAME"].ToString().ToUpper() == "HUNDI")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");
            }
            if (drTname["TNAME"].ToString().ToUpper() == "IVOUCHERW")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");
            }
            if (drTname["TNAME"].ToString().ToUpper() == "IVOUCHERJ")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET VCODE='" + newcode.Trim() + "' WHERE TRIM(VCODE)='" + oldcode + "' ");

            }
            if (drTname["TNAME"].ToString().ToUpper() == "IVOUCHER")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");


                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET VCODE='" + newcode.Trim() + "' WHERE TRIM(VCODE)='" + oldcode + "' ");
            }




            if (drTname["TNAME"].ToString().ToUpper() == "ITEM")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET AC_aCODE='" + newcode.Trim() + "' WHERE TRIM(AC_aCODE)='" + oldcode + "' ");

            }
            if (drTname["TNAME"].ToString().ToUpper() == "IvoucherP")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET vCODE='" + newcode.Trim() + "' WHERE TRIM(vCODE)='" + oldcode + "' ");
            }


            if (drTname["TNAME"].ToString().ToUpper() == "VOUCHER")
            {
                //fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET ACODE='" + newcode.Trim() + "' WHERE TRIM(ACODE)='" + oldcode + "' ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET rCODE='" + newcode.Trim() + "' WHERE TRIM(rCODE)='" + oldcode + "' ");

            }

            if (drTname["TNAME"].ToString().ToUpper() == "VOUCHERP")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");
            }

            if (drTname["TNAME"].ToString().ToUpper() == "TDS")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");

            }

            if (drTname["TNAME"].ToString().ToUpper() == "RGPMST")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET RCODE='" + newcode.Trim() + "' WHERE TRIM(RCODE)='" + oldcode + "' ");

            }

            if (drTname["TNAME"].ToString().ToUpper() == "TPTMST")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + drTname["TNAME"].ToString().ToUpper() + " SET BCODE='" + newcode.Trim() + "' WHERE TRIM(BCODE)='" + oldcode + "' ");

            }

        }
    }




    protected void btnacode_Click(object sender, EventArgs e)
    {
        hfname.Value = "ACD";
        disp_list();
        fgen.Fn_open_sseek("Select Acode ", frm_qstr);
        //ScriptManager.RegisterStartupScript(btnacode, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
    }

    protected void btnagrp_Click(object sender, EventArgs e)
    {
        hfname.Value = "MG";
        disp_list();
        fgen.Fn_open_sseek("Select Acode Group ", frm_qstr);
        //ScriptManager.RegisterStartupScript(btnagrp, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
    }
    protected void btnscode_Click(object sender, EventArgs e)
    {
        hfname.Value = "SC";
        disp_list();
        fgen.Fn_open_sseek("Select Acode Group ", frm_qstr);
        // ScriptManager.RegisterStartupScript(btnscode, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
    }


    protected void btn_club_Click(object sender, EventArgs e)
    {
        if (frm_cocd == "SGRP")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please user 2nd/3rd Tab to Club the Accounts");
            return;
        }

        nflag = 0;
        hf2.Value = "0";
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        if (txtacode.Text.Trim() == "120000" || txtacode.Text.Trim() == "070009" || txtacode.Text.Trim() == "070010" || txtacode.Text.Trim() == "070014" || txtacode.Text.Trim() == "070020")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", The Account Group for this Account can not be changed");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('The Account Group for this Account can not be changed');});</script>", false);
        }

        if (txtacode1.Text.Trim() == "120000" || txtacode1.Text.Trim() == "070009" || txtacode1.Text.Trim() == "070010" || txtacode1.Text.Trim() == "070014" || txtacode1.Text.Trim() == "070020")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", The Account Group for this Account can not be changed");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('The Account Group for this Account can not be changed');});</script>", false);
        }
        if (txtacode.Text.Trim() == "630018")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", The Account Group for this Account can not be changed");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('The Account Group for this Account can not be changed');});</script>", false);
        }

        if (txtacgp.Text.Length < 2)
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Incorrect Account Group Choosen .");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Incorrect Account Group Choosen .');});</script>", false);
        }

        if (Convert.ToInt32(ulvl) == 0) { }
        else
        {
            if (txtacode.Text.Substring(0, 2) == "1Z" && txtacgp.Text.Substring(0, 2) == "1Z")
            {
                nflag = 1;
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Selected Account  is an Asset/liability, '13' Hence cannot be allowed to shift to income or expense groups");
                return;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Selected Account  is an Asset/liability, " + "</br>" + "Hence cannot be allowed to shift to income or expense groups');});</script>", false);
            }
            else if (txtacode.Text.Substring(0, 2) == "04" || txtacode.Text.Substring(0, 2) == "08" || txtacode.Text.Substring(0, 2) == "02" || txtacode.Text.Substring(0, 2) == "05" || txtacode.Text.Substring(0, 2) == "06" || txtacode.Text.Substring(0, 2) == "16" && txtacgp.Text.Substring(0, 2) != "08" && txtacgp.Text.Substring(0, 2) != "04" && txtacgp.Text.Substring(0, 2) != "02" && txtacgp.Text.Substring(0, 2) != "05" && txtacgp.Text.Substring(0, 2) != "06" && txtacgp.Text.Substring(0, 2) != "16")
            {
                nflag = 1;
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Account  is an Debtor/Creditor, '13' Hence Can be allowed to shift to 02,05,06,16 Groups only");
                return;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Selected Account  is an Debtor/Creditor, " + "</br>" + "Hence Can be allowed to shift to 02,05,06,16 Groups only');});</script>", false);
            }
        }
        if (txtacode.Text.Trim() == "")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Account Code");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Select Account Code');});</script>", false);
        }
        if (txtacgp.Text.Trim() == "")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Account Grop");
            return;
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Select Account Grop');});</script>", false);
        }


        if (nflag == 1) { }
        else
        {
            //hfname.Value = "SURE_S";
            //disp_list();            
            fgen.msg("-", "SMSG", "Are you Sure, you want to change code.? ");
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        scode = ""; sname = "";


        if (Request.Cookies["Column1"] != null) scode = Request.Cookies["Column1"].Value.ToString();
        if (Request.Cookies["Column2"] != null) sname = Request.Cookies["Column2"].Value.ToString();

        scode = scode.Replace("&amp", "");
        sname = sname.Replace("&amp", "");

        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
        col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");

        btnmode = hfname.Value;

        switch (btnmode)
        {

            case "ACD":

                txtacode.Text = col1;
                txtaname.Text = col2;
                break;
            case "MG":

                txtacgp.Text = col1;
                txtacname.Text = col2;

                break;
            case "SC":

                txtscode.Text = col1;
                txtsname.Text = col2;

                break;
            case "ACD1":

                txtacode1.Text = col1;
                txtaname1.Text = col2;

                break;
            case "ACD2":

                txtacode2.Text = col1;
                txtaname2.Text = col2;

                break;
            case "ACD3":
                TextBox1.Text = col1;
                TextBox2.Text = col2;
                break;
        }
    }
    protected void btnacode1_Click(object sender, EventArgs e)
    {
        hfname.Value = "ACD1";
        disp_list();
        fgen.Fn_open_sseek("Select Account", frm_qstr);

    }
    protected void btnacode2_Click(object sender, EventArgs e)
    {
        hfname.Value = "ACD2";
        disp_list();
        fgen.Fn_open_sseek("Select Account", frm_qstr);

    }
    protected void btn_ext_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    protected void btn_similar_ServerClick(object sender, EventArgs e)
    {
        oldcode = txtacode1.Text.Trim();
        newcode = txtacode2.Text.Trim();

        nflag = 0;
        hf2.Value = "1";
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        if (txtacode1.Text.Trim() == "")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Account to remove");
            return;
        }

        if (txtacode2.Text.Trim() == "" && txtfixcode.Text.Trim() == "")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select either account to continue or fixed a/c code");
            return;
        }

        if (txtacode1.Text.Trim() == txtacode2.Text.Trim())
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You have Chosen Same Code , Not ok to Proceed ");
            return;
        }

        if (txtacode1.Text.Substring(0, 2) == "02" && txtacode2.Text.Substring(0, 2) == "02")
        {
            nflag = 1;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This is Reserved Code for Inter Branch Accounts '13' This Cannot be Changed");
            return;
        }
        if (Convert.ToInt32(ulvl) == 0) { }
        else
        {
            if (txtacode1.Text.Substring(0, 2) == "1Z" && txtacode2.Text.Substring(0, 2) == "1Z")
            {
                nflag = 1;
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Account  is an Asset/liability, '13' Hence cannot be allowed to shift to income or expense groups");
                return;
            }
            else if (txtacode1.Text.Substring(0, 2) == "02" || txtacode1.Text.Substring(0, 2) == "05" || txtacode.Text.Substring(0, 2) == "06" || txtacode.Text.Substring(0, 2) == "16" && txtacode2.Text.Substring(0, 2) != "02" && txtacode2.Text.Substring(0, 2) != "05" && txtacode2.Text.Substring(0, 2) != "06" && txtacode2.Text.Substring(0, 2) != "16" && txtfixcode.Text == "")
            {
                nflag = 1;
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Account  is an Debtor/Creditor, '13' Hence Can be allowed to shift to 02,05,06,16 Groups only");
                return;
            }
        }


        if (nflag == 1) { }
        else
        {
            //hfname.Value = "SURE_S";
            //disp_list();
            fgen.msg("-", "SMSG", "Old Code = " + oldcode + "'13'New Code = " + newcode + "'13''13'" + "Are you Sure?");
        }
    }
    protected void btn_new_ServerClick(object sender, EventArgs e)
    {
        fgen.EnableForm(this.Controls);
    }
    protected void btn_cancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
    }
    protected void Button1_ServerClick(object sender, EventArgs e)
    {
        oldcode = TextBox1.Text.Trim();
        newcode = TextBox3.Text.Trim();
        hf2.Value = "3";
        {
            //hfname.Value = "SURE_S";
            //disp_list();
            fgen.msg("-", "SMSG", "Old Code = " + oldcode + "'13'New Code = " + newcode + "'13''13'" + "Are you Sure?");
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        hfname.Value = "ACD3";
        disp_list();
        fgen.Fn_open_sseek("Select Account", frm_qstr);
    }
}