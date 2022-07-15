<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="Om_Json_form" Title="Tejaxo" CodeFile="Om_Json_form.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--    <script type="text/javascript">
function submitFile() {
                $("#<%= btnupload.ClientID%>").click();
            };
</script>--%>

    <script language="javascript" type="text/javascript">
        var size = 2;
        var id = 0;
        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=FileUpload1.ClientID %>').value != "") {
                document.getElementById("ctl00_ContentPlaceHolder1_divProgress").style.display = "block";
                document.getElementById("ctl00_ContentPlaceHolder1_divUpload").style.display = "block";
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
            document.getElementById("ctl00_ContentPlaceHolder1_divProgress").style.width = size + "pt";
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
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td>
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="S" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" Width="100px" OnClick="btnext_Click" />
                    </td>                    
                    <td>
                        <asp:TextBox ID="txtdate" runat="server" Width="70px"></asp:TextBox>
                        <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server" Enabled="True"
                            TargetControlID="txtdate" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                            MaskType="Date" TargetControlID="txtdate" />
                        <%-- <asp:DropDownList ID="dd1" runat="server" Width="80px" Height="28px">
                                        </asp:DropDownList> --%>
                    </td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1"></label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Date</label>
                                <div class="col-sm-3">
                                    <input id="txtvchdate" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" onchange="submitFile()" />
                                    <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" OnClick="btnupload_Click" />

                                    <asp:TextBox ID="txtAttch" runat="server" Visible="false"></asp:TextBox>

                                </div>
                                <div>
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
                                <div class="col-sm-2">
                                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-2" style="display: none;">
                                    <asp:Button ID="btnexp" runat="server" Text="Export" OnClick="btnexp_Click" CssClass="mybutton" Height="30px" Width="100px" />
                                </div>
                                <div class="col-sm-2" style="display: none;">
                                    <asp:Button ID="btntfr" runat="server" Text="Tfr to Rej" CssClass="mybutton" Height="30px" Width="100px"
                                        OnClick="btntfr_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="display: none;">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <%-- <label id="lblAcode" runat="server" class="col-sm-2 control-label"></label>--%>
                                <div class="col-sm-1">
                                    <asp:Label ID="lblAcode" runat="server" Text="Party Code:" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnacode" ToolTip="Select Party Code" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Width="24px" Height="22px" OnClick="btnacode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="200px" Height="30px" placeholder="Party Code" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtaname" runat="server" Width="400px" Height="30px" placeholder="Party Name" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" Width="100%" ForeColor="#333333" GridLines="Both"
                                            Style="background-color: #FFFFFF; color: White;" OnRowDataBound="sg1_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsr" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <EmptyDataTemplate>
                                                <asp:Image ID="imgdata" runat="server" ImageUrl="~/images/DataNotFound.jpg" Visible="false" AlternateText="No Data Exist"
                                                    Width="400px" />
                                            </EmptyDataTemplate>
                                        </asp:GridView>

                                    </div>

                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; height: 400px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg3" runat="server" Width="100%" ForeColor="#333333" GridLines="Both"
                                            Style="background-color: #FFFFFF; color: White;">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <%--<asp:TemplateField HeaderText="Sr.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsr" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            </Columns>
                                            <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <EmptyDataTemplate>
                                                <asp:Image ID="imgdata" runat="server" ImageUrl="~/images/DataNotFound.jpg" Visible="false" AlternateText="No Data Exist"
                                                    Width="400px" />
                                            </EmptyDataTemplate>
                                        </asp:GridView>

                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>


    <table>
        <tr id="tr1" runat="server">
        </tr>
    </table>


    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:GridView ID="sg2" runat="server">
    </asp:GridView>
    <asp:HiddenField ID="hffild" runat="server" />
    <asp:HiddenField ID="hfxml" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />

</asp:Content>
