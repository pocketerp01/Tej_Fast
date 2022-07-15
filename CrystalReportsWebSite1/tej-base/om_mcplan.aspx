<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="fin_ppc_web_om_mcplan" EnableViewState="true" CodeFile="om_mcplan.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);

            gridviewScroll('#<%=sg2.ClientID%>', gridDiv2, 1, 1);
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,
                barhovercolor: "#3399FF",
                barcolor: "#3399FF",
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
            }

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

            function calC() {
                var grid1 = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid1.rows.length - 1; i++) {
                    var frmFormID = $('input[id$=hfFormID]').val();
                    var minAllow = document.getElementById('ContentPlaceHolder1_sg1_sg1_f5_' + i).innerText;
                    var totMin = (fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_f1_' + i).innerText) / 1000) * minAllow;
                    var sjobNo = document.getElementById('ContentPlaceHolder1_sg1_sg1_f2_' + i).innerText;
                    var sjobDt = document.getElementById('ContentPlaceHolder1_sg1_sg1_f3_' + i).innerText
                    var sqty = document.getElementById('ContentPlaceHolder1_sg1_sg1_f1_' + i).innerText;
                    var sStg = document.getElementById('ContentPlaceHolder1_sg1_sg1_f10_' + i).innerText;
                    var Sicode = document.getElementById('ContentPlaceHolder1_sg1_sg1_f11_' + i).innerText;

                    var newValue = sjobNo + "-" + sjobDt + "-" + sqty + "-" + totMin + "-" + sStg + "-" + Sicode;
                    document.getElementById('ContentPlaceHolder1_sg1_f1_' + i).innerText = newValue;
                }
                timeCalc();
            }
    </script>

    <script type="text/javascript">

        function allowDrop(ev) {
            ev.preventDefault();
            timeCalc();
        }

        function drag(ev) {
            ev.dataTransfer.setData("text", ev.target.id);
        }

        function drop(ev) {
            ev.preventDefault();
            var data = ev.dataTransfer.getData("text");
            ev.target.appendChild(document.getElementById(data));

            timeCalc();
        }

        function timeCalc() {
            var gridCalc = document.getElementById("<%= sg2.ClientID%>");
            var jcSelected = {};
            for (var i = 0; i < gridCalc.rows.length - 1; i++) {
                var jcColTot = 0;
                for (var x = 1; x < 13; x++) {
                    var fieldName = "ContentPlaceHolder1_sg2_m" + x;
                    var v1 = document.getElementById(fieldName + "_" + i).innerText;
                    if (v1.length > 5) {
                        jcColTot += fill_zero(v1.split("-")[3]) * 1;
                    }
                }
                document.getElementById('ContentPlaceHolder1_sg2_sg2_f2_' + i).innerText = jcColTot;
            }
        }


        function funSave() {
            var grid2 = document.getElementById("<%= sg2.ClientID%>");

            var savedSuccess = "N";
            var qstr = document.getElementById("<%=hfQstr.ClientID %>").value;
            var scUrl = "jcpSave.asmx/saveData?STR=" + qstr + "";
            for (var i = 0; i < grid2.rows.length - 1; i++) {
                var f1 = document.getElementById('ContentPlaceHolder1_sg2_sg2_f1_' + i).innerText;

                var saveYesNO = "N";
                var jsc = "";
                var jcIndex = 0;
                var jcList = new Array();
                for (var x = 1; x < 13; x++) {
                    var fieldName = "ContentPlaceHolder1_sg2_m" + x;
                    var v1 = document.getElementById(fieldName + "_" + i).innerText;

                    var JCInfo = new Object();

                    if (v1.length > 5) {

                        JCInfo["MachineNo"] = f1;
                        JCInfo["jobNo"] = v1.split("-")[0];
                        JCInfo["jobDt"] = v1.split("-")[1];
                        JCInfo["qty"] = v1.split("-")[2];
                        JCInfo["MachineTime"] = v1.split("-")[3];
                        JCInfo["stageCode"] = v1.split("-")[4];
                        JCInfo["iCode"] = v1.split("-")[5];
                        JCInfo["shiftCode"] = document.getElementById("ContentPlaceHolder1_txtShiftName").value.split("-")[0];
                        JCInfo["shiftName"] = document.getElementById("ContentPlaceHolder1_txtShiftName").value.split("-")[1];
                        JCInfo["shiftTime"] = document.getElementById("ContentPlaceHolder1_hf1").value;

                        saveYesNO = "Y";
                        jcList[jcIndex] = JCInfo;
                        jcIndex++;
                    }
                }
                jsc = JSON.stringify(jcList);
                if (saveYesNO == "Y") {
                    savedSuccess = "Y";

                    $.ajax({
                        url: "" + scUrl + "",
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: '{JC:' + jsc + '}',
                        success: function () {
                            savedSuccess = "Y";
                        },
                        error: function (err) {
                            console.log(err);
                            alert(err);
                        }
                    });
                }
            }
            if (savedSuccess == "Y") {
                alert("Saved Successfully!!")
            }
            window.location = window.location.href;

        }

    </script>
    <style>
        .divStyle {
            width: 100px;
        }

        .bgGrad {
            background-image: linear-gradient(to bottom,#fff,#ade8df);
            /*background-image: linear-gradient(to bottom,yellow,red);*/
        }

        .grad {
            color: #1e0202;
            background-image: linear-gradient(to bottom,#cef8e9, rgba(243,227,226,.12));
            margin: 0;
            padding: 0;
        }

        .grad1 {
            background-image: linear-gradient(to bottom,rgba(243,227,226,.12),#fff);
        }

        .zoom {
            transition: transform .2s;
            /*margin: 0 auto;*/
            word-wrap: break-word;
        }

            .zoom:hover {
                padding-left: 2px;
                -ms-transform: scale(1.05);
                -webkit-transform: scale(1.05);
                transform: scale(1.05);
                font-weight: 600;
                border-color: ActiveBorder;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper bgGrad">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td>
                        <div class="col-sm-2">
                            <asp:ImageButton ID="btnShift" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnShift_Click" Visible="false" />
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtShiftName" runat="server" ReadOnly="true" placeholder="Shift" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtShiftTime" runat="server" ReadOnly="true" placeholder="Shift Time" CssClass="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>Corr</u>Print</button>
                        <button type="submit" id="btnOtherPrint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="O" onserverclick="btnOtherPrint_ServerClick"><u>Other</u>Print</button>
                        <%--<button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>--%>

                        <button id="btnSave" class="btn btn-info" style="width: 100px;" runat="server" onclick="funSave(); return false" accesskey="S"><u>S</u>ave</button>
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
                            <div id="gridDiv" class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <asp:GridView ID="sg1" runat="server" Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller" AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Job Name</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="div1" ondrop="drop(event)" ondragover="allowDrop(event)" style="height: 40px; width: 120px;" class="zoom">
                                                    <div id="f1" runat="server" draggable="true" ondragstart="drag(event)" style="height: 40px; width: 120px;" class="zoom">
                                                        <%# Eval("fstr") %>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f1</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f1" runat="server" contenteditable="true" onkeyup="calC();">
                                                    <%# Eval("sg1_f1") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f2</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f2" runat="server">
                                                    <%# Eval("sg1_f2") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f3</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f3" runat="server">
                                                    <%# Eval("sg1_f3") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="sg1_f4" DataField="sg1_f4" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f5</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f5" runat="server">
                                                    <%# Eval("sg1_f5") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="sg1_f6" DataField="sg1_f6" />
                                        <asp:BoundField HeaderText="sg1_f7" DataField="sg1_f7" />
                                        <asp:BoundField HeaderText="sg1_f8" DataField="sg1_f8" />
                                        <asp:BoundField HeaderText="sg1_f9" DataField="sg1_f9" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f10</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f10" runat="server">
                                                    <%# Eval("sg1_f10") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>sg1_f11</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg1_f11" runat="server">
                                                    <%# Eval("sg1_f11") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div id="gridDiv2" class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <asp:GridView ID="sg2" runat="server" Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller" AutoGenerateColumns="false" OnRowDataBound="sg2_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <Columns>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m1</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg2_f1" runat="server" style="height: 40px; width: 200px;">
                                                    <%#Eval("sg2_f1") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>sg2_f2</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="sg2_f2" runat="server" style="height: 40px; width: 40px;">
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m1</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m1" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m2</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m2" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m3</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m3" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m4</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m4" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m5</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m5" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                        <asp:TemplateField>
                                            <HeaderTemplate>m6</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m6" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m7</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m7" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m8</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m8" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m9</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m9" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m10</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m10" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m11</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m11" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>m12</HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="m12" runat="server" class="grad" style="border: 1px solid #dbd7d7; height: 40px; width: 140px;" ondrop="drop(event)" ondragover="allowDrop(event)"></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:Button ID="btnhideSave" runat="server" OnClick="btnhideSave_Click" Style="display: none" />
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
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfQstr" runat="server" />
    <asp:HiddenField ID="hfFormID" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
</asp:Content>

