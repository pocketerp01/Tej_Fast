using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Threading;
using System.Xml;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

//using ZXing;
//using ZXing.PDF417;
//using QRCoder;


    public class fgenLG
    {
        public SmtpClient smtp;
        public MailMessage mail;
        public XmlDocument docxml = new XmlDocument();
        public XmlNodeList getval;
        string sender_id = "", pwd, vsmtp, xvip, xport, resultVal = "", textName = "", resultMsg = "", CCMID = "", frm_grp = "", chkActiVated;
        public string valFound = "N";
        int ssl, port;

        public void DisableForm(ControlCollection control)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is Button) ((Button)(c)).Enabled = true;
                if (c is DropDownList) ((DropDownList)(c)).Enabled = false;
                if (c is TextBox) ((TextBox)(c)).Enabled = false;
                if (c is HtmlInputControl) ((HtmlInputControl)(c)).Disabled = true;
                if (c is RadioButton) ((RadioButton)(c)).Enabled = false;
                if (c is RadioButtonList) ((RadioButtonList)(c)).Enabled = false;
                if (c is Label) ((Label)(c)).Enabled = false;
                if (c is ImageButton) ((ImageButton)(c)).Enabled = false;
                if (c.HasControls()) DisableForm(c.Controls);
            }
        }
        /// <summary>
        /// search data in datatable and returns searched data in datatable
        /// </summary>

        /// <param name="input">Datatable in which we have to search</param>
        /// <returns>Datatable with searched value</returns>
        public DataTable searchDataTable(string searchText, DataTable input)
        {
            DataTable output = input.Clone();
            foreach (DataColumn dc in input.Columns)
            {
                if (dc.ColumnName.ToUpper().Contains(searchText.ToUpper())) return input;
            }
            foreach (DataRow dr in input.Rows)
            {
                for (int i = 0; i < input.Columns.Count; i++)
                {
                    if (dr[i].ToString().ToUpper().Contains(searchText.ToUpper()))
                    {
                        DataRow drnew = output.NewRow();
                        drnew.ItemArray = dr.ItemArray;
                        output.Rows.Add(drnew);
                        break;
                    }
                }
            }
            return output;
        }
        /// <summary>
        /// search data in datatable and returns searched data in datatable
        /// </summary>
        /// <param name="searchText">Text entered by user</param>
        /// <param name="input">Datatable in which we have to search</param>
        /// <returns>Datatable with searched value</returns>
        public DataTable searchDataTable1(string searchText, DataTable input)
        {
            DataTable output = input.Clone();
            foreach (DataColumn dc in input.Columns)
            {
                if (dc.ColumnName.ToUpper().Contains(searchText.ToUpper())) return input;
            }
            DataTable fg_srch_table = input;
            fg_srch_table.Columns.Add(new DataColumn("search_data_fgen_fstr", typeof(string)));
            foreach (DataRow dr in input.Rows)
            {
                for (int i = 0; i < input.Columns.Count; i++)
                {
                    if (dr[i].ToString().ToUpper().Contains(searchText.ToUpper()))
                    {
                        DataRow drnew = output.NewRow();
                        drnew.ItemArray = dr.ItemArray;
                        output.Rows.Add(drnew);
                        break;
                    }
                }
            }
            return output;
        }
        public void EnableForm(ControlCollection control)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is Button) ((Button)(c)).Enabled = true;
                if (c is DropDownList) ((DropDownList)(c)).Enabled = true;
                if (c is TextBox) ((TextBox)(c)).Enabled = true;
                if (c is HtmlInputControl) ((HtmlInputControl)(c)).Disabled = false;
                if (c is RadioButton) ((RadioButton)(c)).Enabled = true;
                if (c is RadioButtonList) ((RadioButtonList)(c)).Enabled = true;
                if (c is Label) ((Label)(c)).Enabled = true;
                if (c is ImageButton) ((ImageButton)(c)).Enabled = true;
                if (c.HasControls()) EnableForm(c.Controls);
            }
        }
        public void ResetForm(ControlCollection control)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is TextBox) ((TextBox)(c)).Text = string.Empty;
                if (c is HtmlInputControl) ((HtmlInputControl)(c)).Value = string.Empty;
                if (c.HasControls()) ResetForm(c.Controls);
            }
        }
        public void fill_dash(ControlCollection control)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is TextBox)
                {
                    if ((((TextBox)c).Text.Trim() == null) || (((TextBox)c).Text.Trim() == "")) ((TextBox)c).Text = "-";
                }
                else if (c is HtmlInputText)
                {
                    if (((HtmlInputText)c).Value.Trim() == "" || ((HtmlInputText)c).Value.Trim() == null) ((HtmlInputText)c).Value = "-";
                }
                else
                {
                    if (c.HasControls()) fill_dash(c.Controls);
                }
            }
        }
        public void fill_zero(ControlCollection control)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is TextBox)
                {
                    if ((((TextBox)c).Text.Trim() == null) || (((TextBox)c).Text.Trim() == "") ||
                        (((TextBox)c).Text.Trim() == "NaN") || (((TextBox)c).Text.Trim() == "Infinity"))
                        ((TextBox)c).Text = "0";
                }
                else
                {
                    if (c.HasControls()) fill_zero(c.Controls);
                }
            }
        }
        public string check_special_char(ControlCollection control)
        {
            string[] characters = new string[2] { "'", "\"" };
            foreach (System.Web.UI.Control c in control)
            {
                string bordercolor = "1px solid red";
                foreach (string character in characters)
                {
                    if (c is TextBox)
                    {
                        if (((TextBox)(c)).Text.Contains(character))
                        {
                            resultVal = "Y";
                            ((TextBox)(c)).Style.Add("border", bordercolor);
                            return resultVal;
                        }
                        else ((TextBox)(c)).Style.Remove("border");
                    }
                    if (c is HtmlInputText)
                    {
                        if (((HtmlInputText)(c)).Value.Contains(character))
                        {
                            resultVal = "Y";
                            ((HtmlInputText)(c)).Style.Add("border", bordercolor);
                            return resultVal;
                        }
                        else ((HtmlInputText)(c)).Style.Remove("border");
                    }
                }
                if (c.HasControls() && resultVal == "") check_special_char(c.Controls);
            }
            return resultVal;
        }
        public string check_special_char(ControlCollection control, string character)
        {
            string resultVal = "N";
            foreach (System.Web.UI.Control c in control)
            {
                string bordercolor = "1px solid red";
                if (c is TextBox)
                {
                    if (((TextBox)(c)).Text.Contains(character))
                    {
                        resultVal = "Y";
                        ((TextBox)(c)).Style.Add("border", bordercolor);
                        break;
                    }
                }
                if (c is HtmlInputText)
                {
                    if (((HtmlInputText)(c)).Value.Contains(character))
                    {
                        resultVal = "Y";
                        ((HtmlInputText)(c)).Style.Add("border", bordercolor);
                        break;
                    }
                }
                if (c.HasControls()) check_special_char(c.Controls, character);
            }
            return resultVal;
        }
        public void SetHeadingCtrl(ControlCollection control, DataTable dtCtrlCheck)
        {
            foreach (System.Web.UI.Control c in control)
            {
                //if (c.ID != null)
                {
                    string bordercolor = "1px solid red";
                    string myTextJS = "checkTextValIsValid(this)";
                    int maxLen = 0;
                    if (c is Label)
                    {
                        try
                        {
                            if (((Label)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME"))
                            {

                                string reg_heads = "";
                                reg_heads = seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_CAPTION_REG");
                                if (reg_heads.Trim() == "" || reg_heads.Trim() == "-" || reg_heads.Trim() == "0")
                                {
                                    ((Label)(c)).Text = seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_CAPTION");
                                }
                                else
                                {
                                    ((Label)(c)).Text = reg_heads.Replace(" ", "_");
                                }

                                if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_VISIBLE") == "N") ((Label)(c)).Visible = false;
                            }
                        }
                        catch { }
                    }
                    if (c is TextBox)
                    {
                        if ((((TextBox)(c)).ID.ToUpper().Replace("TXT", "") == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_NAME")) || (((TextBox)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME")))
                        {
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_VISIBLE") == "N" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_VISIBLE") == "N") ((TextBox)(c)).Visible = false;
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_READONLY") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_READONLY") == "Y") ((TextBox)(c)).Attributes.Add("readonly", "readonly");
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_FMAND") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FMAND") == "Y")
                            {
                                ((TextBox)(c)).Style.Remove("border");
                                if (((TextBox)(c)).Text == "" || ((TextBox)(c)).Text == "-")
                                    ((TextBox)(c)).Style.Add("border", bordercolor);
                                ((TextBox)(c)).Attributes.Add("onchange", myTextJS);
                            }
                            maxLen = make_int(seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_MAXLEN"));
                            if (maxLen > 0) ((TextBox)(c)).MaxLength = maxLen;
                            maxLen = make_int(seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_MAXLEN"));
                            if (maxLen > 0) ((TextBox)(c)).MaxLength = maxLen;
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FIELD_CAST") == "D")
                                ((TextBox)(c)).TextMode = TextBoxMode.Date;
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FIELD_CAST") == "N")
                            {
                                ((TextBox)(c)).TextMode = TextBoxMode.Number;
                                ((TextBox)(c)).Style.Add("text-align", "right");
                            }
                        }
                    }
                    if (c is HtmlInputText)
                    {
                        if ((((HtmlInputText)(c)).ID.ToUpper().Replace("TXT", "") == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_NAME")) || (((HtmlInputText)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME")))
                        {
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_VISIBLE") == "N" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_VISIBLE") == "N") ((HtmlInputText)(c)).Visible = false;
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_READONLY") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_READONLY") == "Y") ((HtmlInputText)(c)).Attributes.Add("readonly", "readonly");
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_FMAND") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FMAND") == "Y")
                            {
                                ((HtmlInputText)(c)).Style.Remove("border");
                                if (((HtmlInputText)(c)).Value == "" || ((HtmlInputText)(c)).Value == "-")
                                    ((HtmlInputText)(c)).Style.Add("border", bordercolor);
                                ((HtmlInputText)(c)).Attributes.Add("onchange", myTextJS);
                            }
                            maxLen = make_int(seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_MAXLEN"));
                            if (maxLen > 0) ((HtmlInputText)(c)).MaxLength = maxLen;
                            maxLen = make_int(seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_MAXLEN"));
                            if (maxLen > 0) ((HtmlInputText)(c)).MaxLength = maxLen;

                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FIELD_CAST") == "D")
                                ((HtmlInputText)(c)).Attributes.Add("type", "date");
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FIELD_CAST") == "N")
                            {
                                ((HtmlInputText)(c)).Attributes.Add("type", "number");
                                ((HtmlInputText)(c)).Style.Add("text-align", "right");
                            }
                        }
                    }
                    if (c is ImageButton)
                    {
                        if (((ImageButton)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME"))
                        {
                            if (seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_VISIBLE") == "N") ((ImageButton)(c)).Visible = false;
                        }
                    }
                    if (c.HasControls()) SetHeadingCtrl(c.Controls, dtCtrlCheck);
                }
            }
        }

        public string checkMandatoryFields(ControlCollection control, DataTable dtCtrlCheck)
        {
            foreach (System.Web.UI.Control c in control)
            {
                if (c is TextBox)
                {
                    if ((((TextBox)(c)).ID.ToUpper().Replace("TXT", "") == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_NAME")) || (((TextBox)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME")))
                    {
                        if ((seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_FMAND") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FMAND") == "Y") && (((TextBox)(c)).Text == "-" || ((TextBox)(c)).Text == ""))
                        {
                            textName = seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_CAPTION");
                            if (textName.Length > 1)
                                resultMsg += " | " + textName;
                        }
                    }
                }
                if (c is HtmlInputText)
                {
                    if ((((HtmlInputText)(c)).ID.ToUpper().Replace("TXT", "") == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_NAME")) || (((HtmlInputText)(c)).ID.ToUpper() == seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_NAME")))
                    {
                        if ((seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString().Replace("TXT", "") + "'", "OBJ_FMAND") == "Y" || seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_FMAND") == "Y") && (((HtmlInputText)(c)).Value == "-" || ((HtmlInputText)(c)).Value == ""))
                        {
                            textName = seek_iname_dt(dtCtrlCheck, "OBJ_NAME='" + c.ID.ToUpper().ToString() + "'", "OBJ_CAPTION");
                            if (textName.Length > 1)
                                resultMsg += " | " + textName;
                        }
                    }
                }
                if (textName.Length > 1)
                {
                    resultMsg = resultMsg.TrimStart('|');
                    resultVal = "Value not Entered for " + resultMsg;
                }
                if (c.HasControls() && resultVal == "") checkMandatoryFields(c.Controls, dtCtrlCheck);
            }
            return resultVal;
        }

        public void msg(string titl, string ctype, string msgval)
        {
            if (titl.Trim() == "-") titl = "Alert Message";
            msgval = msgval.Replace("<", "");
            msgval = msgval.Replace(">", "");
            msgval = msgval.Replace("\"", "");
            if (msgval != null && msgval.Length > 2)
            {
                send_cookie("mid", ctype);
                send_cookie("send_msg", msgval);
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/msg.aspx");

                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUp", "Alert('" + fil_loc + "','" + titl + "');", true);
                }
            }
            FILL_Log(msgval);

        }
        public void msgBig(string qstr, string titl, string ctype, string msgval)
        {
            if (titl.Trim() == "-") titl = "Alert Message";
            if (msgval != null && msgval.Length > 2)
            {
                send_cookie("mid", ctype);
                fgenMV.Fn_Set_Mvar(qstr, "U_MSG", msgval);
                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;
                    string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/msg1.aspx");

                    p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUp", "Alertvip('" + fil_loc + "?STR=" + qstr + "','500px','500px','" + titl + "');", true);
                }
            }
        }
        public void Fn_open_icon(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/open_icon.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string heightper = "70%";
                string widthper = "80%";

                //heightper = "90%";
                //widthper = "75%";

                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle2('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_sseek(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Sseek.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string heightper = "80%";
                string widthper = "65%";

                //heightper = "90%";
                //widthper = "75%";

                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_sseek_Big(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Sseek.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string heightper = "80%";
                string widthper = "65%";

                //heightper = "90%";
                //widthper = "75%";

                //if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void DTFn_open_sseek(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/SseekDT.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string heightper = "80%";
                string widthper = "65%";

                //heightper = "90%";
                //widthper = "75%";

                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void DTFn_open_mseek(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/mseekDT.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string heightper = "80%";
                string widthper = "65%";

                //heightper = "90%";
                //widthper = "75%";

                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_ValueBox(string titl, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUp", "Alert('" + fil_loc + "?STR=" + QR_str + "','" + titl + "');", true);
            }
        }
        public void Fn_ValueBoxMultiple(string titl, string QR_str, string widthPer, string heightPer)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival_multiple.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthPer + "','" + heightPer + "','" + titl + "');", true);
            }
        }
        public void Fn_ValueBoxFinance(string titl, string QR_str, string widthPer, string heightPer)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival_finance.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthPer + "','" + heightPer + "','" + titl + "');", true);
            }
        }

        public void Fn_open_Act_itm_prd(string title, string QR_str)
        {
            frm_grp = fgenMV.Fn_Get_Mvar(QR_str, "U_COGRP");
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_Act_itm_prd.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_PartyItemDateRangeBox(string title, string QR_str)
        {
            frm_grp = fgenMV.Fn_Get_Mvar(QR_str, "U_COGRP");
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/PartyitmRangeBox.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;

                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }

        public void Fn_open_ItemBox(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/PartyitmRangeBox.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_PartyBox(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "PARTY");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/itmBox.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_PartyItemBox(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/PitmBox.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_RangeBox(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/RangeBox.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string widthper = "1000px";
                string heightper = "610px";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    widthper = "95%";
                    heightper = "95%";
                }

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
            }
        }
        public void Fn_open_rptlevel(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RALIGN", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RESIZE", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_WIDTHS", "");

            drillQuery(0, fgenMV.Fn_Get_Mvar(QR_str, "U_SEEKSQL"), QR_str);

            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/drillDown.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_open_rptlevel(string titl, string QR_str, string colsToSumm)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RALIGN", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RESIZE", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_WIDTHS", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_SUM", colsToSumm);
            drillQuery(0, fgenMV.Fn_Get_Mvar(QR_str, "U_SEEKSQL"), QR_str);

            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/drillDownpr.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_DrillReport(string titl, string QR_str, string colsToSumm)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RALIGN", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RESIZE", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_WIDTHS", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_SUM", colsToSumm);

            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/drillDownpr.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_open_rptlevelJS(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/rptlevelJS.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_open_rptlevelIMG(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/rptlevel_img.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','95%','95%','" + titl.Replace("'", "`") + "');", true);
            }
        }

    
        public void Fn_DrillReport(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RALIGN", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RESIZE", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_WIDTHS", "");
            fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_SUM", "");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/drillDownpr.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_DrillReport(string titl, string QR_str, string col2_align, string col2_rsize, string col2_width)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {

                fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RALIGN", col2_align);
                fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_2RESIZE", col2_rsize);
                fgenMV.Fn_Set_Mvar(QR_str, "U_COLS_WIDTHS", col2_width);

                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/drillDown.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','95%','610px','" + titl.Replace("'", "`") + "');", true);


            }
        }
        public void Fn_FinanceDrill(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_fin_drill.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','98%','98%','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_Open_Chart(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/chrtlevel1.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','95%','610px','" + titl.Replace("'", "`") + "');", true);
            }
        }

        public void Fn_Open_ChartFunnel(string titl, string QR_str)
        {
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/chrtlevelFunnel.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','95%','610px','" + titl.Replace("'", "`") + "');", true);
            }
        }
        /// <summary>
        /// Dual Date 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="QR_str"></param>
        public void Fn_open_prddmp1(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prdemp1.aspx");

                string heightper = "300px";
                string widthper = "600px";

                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "300px";
                    widthper = "350px";
                }
                //p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','500px','260px','" + title + "');", true);

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);

            }
        }
        public void Fn_open_prddmp2(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prdemp2.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','500px','260px','" + title + "');", true);
            }
        }
        /// <summary>
        /// Single Date Box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="QR_str"></param>
        public void Fn_open_dtbox(string title, string QR_str)
        {
            if (title.Trim() == "-" && frm_grp.Trim() == "T") title = "tejaxo ERP";
            else if (title.Trim() == "-" && frm_grp.Trim() == "S") title = "Sofgen ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_dtbox.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','300px','200px','" + title + "');", true);
            }
        }
        public void open_pwdbox(string titl, string QR_str, Control btnName)
        {
            fgenMV.Fn_Set_Mvar(QR_str, "U_BTNNAME", btnName.ID.ToString());
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/apwd.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUP", "Alert1('" + fil_loc + "?STR=" + QR_str + "','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void open_pwdbox(string titl, string QR_str)
        {
            fgenMV.Fn_Set_Mvar(QR_str, "U_BTNNAME", "-");
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/apwd.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUP", "Alert1('" + fil_loc + "?STR=" + QR_str + "','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void ActiveBox(string titl, string QR_str)
        {
            fgenMV.Fn_Set_Mvar(QR_str, "U_BTNNAME", "-");
            if (titl.Trim() == "-") titl = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/activPwd.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUP", "Alert1('" + fil_loc + "?STR=" + QR_str + "','" + titl.Replace("'", "`") + "');", true);
            }
        }
        public void Fn_open_mseek(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/MSEEK.aspx");
                string heightper = "80%";
                string widthper = "65%";
                if (fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() > 0 && fgenMV.Fn_Get_Mvar(QR_str, "FRMWINDOWSIZE").toDouble() < 800)
                {
                    heightper = "95%";
                    widthper = "95%";
                }
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','" + widthper + "','" + heightper + "','" + title + "');", true);
                //p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','900px','490px','" + title + "');", true);
            }
        }
        public void Fn_open_helpBox(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/helpInfo.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','900px','490px','" + title + "');", true);
            }
        }
        /// <summary>
        /// Used to Send Cookie
        /// </summary>
        /// <param name="name">Cookie Name</param>
        /// <param name="val">Cookie Value</param>
        public void send_cookie(string name, string val)
        {
            HttpCookie HC;
            try
            {
                if (name == "send_msg")
                    HC = new HttpCookie(name, HttpContext.Current.Server.UrlEncode(val));
                else HC = new HttpCookie(name, val);
                HC.Expires = DateTime.Now.AddMinutes(30);
                HttpContext.Current.Response.Cookies.Add(HC);
                HttpContext.Current.Response.Charset = "UTF-8";

                if (name.ToUpper().Contains("U_CDT1"))
                {

                }
            }
            catch { }
        }
        public void kill_cookie()
        {
            try
            {
                // Cookies
                if (HttpContext.Current.Request.Cookies["CO_CD"] != null) HttpContext.Current.Request.Cookies["CO_CD"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["BRANCH"] != null) HttpContext.Current.Request.Cookies["BRANCH"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["YEAR_SL"] != null) HttpContext.Current.Request.Cookies["YEAR_SL"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["UNAME"] != null) HttpContext.Current.Request.Cookies["UNAME"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["ULEVEL"] != null) HttpContext.Current.Request.Cookies["ULEVEL"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["rid"] != null) HttpContext.Current.Request.Cookies["rid"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["xid"] != null) HttpContext.Current.Request.Cookies["xid"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["srchSql"] != null) HttpContext.Current.Request.Cookies["srchSql"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["seekSql"] != null) HttpContext.Current.Request.Cookies["seekSql"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Value1"] != null) HttpContext.Current.Request.Cookies["Value1"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Value2"] != null) HttpContext.Current.Request.Cookies["Value2"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Value3"] != null) HttpContext.Current.Request.Cookies["Value3"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Vip"] != null) HttpContext.Current.Request.Cookies["Vip"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["sq1"] != null) HttpContext.Current.Request.Cookies["sq1"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["sq2"] != null) HttpContext.Current.Request.Cookies["sq2"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["RPTFILE"] != null) HttpContext.Current.Request.Cookies["RPTFILE"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["YVAL"] != null) HttpContext.Current.Request.Cookies["YVAL"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["mq5"] != null) HttpContext.Current.Request.Cookies["mq5"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Send_Mail"] != null) HttpContext.Current.Request.Cookies["Send_Mail"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["exetime"] != null) HttpContext.Current.Request.Cookies["exetime"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["xprdrange"] != null) HttpContext.Current.Request.Cookies["xprdrange"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["xprd1"] != null) HttpContext.Current.Request.Cookies["xprd1"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["branch_cd"] != null) HttpContext.Current.Request.Cookies["branch_cd"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["fromdt"] != null) HttpContext.Current.Request.Cookies["fromdt"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["todt"] != null) HttpContext.Current.Request.Cookies["todt"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["Value1"] != null) HttpContext.Current.Request.Cookies["Value1"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["cDT1"] != null) HttpContext.Current.Request.Cookies["cDT1"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["cDT2"] != null) HttpContext.Current.Request.Cookies["cDT2"].Expires = DateTime.Now;
                if (HttpContext.Current.Request.Cookies["MPRN"] != null) HttpContext.Current.Request.Cookies["MPRN"].Expires = DateTime.Now;
                // Sessions
                if (HttpContext.Current.Session["RPTDATA"] != null) HttpContext.Current.Session["RPTDATA"] = null;
                if (HttpContext.Current.Session["user_name"] != null) HttpContext.Current.Session["user_name"] = null;
            }
            catch { }
        }
    public int ChkDate(string cdate)
    {
        //if (cdate.Length <= 0 || cdate == null) return 0;
        //string datestr = Convert.ToDateTime(cdate).ToString("dd/MM/yyyy");
        //string format = "dd/MM/yyyy";
        //DateTime dateValue;
        //if (DateTime.TryParseExact(datestr, format, new CultureInfo("en-GB"), DateTimeStyles.None, out dateValue)) return 1;
        //else return 0;

        int date = 0;
        if (cdate.Length <= 0 || cdate == null) return 0;
        string datestr = "";
        try
        {
            datestr = Convert.ToDateTime(cdate).ToString("dd/MM/yyyy");
            string format = "dd/MM/yyyy";
            DateTime dateValue;
            if (DateTime.TryParseExact(datestr, format, new CultureInfo("en-GB"), DateTimeStyles.None, out dateValue))
            {
                date = 1;
            }
            else
            {
                FILL_ERR(cdate + "  " + datestr);
                date = 0;
            }
        }
        catch (Exception ex)
        {

            FILL_ERR(cdate + "  " + datestr + " " + ex.Message);
            date = 0;
        }
        return date;
    }

        public void prnt_1Dbar(string pco_Cd, string bar_val, string img_name)
        {
            int h, f, w, l = bar_val.Length; string font_name = "IDAutomationHC39M";
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            if (pco_Cd == "LIVN") { h = 70; f = 20; w = 60; }
            else if (pco_Cd == "KTPL" || pco_Cd == "MINV" || pco_Cd == "DLJM") { h = 80; f = 22; w = 40; }
            else if (pco_Cd == "MVIN") { h = 80; f = 22; w = 60; l = 5; }
            else if (pco_Cd == "MEGA") { font_name = "Code128bWin"; h = 80; f = 20; w = 20; }
            else { h = 80; f = 16; w = 40; }
            using (Bitmap bitMap = new Bitmap(l * w, h))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
                {
                    //Free 3 of 9
                    //IDAutomationHC39M
                    //Code128bWin
                    //Code128bWinLarge
                    //BarcodeFont
                    Font oFont = new Font(font_name, f);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString("*" + bar_val.Replace(" ", "=") + "*", oFont, blackBrush, point);
                }
                bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
            }
        }
        public void prnt_Code128bar(string pco_Cd, string bar_val, string img_name)
        {
            //int h = 0; int f = 0; int w = 0; int l = 0;
            //f = 45; w = 60; h = 90;
            //l = bar_val.Trim().Length; string font_name = "IDAutomationC128L";
            //bar_val = BarCode.BarcodeConverter128.StringToBarcode(bar_val.Trim());
            //bar_val = bar_val.Replace(" ", "Â");
            //if (pco_Cd == "MEGA" || pco_Cd == "MANU") { l = 15; }
            //else if (pco_Cd == "YTEC") { f = 36; l = l + 2; if (l == 2) { l = 4; } }
            //else { l = l + 2; f = 36; }
            //using (Bitmap bitMap = new Bitmap(l * w, h))
            //{
            //    using (Graphics graphics = Graphics.FromImage(bitMap))
            //    {
            //        Font oFont = new Font(font_name, f);
            //        PointF point = new PointF(2f, 2f);
            //        SolidBrush blackBrush = new SolidBrush(Color.Black);
            //        SolidBrush whiteBrush = new SolidBrush(Color.White);
            //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
            //        graphics.DrawString(bar_val.Trim(), oFont, blackBrush, point);
            //    }
            //    bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
            //}
        }

        public void prnt_2Dbar(string pco_Cd, string bar_val, string img_name)
        {
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            using (Bitmap bitMap = new Bitmap(bar_val.Length * 40, 80))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
                {
                    Font oFont = new Font("IDAutomation2D", 36);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString("*" + bar_val + "*", oFont, blackBrush, point);
                }
                bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
            }


            //var writer = new BarcodeWriter
            //{
            //    Format = BarcodeFormat.PDF_417,
            //    Options = new EncodingOptions { Width = 200, Height = 50 } //optional
            //};
            //var imgBitmap = writer.Write(data);
            //using (var stream = new MemoryStream())
            //{
            //    imgBitmap.Save(stream, ImageFormat.Png);             
            //}
        }

        public void prnt_2Dbar32bit(string pco_Cd, string bar_val, string img_name)
        {
            string barCodeFileName = "";
            try
            {
                System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                string myExeFile = HttpContext.Current.Server.MapPath("~\\tej-base\\myFiles\\Pdfenc.exe");
                string myBarCodeFolderPath = HttpContext.Current.Server.MapPath("~\\tej-base\\BarCode");
                string fileName = "c:\\tej_erp\\np" + "\\" + DateTime.Now.ToString("ddhhmmss") + ".txt";

                StreamWriter streamwriter = new StreamWriter(fileName);
                streamwriter.Write(bar_val);
                streamwriter.Flush();
                streamwriter.Dispose();
                streamwriter.Close();
                //if (!img_name.ToUpper().Contains("BMP")) img_name = img_name + ".bmp";

                barCodeFileName = myBarCodeFolderPath + "\\" + img_name;
                barCodeFileName = img_name;

                process1.StartInfo.FileName = myExeFile;
                process1.StartInfo.Arguments = "-BMP -M7:1 -c12 -a1:2 -x2 -o " + barCodeFileName + " " + fileName;
                FILL_ERR("-BMP -M7:1 -c12 -a1:2 -x2 -o " + barCodeFileName + " " + fileName);
                process1.Start();
                process1.WaitForExit();
                process1.Close();

                if (File.Exists(fileName)) File.Delete(fileName);
            }
            catch (Exception ex) { FILL_ERR(ex.Message); }
        }
        public void prnt_2DbarZXing(string pco_Cd, string bar_val, string img_name) { }
        //{
        //    int w = 0;
        //    w = bar_val.Length * 50;
        //    if (w > 200) w = 300;
        //    if (w < 60) w = 100;
        //    BarcodeWriter barcodeWriter = new BarcodeWriter()
        //    {
        //        Format = BarcodeFormat.PDF_417,
        //        Options = new PDF417EncodingOptions
        //{
        //    Height = 80,
        //    Width = w,
        //    Margin = 10
        //}
        //    };
        //    Bitmap bitMap = barcodeWriter.Write(bar_val);
        //    bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
        //}

        public void prnt_2DbarAll(string pco_Cd, string bar_val, string img_name)
        {
            string barCodeFileName = "";
            try
            {
                System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                string myExeFile = HttpContext.Current.Server.MapPath("~\\tej-base\\myFiles\\Windows2dBarCode.exe");
                string myBarCodeFolderPath = HttpContext.Current.Server.MapPath("~\\tej-base\\BarCode");
                string fileName = "c:\\tej_erp\\np" + "\\" + DateTime.Now.ToString("ddhhmmss") + ".txt";
                if (File.Exists(fileName)) File.Delete(fileName);

                StreamWriter streamwriter = new StreamWriter(fileName);
                streamwriter.Write(bar_val);
                streamwriter.Flush();
                streamwriter.Dispose();
                streamwriter.Close();

                barCodeFileName = myBarCodeFolderPath + "\\" + img_name;
                barCodeFileName = img_name;

                process1.StartInfo.FileName = myExeFile;
                process1.StartInfo.Arguments = fileName + " " + barCodeFileName;
                process1.Start();
                process1.WaitForExit();
                process1.Close();

                if (File.Exists(fileName)) File.Delete(fileName);
            }
            catch (Exception ex) { FILL_ERR(ex.Message); }
        }
        //public void prnt_2DbarZXing(string pco_Cd, string bar_val, string img_name) { }
        //{
        //    int w = 0;
        //    w = bar_val.Length * 50;
        //    if (w > 200) w = 300;
        //    if (w < 60) w = 100;
        //    BarcodeWriter barcodeWriter = new BarcodeWriter()
        //    {
        //        Format = BarcodeFormat.PDF_417,
        //        Options = new PDF417EncodingOptions
        //{
        //    Height = 80,
        //    Width = w,
        //    Margin = 10
        //}
        //    };
        //    Bitmap bitMap = barcodeWriter.Write(bar_val);
        //    bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
        //}

        public void prnt_1DbarZXing(string pco_Cd, string bar_val, string img_name) { }
        //{
        //    BarcodeWriter barcodeWriter = new BarcodeWriter()
        //    {
        //        Format = BarcodeFormat.CODE_128
        //    };
        //    Bitmap bitMap = barcodeWriter.Write(bar_val);
        //    bitMap.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), ImageFormat.Png);
        //}

        public void prnt_QRbar(string pco_Cd, string bar_val, string img_name)
        {
            int s = 5;
            if (pco_Cd == "YTEC" || pco_Cd == "MANU") s = 3;
            if (pco_Cd == "ADWA" || pco_Cd == "PPAP") s = 5;
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            encoder.QRCodeVersion = 0;
            encoder.QRCodeScale = s;
            Bitmap img = encoder.Encode(bar_val, System.Text.Encoding.UTF8);
            img.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), System.Drawing.Imaging.ImageFormat.Jpeg);

            //string level = "L";
            //int size = 2;

            //QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            //using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            //{
            //    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(bar_val, eccLevel))
            //    {
            //        using (QRCode qrCode = new QRCode(qrCodeData))
            //        {
            //            Bitmap img = qrCode.GetGraphic(20, Color.Black, Color.White, null, 0);
            //            img.Save(HttpContext.Current.Server.MapPath(@"..\tej-base\BarCode\" + img_name + ""), System.Drawing.Imaging.ImageFormat.Jpeg);
            //        }
            //    }
            //}            
        }
        public DataTable addBarCode(DataTable mainDataTable, string valueforBarcodeFromTable, bool QR)
        {
            mainDataTable.Columns.Add("BarCodeDesc", typeof(string));
            mainDataTable.Columns.Add("BarCode", typeof(System.Byte[]));
            string fpath = "";
            string bValue = "";
            foreach (DataRow dr in mainDataTable.Rows)
            {
                bValue = dr[valueforBarcodeFromTable].ToString().Trim();
                fpath = HttpContext.Current.Server.MapPath(@"~\tej-base\BarCode\" + bValue.Replace("*", "").Replace("/", "") + ".png");
                del_file(fpath);
                if (QR == true) prnt_QRbar("", bValue, bValue.Replace("*", "").Replace("/", "") + ".png");

                FileStream FilStr = new FileStream(fpath, FileMode.Open);
                BinaryReader BinRed = new BinaryReader(FilStr);

                dr["BarCodeDesc"] = bValue;
                dr["BarCode"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                FilStr.Close();
                BinRed.Close();
            }

            return mainDataTable;
        }
        public void FILL_Log(string msg)
        {

            string ppath = HttpRuntime.AppDomainAppPath + "\\logFile.txt";
            try
            {
                if (File.Exists(ppath))
                {
                    StreamWriter w = File.AppendText(ppath);
                    w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                    w.WriteLine("=====================================================================");
                    w.Flush();
                    w.Close();
                }
                else
                {
                    StreamWriter w = new StreamWriter(ppath, true);
                    w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                    w.WriteLine("=====================================================================");
                    w.Flush();
                    w.Close();
                }
            }
            catch { }
        }

        public void FILL_ERR(string msg)
        {

            string ppath = HttpRuntime.AppDomainAppPath + "\\err.txt";
            try
            {
                if (File.Exists(ppath))
                {
                    StreamWriter w = File.AppendText(ppath);
                    w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                    w.WriteLine("=====================================================================");
                    w.Flush();
                    w.Close();
                }
                else
                {
                    StreamWriter w = new StreamWriter(ppath, true);
                    w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                    w.WriteLine("=====================================================================");
                    w.Flush();
                    w.Close();
                }
            }
            catch { }
        }
        public string fill_handston(DataTable dt, string form_type)
        {
            string data = GetJson(dt);
            string header = header_name(dt);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(@"$(document).ready(function () { ");
            sb.Append(@"var data = " + data.Trim() + ";");

            sb.Append(@"$('#datadiv').handsontable({");
            sb.Append(@"data: data,");
            sb.Append(@"columnSorting: true,");
            sb.Append(@"readOnly: true,");
            sb.Append(@"manualColumnResize: true,");
            sb.Append(@"manualRowResize: true,");
            sb.Append(@"colHeaders: " + header.Trim() + "");

            if (form_type == "SEEK")
            {
                //sb.Append(@",fixedColumnsLeft: 1");
                sb.Append(@",afterSelectionEnd: function (i) {");
                sb.Append(@"Instance = $('#datadiv').handsontable('getInstance');");
                sb.Append(@"data = Instance.getData();");
                sb.Append(@"selected = data[i];");
                sb.Append(@"value = selected[0] + '^' + selected[1] + '^' + selected[2];");
                sb.Append(@"$('#hdata').val(value);");
                sb.Append(@"$('#btnhide').click();");
                sb.Append(@"}");
            }
            sb.Append(@"});");
            sb.Append(@"});");

            sb.Append(@"</script>");
            dt.Dispose();
            return sb.ToString();
        }
        public string fill_handston(DataTable dt, string form_type, string divID)
        {
            string data = GetJson(dt);
            string header = header_name(dt);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(@"$(document).ready(function () { ");
            sb.Append(@"var data = " + data.Trim() + ";");

            sb.Append(@"$('#" + divID + "').handsontable({");
            sb.Append(@"data: data,");
            sb.Append(@"columnSorting: true,");
            sb.Append(@"readOnly: true,");
            sb.Append(@"manualColumnResize: true,");
            sb.Append(@"manualRowResize: true,");
            sb.Append(@"colHeaders: " + header.Trim() + "");

            if (form_type == "SEEK")
            {
                //sb.Append(@",fixedColumnsLeft: 1");
                sb.Append(@",afterSelectionEnd: function (i) {");
                sb.Append(@"Instance = $('#datadiv').handsontable('getInstance');");
                sb.Append(@"data = Instance.getData();");
                sb.Append(@"selected = data[i];");
                sb.Append(@"value = selected[0] + '^' + selected[1] + '^' + selected[2];");
                sb.Append(@"$('#hdata').val(value);");
                sb.Append(@"$('#btnhide').click();");
                sb.Append(@"}");
            }
            sb.Append(@"});");
            sb.Append(@"});");

            sb.Append(@"</script>");
            dt.Dispose();
            return sb.ToString();
        }
        public string fill_handstonDrill(DataTable dt, string form_type)
        {
            string data = GetJson(dt);
            string header = header_name(dt);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(@"$(document).ready(function () { ");
            sb.Append(@"var data = " + data.Trim() + ";");

            sb.Append(@"$('#datadiv').handsontable({");
            sb.Append(@"data: data,");
            sb.Append(@"columnSorting: true,");
            sb.Append(@"readOnly: true,");
            sb.Append(@"manualColumnResize: true,");
            sb.Append(@"manualRowResize: true,");
            sb.Append(@"colHeaders: " + header.Trim() + "");

            sb.Append(@",fixedColumnsLeft: 1");
            sb.Append(@",afterSelectionEnd: function (i) {");
            sb.Append(@"Instance = $('#datadiv').handsontable('getInstance');");
            sb.Append(@"data = Instance.getData();");
            sb.Append(@"selected = data[i];");
            sb.Append(@"value = selected[0] + '^' + selected[1] + '^' + selected[2];");
            sb.Append(@"$('#hdata').val(value);");
            sb.Append(@"$('#btnhide').click();");
            sb.Append(@"}");

            sb.Append(@"});");
            sb.Append(@"});");

            sb.Append(@"</script>");
            dt.Dispose();
            return sb.ToString();
        }
        public string header_name(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            string colStr = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (colStr.Length > 0) colStr += ",";
                colStr += "['" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "']";
            }
            sb.Append("" + colStr + "");
            sb.Append("]");
            dt.Dispose();
            return sb.ToString();
        }
        public string GetJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                string rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0) rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                        rowDataStr += dr[dc].ToString().Replace("\r\n", "").Replace("\n", "").Replace("'", "`").Replace(@"\", "\\");
                    else rowDataStr += "'" + dr[dc].ToString().Replace("\r\n", "").Replace("\n", "").Replace("'", "`").Replace(@"\", "\\") + "'";
                }
                if (count > 0) sb.Append(",[" + rowDataStr + "]");
                else sb.Append("[" + rowDataStr + "]");
            }
            sb.Append("]"); dt.Dispose();
            return sb.ToString();
        }
        public void exp_to_excel(DataTable dt, string exp_typ, string ext, string file_name)
        {
            if (dt == null) return;

            HttpContext.Current.Session["EXP_DT"] = dt;
            send_cookie("exp_type", exp_typ);
            send_cookie("ext", ext);
            send_cookie("file_name", file_name);
            send_cookie("formID", "");
            send_cookie("header_y_n", "Y");

            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("window.open('../tej-base/dwnloadExcelFile.aspx?DTR=1','_blank')");
            HttpContext.Current.Response.Write("</script>");

            dt.Dispose();
        }
        public void exp_to_excel_sp(DataTable dt, string exp_typ, string ext, string file_name, string formID)
        {
            if (dt == null) return;

            HttpContext.Current.Session["EXP_DT"] = dt;
            send_cookie("exp_type", exp_typ);
            send_cookie("ext", ext);
            send_cookie("file_name", file_name);
            send_cookie("formID", formID);
            send_cookie("header_y_n", "Y");

            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("window.open('../tej-base/dwnloadExcelFile.aspx?DTR=2','_blank')");
            HttpContext.Current.Response.Write("</script>");

            dt.Dispose();
        }
        public void exp_to_excel(DataTable dt, string exp_typ, string ext, string file_name, string header_y_n)
        {
            if (dt == null) return;

            HttpContext.Current.Session["EXP_DT"] = dt;
            send_cookie("exp_type", exp_typ);
            send_cookie("ext", ext);
            send_cookie("file_name", file_name);
            send_cookie("formID", "");
            send_cookie("header_y_n", header_y_n);

            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("window.open('../tej-base/dwnloadExcelFile.aspx?DTR=3','_blank')");
            HttpContext.Current.Response.Write("</script>");

            dt.Dispose();
        }
        public void exp_to_excel_multi(DataTable dt, string filepath)
        {
            string tab = "";
            if (File.Exists(filepath)) File.Delete(filepath);
            StreamWriter sw = new StreamWriter(filepath, false);
            //First we will write the headers.
            foreach (DataColumn dc in dt.Columns)
            {
                sw.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(tab + dr[i].ToString());
                    tab = "\t";
                }

                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public void exp_to_pdf(DataTable dt, string file_name)
        {
            iTextSharp.text.Document pdfDoc;
            string firmName = "";
            if (file_name.Contains("_"))
            {
                firmName = fgenCO.chk_co(file_name.Split('_')[0].ToString());
            }

            GridView GridView2 = new GridView();
            GridView2.AllowPaging = false;
            GridView2.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C1F0F6");
            GridView2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#333");
            GridView2.DataSource = dt;
            GridView2.DataBind();

            if (GridView2.Rows.Count > 0)
            {
                for (int x = 0; x < GridView2.HeaderRow.Cells.Count; x++)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    GridView2.HeaderRow.Cells[x].Text = textInfo.ToTitleCase(GridView2.HeaderRow.Cells[x].Text.ToString());
                }
            }

            GridView2.HeaderRow.Style.Add("width", "10%");
            GridView2.HeaderRow.Style.Add("font-size", "9px");
            GridView2.Style.Add("text-decoration", "none");
            GridView2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            GridView2.Style.Add("font-size", "8px");

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView2.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + file_name + ".pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (dt.Columns.Count > 8) pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
            else pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
            iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();
            iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(firmName, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 22));
            para.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            pdfDoc.Add(para);
            para = new iTextSharp.text.Paragraph(" ");
            pdfDoc.Add(para);
            htmlparser.Parse(sr);
            pdfDoc.Close();
            HttpContext.Current.Response.Write(pdfDoc);
            HttpContext.Current.Response.End();
        }
        public void exp_to_word(DataTable dt, string file_name)
        {
            GridView GridView2 = new GridView();
            GridView2.DataSource = dt;
            GridView2.DataBind();
            GridView2.HeaderRow.Style.Add("width", "10%");
            GridView2.HeaderRow.Style.Add("font-size", "11px");
            GridView2.Style.Add("text-decoration", "none");
            GridView2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            GridView2.Style.Add("font-size", "10px");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + file_name + ".doc");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView2.AllowPaging = false;
            GridView2.RenderControl(hw);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            dt.Dispose();
        }
        public void ExportGridToExcel(GridView gridviewName, string fileName)
        {
            HttpContext.Current.Session["EXP_DT"] = null;
            HttpContext.Current.Session["GRIDVIEW"] = gridviewName;
            send_cookie("exp_type", "");
            send_cookie("ext", "");
            send_cookie("file_name", fileName);
            send_cookie("formID", "");
            send_cookie("header_y_n", "Y");

            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("window.open('../tej-base/dwnloadExcelFile.aspx?DTR=4','_blank')");
            HttpContext.Current.Response.Write("</script>");
        }
        public long sum_String(string Comma_seprated_string)
        {
            List<String> lway = new List<String>();
            lway = Comma_seprated_string.Split(',').ToList();
            List<long> myStringList = lway.Select(s => long.Parse(s)).ToList();
            long result = myStringList.Sum();
            return result;
        }
        /// <summary>
        ///  This Function Will help to Get System IP
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            string ip = "";
            try
            {
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] addr = ipEntry.AddressList;
                ip = addr[1].ToString();
                ip = ipEntry.HostName.ToString().Trim();
            }
            catch { }
            return ip;
        }
        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public void del_file(string Full_Path_to_del)
        {
            try
            {
                if (System.IO.File.Exists(Full_Path_to_del)) System.IO.File.Delete(Full_Path_to_del);
            }
            catch { }
        }
        public DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
            .Select(c => c.ColumnName)
            .ToArray();
            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
            .Select(r => r[pivotColumn.ColumnName].ToString())
            .Distinct().ToList()
            .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));
            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                pkColumnNames
                .Select(c => row[c])
                .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }
            return result;
        }
        private string GetConnection(string path)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }
        public string connStringexcel(string filesavepath)
        {
            string path = HttpRuntime.AppDomainAppPath + "\\foxtns.txt";
            string str = "";
            string Provider = "", Extended_Properties = "", hdr = "";
            string returnstring = "";
            try
            {
                if (File.Exists(path)) { }
                using (StreamReader sr_fgen = new StreamReader(path))
                {
                    str = sr_fgen.ReadToEnd().Trim();
                    if (str.Contains("\r")) str = str.Replace("\r", ",");
                    if (str.Contains("\n")) str = str.Replace("\n", ",");
                    str = str.Replace(",,", ",");

                    Provider = str.Split(',')[0];
                    Extended_Properties = str.Split(',')[1];
                    hdr = str.Split(',')[2];
                    sr_fgen.Dispose();
                }
                returnstring = "Provider=" + Provider + ";Data Source=" + filesavepath + ";Extended Properties=\"" + Extended_Properties + ";HDR=" + hdr + ";\"";
            }
            catch { }
            return returnstring;
        }
        public string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }
        //public string[] m_Array;
        //public string[] drillQuery
        //{
        //    get
        //    {
        //        if (m_Array == null)
        //        {
        //            m_Array = new string[20];
        //        }
        //        return m_Array;
        //    }
        //    set { m_Array = value; }
        //}
        public void clearDrill(string frmQstr)
        {
            for (int i = 0; i < 20; i++)
            {
                fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ" + i, "");
                fgenMV.Fn_Set_Mvar(frmQstr, "U_LVAL" + i, "");
            }
        }
        public void drillQuery(int drillLevel, string query, string frmQstr)
        {
            if (drillLevel == 0) clearDrill(frmQstr);
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ" + drillLevel, query);
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_2RALIGN" + drillLevel, "");
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_2RESIZE" + drillLevel, "");
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_WIDTHS" + drillLevel, "");

        }

        public string getDrillQuery(int drillLevel, string frmQstr)
        {
            string rVal = fgenMV.Fn_Get_Mvar(frmQstr, "M_DQ" + drillLevel);
            return rVal;
        }

        public void drillQuery(int drillLevel, string query, string frmQstr, string col_ralin, string col_setw, string col_size)
        {
            if (drillLevel == 0) clearDrill(frmQstr);
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ" + drillLevel, query);

            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_2RALIGN" + drillLevel, col_ralin);
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_2RESIZE" + drillLevel, col_setw);
            fgenMV.Fn_Set_Mvar(frmQstr, "M_DQ_COLS_WIDTHS" + drillLevel, col_size);
        }

        public void DataSetIntoDBF(string fileName, DataSet dataSet)
        {
            ArrayList list = new ArrayList();

            if (File.Exists(@"c:\tej_erp\" + fileName.ToUpper().Trim() + ".dbf")) File.Delete(@"c:\tej_erp\" + fileName.ToUpper().Trim() + ".dbf");
            string createSql = "create table " + fileName.ToUpper().Trim() + " (";

            foreach (DataColumn dc in dataSet.Tables[0].Columns)
            {
                string fieldName = dc.ColumnName;
                string type = dc.DataType.ToString();

                switch (type)
                {
                    case "System.String":
                        type = "char(80)";
                        break;
                    case "System.Boolean":
                        type = "char(6)";
                        break;
                    case "System.Int32":
                        type = "char(20)";
                        break;
                    case "System.Double":
                        type = "char(20)";
                        break;
                    case "System.DateTime":
                        type = "char(25)";
                        break;
                    case "System.Decimal":
                        type = "char(30)";
                        break;
                }

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";
                list.Add(fieldName);
            }
            createSql = createSql.Substring(0, createSql.Length - 1) + ")";
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\tej_erp;Extended Properties=dBASE IV;");
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = createSql;
            cmd.ExecuteNonQuery();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string insertSql = "insert into " + fileName.ToUpper().Trim() + " values(";
                for (int i = 0; i < list.Count; i++)
                {
                    insertSql = insertSql + "'" + (row[list[i].ToString()].ToString()).Replace("'", "''") + "',";
                }
                insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";
                cmd.CommandText = insertSql;
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        public string make_def_Date(string mtext, string mtext2)
        {
            string result = "-";
            try
            {
                if (mtext == null) mtext = mtext2;
                result = mtext.Trim();

                if (mtext == "") mtext = mtext2;
                result = mtext.Trim();

                if (mtext == "-") mtext = mtext2;
                result = mtext.Trim();
            }
            catch { result = "-"; }
            return result;
        }
        public string padlc(Int64 Number, int totalcharactes)
        {
            String result = "";
            Int64 temp = Number;
            for (int i = 1; i < totalcharactes; i++)
            {
                temp /= 10;
                if (temp == 0) result += "0";
            }
            result += Number.ToString();
            return result;
        }
        public string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }
        public Int32 make_int(string val)
        {
            Int32 result = 0;
            try
            {
                if (val == "Infinity" || val == "∞") val = "0";
                result = Convert.ToInt32(val);
            }
            catch { result = 0; }
            return result;
        }
        public double make_double(string val)
        {
            double result = 0;
            try
            {
                if (val == "Infinity" || val == "NaN" || val == "∞" || val == "-∞") val = "0";
                result = Convert.ToDouble(val);
            }
            catch { result = 0; }
            return result;
        }
        public decimal Make_decimal(string val)
        {
            decimal result = 0;
            try
            {
                if (val == "Infinity" || val == "NaN" || val == "∞" || val == "-∞") val = "0";
                result = Convert.ToDecimal(val);
            }
            catch { result = 0; }
            return result;
        }
        public string make_dash(string mtext)
        {
            string result = "-";
            try
            {
                if (mtext == "") mtext = "-";
                result = mtext.Trim();
            }
            catch { result = "-"; }
            return result;
        }
        public double make_double(string val, int digit)
        {
            double result = 0;
            try
            {
                if (val == "Infinity" || val == "∞") val = "0";
                result = Convert.ToDouble(val);
            }
            catch
            {
                result = 0;
            }
            result = Math.Round(result, digit);
            return result;
        }
        public double make_double(double val, int digit)
        {
            double result = 0;
            try
            {
                if (val.ToString() == "Infinity" || val.ToString() == "∞") val = 0;
                result = val;
            }
            catch
            {
                result = 0;
            }
            result = Math.Round(result, digit);
            return result;
        }
        public string make_double(double val, int digit, bool formatted)
        {
            double result = 0;
            try
            {
                if (val.ToString() == "Infinity" || val.ToString() == "∞") val = 0;
                result = val;
            }
            catch
            {
                result = 0;
            }
            string format = "#0.";
            for (int dig = 0; dig < digit; dig++)
            {
                format += "0";
            }
            return result.ToString(format);
        }
        public string Fn_txt_dt(string Date_for_Textbox)
        {
            string rdate = "";
            try
            {
                rdate = Convert.ToDateTime(Date_for_Textbox).ToString("yyyy-MM-dd");
            }
            catch { }
            return rdate;
        }
        public string captionSeek(DataTable dt_seek, string conditions, string col1)
        {
            string result = "0";
            valFound = "N";
            if (dt_seek.Rows.Count > 0)
            {
                DataRow[] rows = dt_seek.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
                if (rows.Length == 0) result = "0";
                else
                {
                    try
                    {
                        result = rows[0]["OBJ_CAPTION_REG"].ToString().Trim();
                        if (result == "-" || result == "" || result.Length <= 1)
                            result = rows[0][col1].ToString().Trim();
                    }
                    catch
                    {
                        result = rows[0][col1].ToString().Trim();
                    }
                    valFound = "Y";
                }
            }
            return result;
        }
        public string seek_iname_dt(DataTable dt_seek, string conditions, string col1)
        {
            string result = "0";
            valFound = "N";
            try
            {
                if (dt_seek == null) return result;
                if (dt_seek.Rows.Count > 0)
                {
                    if (conditions.Contains("COL_NO") && col1.Contains("OBJ_CAPTION"))
                    {
                        return captionSeek(dt_seek, conditions, col1);
                    }

                    DataRow[] rows = dt_seek.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
                    if (rows.Length == 0) result = "0";
                    else
                    {
                        result = rows[0][col1].ToString().Trim();
                        valFound = result;
                    }
                }
            }
            catch { result = "0"; }
            return result;
        }
        public string seek_iname_dt(DataTable dt_seek, string conditions, string col1, string orderSort)
        {
            string result = "0";
            valFound = "N";
            if (dt_seek.Rows.Count > 0)
            {
                DataRow[] rows = dt_seek.Select(conditions, orderSort, System.Data.DataViewRowState.CurrentRows);
                if (rows.Length == 0) result = "0";
                else
                {
                    result = rows[0][col1].ToString().Trim();
                    valFound = "Y";
                }
            }
            return result;
        }
        public string DataTableToJSSEEKArray(DataTable dt, string modeid)
        {
            StringBuilder sb = new StringBuilder();
            string rowDataStr = "";
            if (dt.Rows.Count > 0)
            {
                for (int count = 0; count < dt.Rows.Count; count++)
                {
                    DataRow dr = dt.Rows[count];
                    rowDataStr = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (rowDataStr.Length > 0)
                            rowDataStr += ",";
                        if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                            rowDataStr += dr[dc].ToString();
                        else
                            rowDataStr += "'" + dr[dc].ToString().Replace("'", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'";
                    }
                    sb.Append(",");
                    sb.Append("[" + rowDataStr + "]");
                }
            }
            return sb.ToString();
        }

        public string DataTableToJSArray(DataTable dt, string modeid)
        {
            StringBuilder sb = new StringBuilder();
            string rowDataStr = "";
            double icount = 0;
            int nflg = 0;
            if (dt.Rows.Count > 0)
            {
                for (int count = 0; count < dt.Rows.Count; count++)
                {
                    DataRow dr = dt.Rows[count];
                    rowDataStr = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (rowDataStr.Length > 0)
                            rowDataStr += ",";
                        if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                        {
                            if (count > 15)
                            {
                                icount = icount + Convert.ToDouble(dr[dc].ToString().Trim());
                                nflg = 1;
                            }
                            else
                                rowDataStr += dr[dc].ToString().Trim();
                        }
                        else
                        {
                            if (count > 15) { }
                            else
                            {
                                try
                                {
                                    Double temp = Convert.ToDouble(dr[dc]);
                                    rowDataStr += temp.ToString();
                                }
                                catch
                                {
                                    rowDataStr += "'" + dr[dc].ToString().Trim().Replace("'", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'";
                                }
                            }
                        }
                    }
                    if (icount > 0 || nflg == 1) { }
                    else
                    {
                        sb.Append(",");
                        sb.Append("[" + rowDataStr + "]");
                    }
                }
            }
            if (icount == 0 && nflg == 0) { }
            else
            {
                rowDataStr = "";
                rowDataStr += "'OTHERS'";
                rowDataStr += ",";
                rowDataStr += icount.ToString();

                sb.Append(",");
                sb.Append("[" + rowDataStr + "]");
            }
            return sb.ToString();
        }
        public string DataTableToJSArray(DataTable dt, int index, string modeid)
        {
            StringBuilder sb = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                string colStr = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (colStr.Length > 0)
                        colStr += ",";
                    if (dr[index].GetType() == typeof(Int32) || dr[index].GetType() == typeof(Double) || dr[index].GetType() == typeof(Decimal))
                        colStr += System.Math.Abs(Convert.ToDecimal(dr[index].ToString()));
                    else
                    {
                        if (modeid == "LBG2")
                            colStr += "'" + dr[index].ToString().Trim().Substring(5, 3) + "'";
                        else
                        {
                            try
                            {
                                Double temp = Convert.ToDouble(dr[index]);
                                colStr += temp.ToString();
                            }
                            catch
                            {
                                colStr += "'" + dr[index].ToString().Trim() + "'";
                            }
                        }
                    }
                }
                sb.Append("[" + colStr + "]");
            }
            return sb.ToString();
        }
        public void chk_email_info(string co_cd, string check_file)
        {
            string str, path;
            if (sender_id == "")
            {
                //if (co_cd.Substring(0, 1) == "A" || co_cd.Substring(0, 1) == "B" || co_cd.Substring(0, 1) == "C" || co_cd.Substring(0, 1) == "D" || co_cd.Substring(0, 1) == "E")
                //{
                //    sender_id = "erp1@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "F" || co_cd.Substring(0, 1) == "G" || co_cd.Substring(0, 1) == "H" || co_cd.Substring(0, 1) == "I" || co_cd.Substring(0, 1) == "J")
                //{
                //    sender_id = "erp2@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "K" || co_cd.Substring(0, 1) == "L" || co_cd.Substring(0, 1) == "M" || co_cd.Substring(0, 1) == "N" || co_cd.Substring(0, 1) == "O")
                //{
                //    sender_id = "erp3@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "P" || co_cd.Substring(0, 1) == "Q" || co_cd.Substring(0, 1) == "R" || co_cd.Substring(0, 1) == "S" || co_cd.Substring(0, 1) == "T")
                //{
                //    sender_id = "erp4@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "U" || co_cd.Substring(0, 1) == "V" || co_cd.Substring(0, 1) == "W" || co_cd.Substring(0, 1) == "X" || co_cd.Substring(0, 1) == "Y" || co_cd.Substring(0, 1) == "Z")
                //{
                //    sender_id = "erp4@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                xvip = "1";
                xport = "465";
                path = @"c:\TEJ_ERP\email_info.txt";
                if (check_file == "2")
                {
                    // Checking for Second file
                    path = @"c:\tej_ERP\email_info2.txt";
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                        else
                        {
                            sender_id = str.Split(',')[0].ToString().Trim();
                            pwd = str.Split(',')[1].ToString().Trim();
                            vsmtp = str.Split(',')[2].ToString().Trim();
                            xvip = str.Split(',')[3].ToString().Trim();
                            xport = str.Split(',')[4].ToString().Trim();
                            try
                            {
                                CCMID = str.Split('=')[1].ToString().Trim();
                            }
                            catch { CCMID = ""; }
                        }
                    }
                    else
                    {
                        StreamWriter tw = File.AppendText(path);
                        tw.WriteLine("Email From");
                        tw.WriteLine("Password");
                        tw.WriteLine("SMTP");
                        tw.WriteLine("SSL==> 1 if True, 0 if false");
                        tw.WriteLine("PORT");
                        //tw.WriteLine("CC=");
                        tw.Close();
                    }
                    ssl = Convert.ToInt32(xvip);
                    port = Convert.ToInt32(xport);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                        else
                        {
                            sender_id = str.Split(',')[0].ToString().Trim();
                            pwd = str.Split(',')[1].ToString().Trim();
                            vsmtp = str.Split(',')[2].ToString().Trim();
                            xvip = str.Split(',')[3].ToString().Trim();
                            xport = str.Split(',')[4].ToString().Trim();
                            try
                            {
                                CCMID = str.Split('=')[1].ToString().Trim();
                            }
                            catch { CCMID = ""; }
                        }
                    }
                    else
                    {
                        StreamWriter tw = File.AppendText(path);
                        tw.WriteLine("Email From");
                        tw.WriteLine("Password");
                        tw.WriteLine("SMTP");
                        tw.WriteLine("SSL==> 1 if True, 0 if false");
                        tw.WriteLine("PORT");
                        //tw.WriteLine("CC=");
                        tw.Close();
                    }
                    ssl = Convert.ToInt32(xvip);
                    port = Convert.ToInt32(xport);
                }
            }
        }
        public void chk_email_info(string qstr, string co_cd, string check_file)
        {
            string str, path;
            if (sender_id == "")
            {
                //if (co_cd.Substring(0, 1) == "A" || co_cd.Substring(0, 1) == "B" || co_cd.Substring(0, 1) == "C" || co_cd.Substring(0, 1) == "D" || co_cd.Substring(0, 1) == "E")
                //{
                //    sender_id = "erp1@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "F" || co_cd.Substring(0, 1) == "G" || co_cd.Substring(0, 1) == "H" || co_cd.Substring(0, 1) == "I" || co_cd.Substring(0, 1) == "J")
                //{
                //    sender_id = "erp2@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "K" || co_cd.Substring(0, 1) == "L" || co_cd.Substring(0, 1) == "M" || co_cd.Substring(0, 1) == "N" || co_cd.Substring(0, 1) == "O")
                //{
                //    sender_id = "erp3@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "P" || co_cd.Substring(0, 1) == "Q" || co_cd.Substring(0, 1) == "R" || co_cd.Substring(0, 1) == "S" || co_cd.Substring(0, 1) == "T")
                //{
                //    sender_id = "erp4@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                //if (co_cd.Substring(0, 1) == "U" || co_cd.Substring(0, 1) == "V" || co_cd.Substring(0, 1) == "W" || co_cd.Substring(0, 1) == "X" || co_cd.Substring(0, 1) == "Y" || co_cd.Substring(0, 1) == "Z")
                //{
                //    sender_id = "erp4@tejaxo.co.in";
                //    pwd = "erp_2014";
                //    vsmtp = "smtp.bizmail.yahoo.com";
                //}
                xvip = "1";
                xport = "465";
                path = @"c:\tej_ERP\email_info.txt";
                if (check_file == "2")
                {
                    // Checking for Second file
                    path = @"c:\tej_ERP\email_info2.txt";
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                        else
                        {
                            sender_id = str.Split(',')[0].ToString().Trim();
                            pwd = str.Split(',')[1].ToString().Trim();
                            vsmtp = str.Split(',')[2].ToString().Trim();
                            xvip = str.Split(',')[3].ToString().Trim();
                            xport = str.Split(',')[4].ToString().Trim();
                            try
                            {
                                CCMID = str.Split('=')[1].ToString().Trim();
                            }
                            catch { CCMID = ""; }
                        }
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        StreamWriter tw = File.AppendText(path);
                        tw.WriteLine("Email From");
                        tw.WriteLine("Password");
                        tw.WriteLine("SMTP");
                        tw.WriteLine("SSL==> 1 if True, 0 if false");
                        tw.WriteLine("PORT");
                        //tw.WriteLine("CC=");
                        tw.Close();
                    }
                    ssl = Convert.ToInt32(xvip);
                    port = Convert.ToInt32(xport);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                        else
                        {
                            sender_id = str.Split(',')[0].ToString().Trim();
                            pwd = str.Split(',')[1].ToString().Trim();
                            vsmtp = str.Split(',')[2].ToString().Trim();
                            xvip = str.Split(',')[3].ToString().Trim();
                            xport = str.Split(',')[4].ToString().Trim();
                            try
                            {
                                CCMID = str.Split('=')[1].ToString().Trim();
                            }
                            catch { CCMID = ""; }
                        }
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        StreamWriter tw = File.AppendText(path);
                        tw.WriteLine("Email From");
                        tw.WriteLine("Password");
                        tw.WriteLine("SMTP");
                        tw.WriteLine("SSL==> 1 if True, 0 if false");
                        tw.WriteLine("PORT");
                        tw.WriteLine("CC=");
                        tw.Close();
                    }
                    ssl = Convert.ToInt32(xvip);
                    port = Convert.ToInt32(xport);
                }

                DataTable dtType = new DataTable();
                fgenDB fgen = new fgenDB();
                string mbr = fgenMV.Fn_Get_Mvar(qstr, "U_MBR");
                dtType = fgen.getdata(qstr, co_cd, "SELECT MAIL_FLD1,MAIL_FLD2,MAIL_FLD3,MAIL_FLD4,MAIL_FLD5,MAIL_FLD6 FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "'");
                if (dtType.Rows.Count > 0)
                {
                    if (dtType.Rows[0]["MAIL_FLD1"].ToString().Trim().Length > 1)
                    {
                        sender_id = dtType.Rows[0]["MAIL_FLD1"].ToString().Trim();
                        pwd = dtType.Rows[0]["MAIL_FLD2"].ToString().Trim();
                        vsmtp = dtType.Rows[0]["MAIL_FLD3"].ToString().Trim();
                        xvip = dtType.Rows[0]["MAIL_FLD5"].ToString().Trim();

                        if (xvip == "Y") xvip = "1";
                        else xvip = "0";

                        xport = dtType.Rows[0]["MAIL_FLD4"].ToString().Trim();

                        ssl = Convert.ToInt32(xvip);
                        port = Convert.ToInt32(xport);
                    }
                }

            }
        }
        public string send_mail(string co_cd, string name, string to, string Cc, string Bcc, string subj, string body)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();

                chk_email_info(co_cd, "1");
                mail.From = new MailAddress(name + "<" + sender_id + ">");
                //mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }
                merror = "1";

                //SendEmailInBackgroundThread(mail);

                smtp = new SmtpClient();
                try
                {
                    //Task.Factory.StartNew(() =>{
                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                    FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                    smtp.UseDefaultCredentials = false;
                    //***********************************
                    smtp.Host = vsmtp;
                    smtp.Port = port;
                    if (ssl == 1) smtp.EnableSsl = true;
                    else smtp.EnableSsl = false;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                    smtp.Credentials = SMTPUserInfo;

                    smtp.Send(mail);

                    FILL_Log("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
                    //});
                }
                catch (SmtpException ex)
                {
                    FILL_ERR(ex.Message);
                }
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        public string send_mail(string co_cd, string name, string to, string Cc, string Bcc, string subj, string body, string attachmentPath)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();
                if (co_cd == "SDM") chk_email_info(co_cd, "2");
                else chk_email_info(co_cd, "1");
                //mail.From = new MailAddress(name + "<" + sender_id + ">");
                mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                if (attachmentPath.Length > 1)
                {
                    Attachment atch = new Attachment(attachmentPath);
                    mail.Attachments.Add(atch);
                }

                merror = "1";

                smtp = new SmtpClient();
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mail);

                //SendEmailInBackgroundThread(mail);

                FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        public string send_mail(string co_cd, string name, string to, string Cc, string Bcc, string subj, string body, Attachment attachmentPath)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();

                chk_email_info(co_cd, "1");
                //mail.From = new MailAddress(name + "<" + sender_id + ">");
                mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                if (attachmentPath != null)
                {
                    mail.Attachments.Add(attachmentPath);
                }

                merror = "1";

                smtp = new SmtpClient();
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mail);

                //SendEmailInBackgroundThread(mail);

                FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        public string send_mail(string qstr, string co_cd, string name, string to, string Cc, string Bcc, string subj, string body, Attachment attachmentPath)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();

                if (sender_id == "")
                {
                    if (co_cd == "SDM") chk_email_info(qstr, co_cd, "2");
                    else chk_email_info(qstr, co_cd, "1");
                }
                mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                if (attachmentPath != null)
                {
                    mail.Attachments.Add(attachmentPath);
                }

                merror = "1";

                smtp = new SmtpClient();
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mail);

                //SendEmailInBackgroundThread(mail);

                FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        public string send_mail(string qstr, string co_cd, string name, string to, string Cc, string Bcc, string subj, string body, Attachment attachmentPath, string emailinfo)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();

                if (sender_id == "")
                {
                    if (emailinfo == "2") chk_email_info(qstr, co_cd, "2");
                    else chk_email_info(qstr, co_cd, "1");
                }
                mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                Cc = CCMID + (Cc.Trim().Length > 2 ? "," + Cc : "");
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            if (mul_id.Length > 0)
                                mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                if (attachmentPath != null)
                {
                    mail.Attachments.Add(attachmentPath);
                }

                merror = "1";

                smtp = new SmtpClient();
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mail);

                //SendEmailInBackgroundThread(mail);

                FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        public string send_mail(string qstr, string co_cd, string name, string to, string Cc, string Bcc, string subj, string body, string multiAttachmentPath, string emailinfo)
        {
            string merror = ""; string[] mul;
            try
            {
                mail = new MailMessage();

                if (sender_id == "")
                {
                    if (emailinfo == "2") chk_email_info(qstr, co_cd, "2");
                    else chk_email_info(qstr, co_cd, "1");
                }
                mail.From = new MailAddress(sender_id);

                mail.Subject = subj;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (to.Contains(",") || to.Contains(";"))
                {
                    to = to.Replace(";", ",");
                    mul = to.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.To.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    to = to.Replace(";", ""); to = to.Replace(",", "");
                    mail.To.Add(new MailAddress(to));
                }
                Cc = CCMID + (Cc.Trim().Length > 2 ? "," + Cc : "");
                if (Cc.Trim().Length > 2)
                {
                    if (Cc.Contains(",") || Cc.Contains(";"))
                    {
                        Cc = Cc.Replace(";", ",");
                        mul = Cc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            if (mul_id.Length > 0)
                                mail.CC.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                        mail.CC.Add(new MailAddress(Cc));
                    }
                }
                if (Bcc.Trim().Length > 2)
                {
                    if (Bcc.Contains(",") || Bcc.Contains(";"))
                    {
                        Bcc = Bcc.Replace(";", ",");
                        mul = Bcc.Split(',');
                        foreach (string mul_id in mul)
                        {
                            mail.Bcc.Add(new MailAddress(mul_id));
                        }
                    }
                    else
                    {
                        Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                        mail.Bcc.Add(new MailAddress(Bcc));
                    }
                }

                Attachment atch = null;
                if (multiAttachmentPath != "")
                {
                    foreach (string singleAtchP in multiAttachmentPath.Split(','))
                    {
                        atch = new Attachment(singleAtchP);
                        mail.Attachments.Add(atch);
                    }
                }

                merror = "1";

                smtp = new SmtpClient();
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
                FILL_ERR("SenderID# " + sender_id + "  " + pwd);
                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mail);

                //SendEmailInBackgroundThread(mail);

                FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            }
            catch (Exception ex)
            {
                merror = "0";
                FILL_ERR(co_cd + " " + ex.Message);
                FILL_ERR("Rcv ID: " + to);
                FILL_ERR("Sender ID: " + sender_id);
            }
            return merror;
        }
        void SendEmail(Object mailVal)
        {
            MailMessage mailMessage = (MailMessage)mailVal;
            smtp = new SmtpClient();
            try
            {
                Task.Factory.StartNew(() =>
    {
        {
            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);
            FILL_ERR("SenderID# " + sender_id + "  " + pwd);
            smtp.UseDefaultCredentials = false;
            //***********************************
            smtp.Host = vsmtp;
            smtp.Port = port;
            if (ssl == 1) smtp.EnableSsl = true;
            else smtp.EnableSsl = false;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
            smtp.Credentials = SMTPUserInfo;
        }

        smtp.Send(mailMessage);
    });
            }
            catch (SmtpException ex)
            {
                FILL_ERR(ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
                smtp.Dispose();
            }
        }
        void SendEmailInBackgroundThread(MailMessage mailMessage)
        {
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            bgThread.IsBackground = true;
            bgThread.Start(mailMessage);
        }
        public string send_sms(string qstr, string comp, string mob, string mymsg, string entby)
        {
            string result = "";
            try
            {
                //string muname = "tejaxo";
                // string mpwd = "tejaxo123";
                //string smsSenderID = "tejaxo";
                //string muname = "U1668";
                //string mpwd = "pAU8FO4W";
                //string smsSenderID = "tejaxo";
                //string secretcode = "sRJyPmcaN4qJ8F3dHxTV";
                //HttpUtility.UrlEncode("SMS");
                //string API = "";

                //if (comp == "BUPL")
                //{
                //    muname = "Beri Udyog pvt.Ltd";
                //    mpwd = "123456";
                //}
                //if (comp == "JSGI")
                //{
                //    muname = "jsginnotech";
                //    mpwd = "sms@2018";
                //}
                //if (comp == "DISP")
                //{
                //    muname = "disposafe01";
                //    mpwd = "Disposafe@01";
                //    smsSenderID = "DISPOS";
                //    if (!mymsg.Contains("Rgds : " + comp))
                //        mymsg = mymsg + ", Rgds : " + "Disposafe";
                //}
                //if (!mymsg.Contains("Rgds : " + comp) && comp != "DISP")
                //    mymsg = mymsg + ", Rgds : " + comp;

                //DataTable dtType = new DataTable();
                //fgenDB fgen = new fgenDB();
                //string mbr = fgenMV.Fn_Get_Mvar(qstr, "U_MBR");
                ////dtType = fgen.getdata(qstr, comp, "SELECT SMS_FLD1,SMS_FLD2 FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "'");
                ////if (dtType.Rows.Count > 0)
                ////{
                ////    if (dtType.Rows[0]["SMS_FLD1"].ToString().Trim().Length > 1)
                ////    {
                ////        muname = dtType.Rows[0]["SMS_FLD1"].ToString().Trim();
                ////        mpwd = dtType.Rows[0]["SMS_FLD2"].ToString().Trim();
                ////    }
                ////}
                //// old api http://manage.staticking.net/index.php/smsapi/httpapi/ changed mg 12.6.21
                //API = "http://sms.staticking.com/index.php/smsapi/httpapi/?uname=" + muname + "&password=" + mpwd + "&sender=" + smsSenderID + "&receiver=" + mob + "&route=TA&msgtype=1&sms=" + mymsg + "";
                ////API = "http://sms.staticking.com/index.php/smsapi/httpapi/?secret=" + secretcode + "&sender=" + smsSenderID + "&tempid=" + templateid + "&receiver= " + mob + "&route=TA&msgtype=1&sms= " + mymsg + "";

                //if (comp == "KUNS")
                //    API = "http://www.smsnmedia.com/api/push?user=KUNSTOCOM&pwd=123456&route=Transactional&sender=KUNSTO&mobileno=" + mob + "&text=" + mymsg + " ";
                //if (comp == "JSGI")
                //    API = "http://manage.staticking.net/index.php/smsapi/httpapi/?uname=" + muname + "&password=" + mpwd + "&sender=FinJSG&receiver=" + mob + "&route=TA&msgtype=1&sms=" + mymsg + "";
                //if (comp == "BUPL")
                //    API = "http://trans.smsfresh.co/api/sendmsg.php?user=" + muname + "&pass=" + mpwd + "&sender=tejaxo&phone=" + mob + "&text=" + mymsg + "&priority=ndnd&stype=normal";

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API);
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //StreamReader reader = new StreamReader(response.GetResponseStream());
                //HttpContext.Current.Response.Write(reader.ReadToEnd());
                //result = "SMS has been sent successfully to " + mob + " on date " + DateTime.Now.ToShortDateString() + " and time " + DateTime.Now.ToShortTimeString();
            }
            catch (Exception ex)
            {
                //result = "Error : SMS could not be sent due to some technical reason. Please check up with SMS service provider.(" + ex.Message + ")";
            }
            return result;
        }
        public string send_smsAUTOAPI(string apistring)
        {
            string result = "";
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apistring);
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    StreamReader reader = new StreamReader(response.GetResponseStream());
            //    HttpContext.Current.Response.Write(reader.ReadToEnd());
            //    result = "SMS has been sent successfully on date " + DateTime.Now.ToShortDateString() + " and time " + DateTime.Now.ToShortTimeString();
            //}
            //catch (Exception ex)
            //{
            //    result = "Error : SMS could not be sent due to some technical reason. Please check up with SMS service provider.(" + ex.Message + ")";
            //}
            return result;
        }
        public bool isValidMailID(string emailaddress)
        {

            if (emailaddress.ToString().Length < 2) return false;
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (System.FormatException)
            {
                return false;
            }
        }


        public bool CheckIsDate(String givendate)
        {
            try
            {
                DateTime dt = DateTime.Parse(givendate);
                return true;
            }
            catch
            {
                return false;
            }
        }



        public string getNumericOnly(string valueToConvert)
        {
            string output = System.Text.RegularExpressions.Regex.Replace(valueToConvert, "[^0-9]+", string.Empty);
            return output;
        }
        /// <summary>
        /// Original for Recipients, Duplicate for Transporter, Triplicate for Supplier, Gate Pass, Extra Copy
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="repCount"></param>
        /// <returns></returns>
        public DataTable mTitle(DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Recipient             ";
                    if (j == 1) mtitle = "Duplicate for Transporter";
                    if (j == 2) mtitle = "Triplicate for Supplier";

                    if (j == 3) mtitle = "Gate Pass";
                    if (j == 4) mtitle = "Extra Copy";

                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }
        /// <summary>
        /// Original for Recipients, Duplicate for Transporter, Triplicate for Supplier, Gate Pass, Extra Copy
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="repCount"></param>
        /// <returns></returns>
        public DataTable mTitle(string cocd, DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Recipient             ";
                    if (cocd == "MLAB" && repCount < 2)
                    {
                        if (j == 0) mtitle = "Extra Copy             ";
                    }
                    if (j == 1) mtitle = "Duplicate for Transporter";
                    if (cocd == "SAIP" || cocd == "JLAP" || cocd == "PANO")
                    {
                        if (j == 2) mtitle = "Triplicate for Assesse";
                    }
                    else
                    {
                        if (j == 2) mtitle = "Triplicate for Supplier";
                    }
                    if (cocd == "PPAP" || cocd == "VICT" || cocd == "ADWA" || cocd == "MULT" || cocd == "SFLG" || cocd == "ATOP" || cocd == "PRIN")
                    {
                        if (j == 3) mtitle = "Extra Copy";
                    }
                    else if (cocd == "SAIP" || cocd == "JLAP")
                    {
                        if (j == 3) mtitle = "Extra Copy";
                        if (j == 4) mtitle = "Office Copy";
                    }
                    else if (cocd == "SAIL" || cocd == "MINV" || cocd == "DLJM" || cocd == "PGEL" || cocd == "OTTO" || cocd == "JCPS" || cocd == "KPPL" || cocd == "VELV")
                    {
                        if (j == 3) mtitle = "Extra Copy";
                        if (j == 4) mtitle = "Extra Copy";
                    }
                    else if (cocd == "VIGP")
                    {
                        if (j == 2) mtitle = "Triplicate for Supplier";
                        if (j == 3) mtitle = "Gate Pass";
                        if (j == 4) mtitle = "Extra Copy";
                    }
                    else if (cocd == "PANO")
                    {
                        if (j == 3) mtitle = "IV TH COPY : (NOT FOR CENVAT)";
                        if (j == 4) mtitle = "VTH : EXTRA COPY";
                    }
                    else if (cocd == "ADMC")
                    {
                        if (j == 3) mtitle = "Gate Copy";
                        if (j == 4) mtitle = "Gate Copy";
                    }
                    else
                    {
                        if (j == 3) mtitle = "Gate Pass";
                        if (j == 4) mtitle = "Extra Copy";
                    }
                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }
        /// <summary>
        /// Original for Recipients, Duplicate for Copy, Extra Copy
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="repCount"></param>
        /// <returns></returns>
        public DataTable mTitle2(DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Recipient             ";
                    if (j == 1) mtitle = "Duplicate Copy";
                    if (j == 2) mtitle = "Extra Copy";

                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }
        /// <summary>
        /// Original for Recipients, Duplicate for Copy, Extra Copy
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="repCount"></param>
        /// <returns></returns>
        public DataTable mTitle3(DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Consignee             ";
                    if (j == 1) mtitle = "Duplicate for Transport";
                    if (j == 2) mtitle = "Triplicate for Consignor";
                    if (j == 3) mtitle = "Gate Pass";
                    if (j == 4) mtitle = "Extra Copy";

                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }

        public DataTable mTitle4(DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Recipient             ";
                    if (j == 1) mtitle = "Duplicate for Transporter";
                    if (j == 2) mtitle = "Triplicate for Supplier";
                    if (j == 3) mtitle = "Extra Copy";

                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }

        public DataTable mTitle_41(DataTable dataTable, int repCount)
        {
            string mtitle = "";
            DataTable dtN = new DataTable();
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
                if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
            }
            dtN = dataTable.Clone();
            for (int j = 0; j < repCount; j++)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (j == 0) mtitle = "Original for Recepient               ";
                    if (j == 1) mtitle = "Duplicate for Supplier";
                    if (j == 2) mtitle = "Extra Copy";
                    if (j == 3) mtitle = "Gate Pass";
                    if (j == 4) mtitle = "Extra Copy";

                    dr["mTITLE"] = mtitle;
                    dr["MTITLESRNO"] = j;
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dtN.ImportRow(dataTable.Rows[i]);
                }
            }
            dtN.TableName = dataTable.TableName.ToString();
            return dtN;
        }


        public bool ieBrowser()
        {
            if (HttpContext.Current.Request.Browser.Type.ToUpper().Contains("IE")) return true;
            return false;
        }
        public void dPrint(string comp_code, string frm_mbr, string userID, string formID, string CDT1, string fstr)
        {
            string pageurl = "../tej-base/dprint.aspx?STR=ERP@" + DateTime.Now.ToString("dd") + "@" + comp_code + "@" + CDT1.Substring(6, 4) + frm_mbr + "@" + userID + "@BVAL@" + formID + "@" + fstr + "";
            HttpContext.Current.Response.Write("<script>window.open('" + pageurl + "');</script>");
        }
        public void fin_engg_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/engg-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_gate_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/gate-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_purc_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/purc-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_invn_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/invn-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_qa_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/qa-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_prod_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prod-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_prodpm_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prodpm-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_ppc_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ppc-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_smktg_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/smktg-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_sales_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/sale-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_emktg_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/emktg-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_esales_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/esale-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_acct_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/acct-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_oth_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-oth-reps/sale-oth.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_prodpp_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prodpp-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_prodrx_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prodrx-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_prodr_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/prodrx-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_pay_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/pay-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_hrm_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/hrm-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_cust_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-cust-reps/cust-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_prodcast_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-prodcast-reps/prodcast-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_maint_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-maint-reps/maint-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_pmaint_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/maint-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_supp_port(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_sport_reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public void fin_cust_port(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om-cport-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }
        public void fin_crm_reps(string Qr_Str)
        {
            string pageurl = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/crm-reps.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + pageurl + "?STR=" + Qr_Str + "','95%','95%','');", true);
        }

        public string GetXMLTag(String xmlval)
        {
            string strval = "";
            try
            {
                string xmlFilePath = (@"C:\ipinfo.xml");
                docxml.Load(xmlFilePath);
                getval = docxml.GetElementsByTagName(xmlval);
                strval = getval[0].InnerText.Trim();
            }
            catch (Exception ex)
            {
                FILL_ERR(ex.Message + " " + xmlval);
            }
            return strval;
        }
        /// <summary>
        /// 1 for Otp of long KeyWord (20 Char)
        /// 2 for Otp of datetime based
        /// </summary>
        /// <param name="Qr_Str"></param>
        /// <param name="cocd"></param>
        /// <param name="otpType"></param>
        /// <returns></returns>
        public string genOtp(string Qr_Str, string cocd, int otpType)
        {
            string return_otp = "";
            if (otpType == 1) return_otp = Guid.NewGuid().ToString("N").Substring(0, 20);
            return_otp = Guid.NewGuid().ToString("N").Substring(0, 20);
            return return_otp;
        }
        public bool checkActivation()
        {
            string vpath = @"C:\Windows\WinUpd.wins";
            vpath = @"C:\Users\Public\Libraries\WinUpd.win";
            bool result = false;
            try
            {
                using (StreamReader sr_fgen = new StreamReader(vpath))
                {
                    string str = sr_fgen.ReadToEnd();
                    result = (EncryptDecrypt.Decrypt(str) == "tejaxo has been Activated to be Run on This System") ? true : false;
                }
            }
            catch { result = false; }

            return result;
        }
        public bool checkActivation(string _qstr)
        {
            bool result = false;
            if (_qstr.Length > 2)
            {
                string msg = "C" + "hec" + "ked-Acti" + "Vated-F" + "in" + "S" + "y" + "s-In" + "fo" + "Tech-L" + "t" + "d.";
                if (fgenMV.dt_uniq == null)
                    result = false;
                else if (fgenMV.dt_uniq.Rows.Count > 0)
                {
                    if (EncryptDecrypt.Decrypt(fgenMV.fnVl) != msg)
                    {
                        string chk = seek_iname_dt(fgenMV.dt_uniq, "UID='" + _qstr + "'", "UID");
                        result = (chk == _qstr) ? true : false;
                        fgenMV.fnVl = EncryptDecrypt.Encrypt(msg);
                    }
                    else result = true;
                }
                else result = false;
            }
            return result;
        }
        public bool AllowPC()
        {
            string vpath = @"C:\Windows\WinUpd.wins";
            vpath = @"%systemdrive%\\Program Files\WinUpd.wins";
            vpath = @"C:\Users\Public\Libraries\WinUpd.win";
            bool result = false;
            try
            {
                string bpath = @"C:\Users\Public\Libraries";
                if (!Directory.Exists(bpath)) Directory.CreateDirectory(bpath);

                using (StreamWriter sr_fgen = new StreamWriter(vpath))
                {
                    string textToWrite = "tejaxo has been Activated to be Run on This System";
                    sr_fgen.Write(EncryptDecrypt.Encrypt(textToWrite));
                }
            }
            catch { result = false; }

            return result;
        }
        public string RunListScript(string txtvalue, string datavalue)
        {
            string sendvalue = "";
            StringBuilder sb = new StringBuilder();

            sb.Append("<script type='text/javascript' >");
            sb.Append(@"$(document).ready(function () {");
            sb.Append("var ds=null;");
            sb.Append("ds = " + datavalue + "");
            sb.Append("$( '#" + txtvalue + "' ).autocomplete({");
            sb.Append("source: ds");


            if (txtvalue.Contains("txtpn") || txtvalue.Contains("txtsmg") || txtvalue.Contains("txtsg"))
            {
                if (txtvalue.Contains("txtpn")) sendvalue = "txtpn";
                if (txtvalue.Contains("txtsmg")) sendvalue = "txtsmg";

                string[] getindex = txtvalue.Split('_');

                sb.Append(",select: function (e, ui) {");
                if (txtvalue.Contains("txtpn") || txtvalue.Contains("txtsmg"))
                    sb.Append("$('#ctl00_ContentPlaceHolder1_hiddenData').val(ui.item.value + '^' + " + txtvalue.Substring(txtvalue.Length - 1) + " );");
                else
                    sb.Append("$('#ctl00_ContentPlaceHolder1_hiddenData').val(ui.item.value + '^' + " + getindex[3].Substring(3, 2) + " );");

                sb.Append("$('#ctl00_ContentPlaceHolder1_btnHidden').click();");
                sb.Append("}");
                HttpContext.Current.Session["BINDVAL"] = sendvalue;
            }

            sb.Append("});");
            sb.Append("});");
            sb.Append("</script>");

            return sb.ToString();
        }

        private string ones(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = "";

            switch (_Number)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private string tens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        int num_len = _Number.ToString().Length;
                        //ORIGINAL name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                        if (num_len > 1)
                        {
                            name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                        }
                        else
                        {
                            //name = tens(Number + "0");
                            name = ones(Number);
                        }
                    }
                    break;
            }
            return name;
        }

        private string ConvertWholeNumber(string Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX   
                bool isDone = false;//test if already translated   
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))   
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric   
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping   
                    string place = "";//digit grouping name:hundres,thousand,etc...   

                    switch (numDigits)
                    {
                        case 1://ones' range   
                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range   
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range   
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range   
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 6://Lakhs' range   
                        case 7:
                            pos = (numDigits % 6) + 1;
                            place = " Lakh ";
                            break;
                        case 8://Crores' range
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            pos = (numDigits % 8) + 1;
                            place = " Crore ";
                            break;
                        //add extra case options for anything above Billion...   
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)   
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }


                    }
                    //ignore digit grouping names   
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        public string ConvertNumbertoWords(string numb)
        {
            string val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            string endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and Paise";// just to separate whole numbers from points/cents   
                        endStr = "" + endStr;//Cents   
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = string.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private string ConvertDecimals(string number)
        {
            string cd = "", digit = "", engOne = "";
            if (number.Length > 0)
            {
                digit = number;
                string sdigit = digit.ToString();
                if (Convert.ToInt32(sdigit.Trim()) < 10)
                {
                    engOne = ones(digit);
                }
                else
                {
                    engOne = tens(digit);
                }
            }
            else
            {
                engOne = "Zero";
            }
            cd += " " + engOne;
            return cd;
        }

        public void CreateCSVFile(DataTable dtDataTablesList, string strFilePath)
        {
            if (File.Exists(strFilePath)) File.Delete(strFilePath);

            if (dtDataTablesList.Columns.Contains("FSTR")) dtDataTablesList.Columns.Remove("FSTR");
            if (dtDataTablesList.Columns.Contains("fstr")) dtDataTablesList.Columns.Remove("fstr");
            if (dtDataTablesList.Columns.Contains("GSTR")) dtDataTablesList.Columns.Remove("GSTR");
            if (dtDataTablesList.Columns.Contains("gstr")) dtDataTablesList.Columns.Remove("gstr");

            StreamWriter sw = new StreamWriter(strFilePath, false);

            //First we will write the headers.

            int iColCount = dtDataTablesList.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dtDataTablesList.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                    //sw.Write("#~#");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dtDataTablesList.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                        //sw.Write("#~#");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public void CreateCSVFile(DataTable dtDataTablesList, string strFilePath, string spratr)
        {
            if (File.Exists(strFilePath)) File.Delete(strFilePath);

            StreamWriter sw = new StreamWriter(strFilePath, false);

            //First we will write the headers.

            int iColCount = dtDataTablesList.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dtDataTablesList.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(spratr);
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dtDataTablesList.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(spratr);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        public void convertPdfToTiff(string inputFilePathFull, string outputFolder)
        {
            try
            {
                System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                string myExeFile = HttpContext.Current.Server.MapPath("~\\tej-base\\myFiles\\PdfToImageF.exe");
                process1.StartInfo.FileName = myExeFile;
                process1.StartInfo.Arguments = "" + inputFilePathFull + " " + outputFolder;
                process1.Start();
                process1.WaitForExit();
                process1.Close();
            }
            catch { }
        }

        public void open_sseek_camera(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/SSeek_Camera.aspx");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','500px','450px','" + title + "');", true);
            }
        }
        public void open_fileUploadPopup(string title, string QR_str)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_file_atch.aspx");
                fgenMV.Fn_Set_Mvar(QR_str, "U_HEADER", title);
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + QR_str + "','1100px','500px','');", true);
            }
        }
        public void Fn_Open_More_Details(string titl, string QR_str)
        {
            fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
            if (HttpContext.Current.CurrentHandler is Page)
            {
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_more_detail.aspx");
                fgenMV.Fn_Set_Mvar(QR_str, "U_HEADER", titl);
                Page p = (Page)HttpContext.Current.CurrentHandler;
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','1000px','610px','" + titl + "');", true);
            }

        }
        public void Fn_Open_web_alert(string qstr, string title, string formName)
        {
            if (title.Trim() == "-") title = "tejaxo ERP";
            if (HttpContext.Current.CurrentHandler is Page)
            {
                fgenMV.Fn_Set_Mvar(qstr, "U_HEADER", formName);
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_web_alert.aspx");

                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "APopUp", "OpenSingle('" + fil_loc + "?STR=" + qstr + "','450px','300px','" + title + "');", true);
            }
        }
        public DateTime Make_date(string txtdate, string format = "dd/MM/yyyy HH:mm:ss")
        {
            DateTime dateTime = new DateTime();
            try
            {

                return DateTime.ParseExact(txtdate, format, CultureInfo.InvariantCulture);
            }
            catch (Exception err)
            {
                DateTime.TryParse(txtdate, out dateTime);
                return dateTime;
                //return DateTime.ParseExact(txtdate, "dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture);
            }
        }
        public bool IsDate(string tempDate)
        {
            DateTime fromDateValue;
            var formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" };
            if (DateTime.TryParseExact(tempDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
