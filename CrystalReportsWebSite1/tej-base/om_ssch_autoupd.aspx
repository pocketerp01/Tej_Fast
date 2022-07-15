<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_ssch_autoupd" CodeFile="om_ssch_autoupd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">   s   
    <script type="text/javascript" src="Scripts/jquery.handsontable.full.js"></script>
    <link rel="Stylesheet" type="text/css" href="Styles/jquery.handsontable.full.css" />
    <script type="text/javascript">
        var size = 2;
        var id = 0;
        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=FileUpload1.ClientID %>').value != "") {
                document.getElementById("ContentPlaceHolder1_divProgress").style.display = "block";
                document.getElementById("ContentPlaceHolder1_divUpload").style.display = "block";
                id = setInterval("progress()", 20);
                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }
        }
        function progress() {
            size = size + 1;
            if (size > 199) {
                clearTimeout(id);
            }
            document.getElementById("ContentPlaceHolder1_divProgress").style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").
                firstChild.data = parseInt(size / 2) + "%";
        }
    </script>       
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" visible="false" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" visible="false" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btnvalidate" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnvalidate_ServerClick">Va<u>li</u>date</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" visible="false" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">V<u>i</u>ew</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="EntryNo" CssClass="col-sm-1 control-label"></asp:Label>
                                <asp:Label ID="lbl1a" runat="server" Visible="false" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                </div>
                                 <asp:Label ID="Label2" runat="server" Text="EntryDate" CssClass="col-sm-1 control-label"></asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                                 <asp:Label ID="Label1" runat="server" Text="Customer" CssClass="col-sm-1 control-label"></asp:Label>
                                 <div class="col-sm-1" runat="server">
                                                        <asp:ImageButton ID="btniname" runat="server" ToolTip="Select Customer Name" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btniname_Click" />
                                                    </div>
                                 <div class="col-sm-1">
                                    <asp:TextBox ID="txtacode" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                </div>
                                 <div class="col-sm-3">
                                    <asp:TextBox ID="txtaname" runat="server" Width="300px" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                            <br />
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" onchange="submitFile()" />
                            <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" Style="display: none" OnClick="btnupload_Click" />

                            <div id="divUpload" style="display: none" runat="server">
                                <div id="Div1" style="width: 200pt; height: 15px; border: solid 1pt gray" runat="server">
                                    <div id="divProgress" runat="server" style="width: 1pt; height: 15px; background-color: #1797C0; display: none">
                                    </div>
                                </div>
                                <div style="width: 200pt; text-align: center;">
                                    <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                    <div class="col-md-12">
                    <div>
                        <div class="box-body">
                     <div class="lbBody" style="height: 500px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1250px"  Font-Size="13px" 
                                            AutoGenerateColumns="true" CaptionAlign="Bottom" EmptyDataText="page" Font-Names="Arial">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>                                            

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerSettings FirstPageText="Pagertretretretr" LastPageText="Last" Mode="NumericFirstLast" NextPageText="Next" PreviousPageText="Prev" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                            </div>
                        </div></div>

     
              

            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />

    <asp:HiddenField ID="hfname" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:HiddenField ID="hfCNote" runat="server" />

    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
