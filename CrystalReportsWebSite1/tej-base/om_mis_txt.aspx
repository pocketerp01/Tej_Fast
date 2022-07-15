<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_mis_txt" CodeFile="om_mis_txt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            //calculateSum();
            refreshPage();
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


            function refreshPage() {
                var min = 1;
                var sec = 60;
                setInterval(function () {
                    document.getElementById("timer").innerHTML = min + " : " + sec;
                    sec--;
                    if (min < 1 && sec < 25) {
                        if (sec % 2 == 0) document.getElementById("timer").style.color = "red";
                        else document.getElementById("timer").style.color = "black";
                    }
                    if (sec < 8) {
                        if (min < 0) min = 0;
                        if (min < 1 && sec < 8) {
                            document.getElementById("ContentPlaceHolder1_btnnew").click();
                        }
                    }
                    if (sec == 0) {
                        sec = 60;
                        min--;
                    }
                    if (min < 0) min = 0;
                }, 1000);
            }

    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td>Refreshing Values in <span id="timer" style="font-weight: bold;"></span>&nbsp;minutes!
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnnew_ServerClick">Refre<u>s</u>h</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-4">
                    <div class="box box-success" id="div_box1" runat="server">
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Box1_01" runat="server" class="col-sm-6 control-label" title="lbl1">Task_No</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box1_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box1_02" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box1_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box1_03" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box1_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box1_04" runat="server" class="col-sm-6 control-label" title="lbl1">Client_Name</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box1_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box2" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box2_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box2_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box2_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box2_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box2_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box2_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box2_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box2_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box3" runat="server">
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Box3_01" runat="server" class="col-sm-6 control-label" title="lbl1">Task_No</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box3_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box3_02" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box3_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box3_03" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box3_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box3_04" runat="server" class="col-sm-6 control-label" title="lbl1">Client_Name</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box3_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box4" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box4_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box4_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box4_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box4_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box4_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box4_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box4_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box4_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box5" runat="server">
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Box5_01" runat="server" class="col-sm-6 control-label" title="lbl1">Task_No</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box5_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box5_02" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box5_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box5_03" runat="server" class="col-sm-6 control-label" title="lbl1">Task Date</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box5_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Box5_04" runat="server" class="col-sm-6 control-label" title="lbl1">Client_Name</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box5_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box6" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box6_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box6_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box6_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box6_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box6_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box6_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box6_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box6_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box7" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box7_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box7_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box7_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box7_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box7_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box7_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box7_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box7_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box8" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box8_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box8_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box8_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box8_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box8_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box8_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box8_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box8_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box9" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box9_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box9_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box9_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box9_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box9_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box9_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box9_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box9_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box10" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <div>
                                    <label id="Box10_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                </div>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box10_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box10_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box10_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box10_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box10_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box10_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box10_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box11" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box11_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box11_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box11_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box11_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box11_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box11_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box11_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box11_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="box box-success" id="div_box12" runat="server">
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Box12_01" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px; background-color: #99e4c2; font-weight: bolder; font-size: large;" id="Box12_T01" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box12_02" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box12_T02" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box12_03" runat="server" class="col-sm-6 control-label" title="lbl1">Specific Area</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box12_T03" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Box12_04" runat="server" class="col-sm-6 control-label" title="lbl1">Contact Person</label>
                                <div class="col-sm-6">
                                    <input style="text-align: right; height: 26px" id="Box12_T04" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="display: none">
                    <div>
                        <div class="box-body">
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" /></td>
                                    <td>
                                        <asp:TextBox ID="txtAttch" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAtt" runat="server" Text="Attachment" OnClick="btnAtt_Click" Width="134px" Style="display: none" />

                            <asp:Label ID="lblShow" runat="server"></asp:Label>
                            <asp:Label ID="lblUpload" runat="server" Style="display: none"></asp:Label>

                            <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                            <asp:ImageButton ID="btnDwnld1" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDwnld1_Click" Visible="false" />
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="40"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 150px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2" />
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3" />
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4" />
                                                <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5" />
                                                <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6" />
                                                <asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7" />
                                                <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8" />
                                                <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9" />
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%--                                                        <asp:TemplateField>
                                                            <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="sg1_t2" />
                                                                <asp:CalendarExtender ID="txtvchdate_CalendarExtender2" runat="server"
                                                                    Enabled="True" TargetControlID="sg1_t2"
                                                                    Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
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
