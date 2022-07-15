<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_costing_est" CodeFile="om_costing_est.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>



    <script type="text/javascript">
        $(document).ready(function () {
         
            //calculateSum();
        });
       

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                       <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
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

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label19" runat="server" class="col-md-4 control-label" title="lbl1">EntryNo.</label>

                                <div class="col-md-8">
                                    <input id="txtVchnum" type="text" readonly="readonly" class="form-control" runat="server" placeholder="EntryNumber" maxlength="150" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-md-4 control-label" title="lbl1">EntryDt.</label>

                                <div class="col-md-8">
                                    <input id="txtVchdate" type="text" class="form-control" style="height:30px"  readonly="readonly" runat="server" placeholder="EntryDate" maxlength="9" />
                                </div>
                            </div>

                          
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label13" runat="server" class="col-md-3 control-label" title="lbl1">Customer</label>

                                <div class="col-md-8">
                                    <input id="txtCustomer" type="text" class="form-control" style="height:30px" runat="server" placeholder="CustomerName" maxlength="100" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label17" runat="server" class="col-md-3 control-label" title="lbl1">Item</label>

                               

                                <div class="col-md-8">
                                    <input id="txtItem" type="text"  class="form-control" runat="server" placeholder="Item Name" maxlength="50" />
                                </div>
                            </div>
                        

                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                          
                            <div class="col-md-5">
                                <div class="form-group">
                                <label id="Label48" runat="server"  class="col-md-12 control-label" title="lbl1" style="background-color:#33FFD1;align-content:center;text-align:center;">Box Dimensions in mm Specifications</label>
                            </div>

                               
                            <div class="form-group">
                                <label id="Label49" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">L</label>
                            </div>

                            <div class="form-group">
                                <label id="Label50" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">W</label>
                            </div>

                            <div class="form-group">
                                <label id="Label51" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">H</label>
                            </div>
                            
                            <div class="form-group">
                                <label id="Label52" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Ply</label>
                            </div>

                            <div class="form-group">
                                <label id="Label39" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Flute</label>
                            </div>

                            <div class="form-group">
                                <label id="Label53" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">CS</label>
                            </div>
                            
                            <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtL" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtWid" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                                 <div class="col-md-2">
                                    <input id="txtHeight" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                               <div class="col-md-2">
                                <select id="txtPly" runat="server" style="height:30px" class="form-control">
                                        <option value="3" selected="selected">3</option>
                                        <option value="5" >5</option>
                                    </select>
                                </div>
                            </div>

                             <div class="form-group">
                                 <div class="col-md-2">
                                    <select id="txtFlute" style="height:30px" runat="server" class="form-control">
                                        <option value="B" selected="selected">B</option>
                                        <option value="C" >C</option>
                                        <option value="BC">BC</option>
                                    </select>
                                </div>
                           </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtCs" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

               <!--second row-->
                           

                            <div class="form-group">
                                <label id="Label54" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Caliper</label>
                            </div>

                            <div class="form-group">
                                <label id="Label66" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Z</label>
                            </div>

                            <div class="form-group">
                                <label id="Label67" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">ECT</label>
                            </div>

                            <div class="form-group">
                                <label id="Label68" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">BS</label>
                            </div>

                            <div class="form-group">
                                <label id="Label69" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">GSM</label>
                            </div>

                            <div class="form-group">
                                <label id="Label70" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Deckle</label>
                            </div>
                            

                            <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCaliper" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>

                             <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtZ" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  readonly="readonly" maxlength="9" />
                                </div>
                            </div>

                             <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtECT" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                                 <div class="col-md-2">
                                    <input id="txtBS" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtGSM" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                             </div>

                           <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtDeckle" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

               <!--Third row-->

                            <div class="form-group">
                                <label id="Label71" runat="server"  class="col-md-6 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Length</label>
                            </div>

                            <div class="form-group">
                                <label id="Label72" runat="server"  class="col-md-6 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;">Area</label>
                            </div>
                                 
                             <div class="form-group">
                                <div class="col-md-6">
                                    <input id="txtLength" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                               <div class="col-md-6">
                                    <input id="txtArea" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>
      <!-- 4th row-->

                             <div class="form-group">
                                <label id="Label73" runat="server"  class="col-md-1 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;font:small;height:40px">Layer</label>
                            </div>

                             <div class="form-group">
                                <label id="Label74" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;height:40px">GSM</label>
                            </div>

                             <div class="form-group">
                                <label id="Label75" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;height:40px">BF</label>
                            </div>

                            <div class="form-group">
                                <label id="Label76" runat="server"  class="col-md-1 control-label" title="lbl1" w style="background-color:#F9FF33;text-align:left;height:40px">RCT Grade</label>
                            </div>

                            <div class="form-group">
                                <label id="Label77" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;height:40px">RCT</label>
                            </div>

                            <div class="form-group">
                                <label id="Label78" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;text-align:left;height:40px">T.RCT</label>
                            </div>

                            <div class="form-group">
                                <label id="Label79" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:40px">COST</label>
                            </div>

                            <div class="form-group">
                                <div class="col-md-1">
                                   <label id="Label80" runat="server"  class="col-md-1 control-label" title="lbl1" style="text-align:left;font:small" >Top</label>
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtGSM1" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                   <div class="col-md-2">
                                         <select id="txtBF1" runat="server" style="height:30px" class="form-control">
                                        <option value="16" selected="selected">16</option>
                                        <option value="18" >18</option>
                                        <option value="20">20</option>
                                        <option value="22">22</option>
                                        <option value="24">24</option>
                                        <option value="28">28</option>
                                        <option value="35">35</option>
                                        <option value="45">45</option>
                                    </select>
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-1">
                                    <input id="txtRCTGrade1" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtRCT1" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtTRCT1" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCost1" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

      <!-- 5th row-->

                             <div class="form-group">
                                <div class="col-md-1">
                                   <label id="Label81" runat="server"  class="col-md-1 control-label" title="lbl1" style="text-align:left;font:small">Flu1</label>
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtGSM2" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                      <div class="col-md-2">
                                         <select id="txtBF2" runat="server" style="height:30px" class="form-control">
                                        <option value="16" selected="selected">16</option>
                                        <option value="18" >18</option>
                                        <option value="20">20</option>
                                        <option value="22">22</option>
                                        <option value="24">24</option>
                                        <option value="28">28</option>
                                        <option value="35">35</option>
                                        <option value="45">45</option>
                                    </select>
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-1">
                                    <input id="txtRCTGrade2" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtRCT2" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtTRCT2" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCost2" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>
      <!-- 6th row-->

                            <div class="form-group">
                                <div class="col-md-1">
                                   <label id="Label82" runat="server"  class="col-md-1 control-label" title="lbl1" style="text-align:left;font:small" >Lin1</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtGSM3" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                         <select id="txtBF3" runat="server" style="height:30px" class="form-control">
                                        <option value="16" selected="selected">16</option>
                                        <option value="18" >18</option>
                                        <option value="20">20</option>
                                        <option value="22">22</option>
                                        <option value="24">24</option>
                                        <option value="28">28</option>
                                        <option value="35">35</option>
                                        <option value="45">45</option>
                                    </select>
                                </div>
                                
                            </div>

                            <div class="form-group">
                               <div class="col-md-1">
                                    <input id="txtRCTGrade3" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtRCT3" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtTRCT3" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  readonly="readonly" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCost3" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

        <!-- 7th row-->
                          <div class="form-group">
                                <div class="col-md-1">
                                   <label id="Label83" runat="server"  class="col-md-1 control-label" title="lbl1" style="text-align:left;font:small" >Flu2</label>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtGSM4" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                         <select id="txtBF4" runat="server" style="height:30px" class="form-control">
                                        <option value="16" selected="selected">16</option>
                                        <option value="18" >18</option>
                                        <option value="20">20</option>
                                        <option value="22">22</option>
                                        <option value="24">24</option>
                                        <option value="28">28</option>
                                        <option value="35">35</option>
                                        <option value="45">45</option>
                                    </select>
                                </div>
                                
                            </div>

                            <div class="form-group">
                               <div class="col-md-1">
                                    <input id="txtRCTGrade4" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>

                            <div class="form-group">
                              <div class="col-md-2">
                                    <input id="txtRCT4" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtTRCT4" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCost4" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>
        <!-- 8th row-->
                                  <div class="form-group">
                                <div class="col-md-1">
                                   <label id="Label84" runat="server"  class="col-md-1 control-label" title="lbl1" style="text-align:left;font:small">Lin2</label>
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtGSM5" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                     <div class="col-md-2">
                                         <select id="txtBF5" runat="server" style="height:30px" class="form-control">
                                        <option value="16" selected="selected">16</option>
                                        <option value="18" >18</option>
                                        <option value="20">20</option>
                                        <option value="22">22</option>
                                        <option value="24">24</option>
                                        <option value="28">28</option>
                                        <option value="35">35</option>
                                        <option value="45">45</option>
                                    </select>
                                </div>
                                                                
                            </div>
                                 <div class="form-group">
                               <div class="col-md-1">
                                    <input id="txtRCTGrade5" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtRCT5" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtTRCT5" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  readonly="readonly" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtCost5" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                            </div>

  <!---9 th row-->
                                 <div class="form-group">
                               
                             
                                     <label id="Label86" runat="server"  class="col-md-6 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center">Total</label>
                                 
                             
                                <div class="col-md-3">
                                    <input id="txtTRCTtot" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>
                                      <div class="col-md-3">
                                    <input id="txtCosttot" type="text" class="form-control" runat="server"  style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>

 <!--10th row-->
                                  <div class="form-group">
                                <label id="Label85" runat="server"  class="col-md-3 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">Cal.Parameter</label>
                               
                            </div>
                                 <div class="form-group">
                                <label id="Label87" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">Weight</label>
                               
                            </div>
                                 <div class="form-group">
                                <label id="Label88" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">BS</label>
                               
                            </div>
                                 <div class="form-group">
                                <label id="Label89" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">GSM</label>
                               
                            </div>
                                  <div class="form-group">
                                <label id="Label90" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">ECT</label>
                               
                            </div>
                                 <div class="form-group">
                                <label id="Label91" runat="server"  class="col-md-1 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;">CS</label>
                               
                            </div>
                                 
                                      <div class="form-group">
                               
                            
                                   <label id="Label92" runat="server"  class="col-md-3 control-label" title="lbl1" >Minimum</label>
                               
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtwghtmin" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                 <div class="col-md-2">
                                    <input id="txtBSmin" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtGSMmin" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtECTmin" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-1">
                                    <input id="txtCSmin" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                

                            <div class="form-group">
                                   <label id="Label93" runat="server"  class="col-md-3 control-label" title="lbl1" >Maximum</label>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtwghtmax" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                 <div class="col-md-2">
                                    <input id="txtBSmax" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtGSMmax" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtECTmax" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-1">
                                    <input id="txtCSmax" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                               
                            
                                   <label id="Label94" runat="server"  class="col-md-3 control-label" title="lbl1" >Average</label>
                            
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txtwghtavg" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                                 <div class="col-md-2">
                                    <input id="txtBSavg" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                               
                            </div>
                                 <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txtGSMavg" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                                  <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtECTavg" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-1">
                                    <input id="txtCSavg" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                               
                            </div>
                            
                                </div>
                              
                             <!-- 2nd-->

                                <div class="col-md-4">

                               <div class="form-group">
                               <asp:Button ID="btncal" runat="server" CssClass="btn btn-block warning" BackColor="YellowGreen" BorderStyle="Double" OnClick="btncal_Click" Text="CALCULATE" Font-Bold="true" ToolTip="Pease click the calculate button after entering all values to calculate  Box-Cost."/>
                               <label id="Label27" runat="server" class="col-md-12 control-label" style="text-align:justify;background-color:burlywood"  title="lbl1">The grey textboxes are autocalculated. Please enter values in white textboxes.Paper GSM,BF,Grade, RCT (Left Hand side segment) are coming from masters.Paper Index, Rates(below) are coming from masters but rates are editable.Conversion costs(right hand side segment) are to be entered for 1000 pcs.Enter desirable Box dimensions, available paper combination, press calculate (green button above) and see the cost per box. </label>
                                <label id="Label9" runat="server"  class="col-md-12 control-label" title="lbl1" style="background-color:#33FFD1;align-content:center;text-align:center;">Paper RCT INDEX & RATE(from Master)</label>
                               
                            </div>
                                <div class="form-group">
                                <label id="Label47" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:60px">BF</label>
                            </div>
                                     <div class="form-group">
                                <label id="Label1" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:60px">High RCT Index</label>

                            </div>
                                      <div class="form-group">
                                <label id="Label2" runat="server"  class="col-md-2 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:60px">High RCT Rate</label>

                            </div>
                                      <div class="form-group">
                                <label id="Label7" runat="server"  class="col-md-3 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:60px">Normal RCT Index</label>

                            </div>
                                      <div class="form-group">
                                <label id="Label8" runat="server"  class="col-md-3 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;height:60px">Normal RCT Rate</label>

                            </div>
                                 <!--2nd row-->
                              
                                <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFa" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexa" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRatea" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexa" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRatea" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                    <!--3rd row-->
                                    
                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFb" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexb" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRateb" type="text" class="form-control" runat="server"  style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexb" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRateb" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                    
                                    
                                    
                                 <!---4 row-->

                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFc" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexc" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRatec" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexc" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRatec" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>

                                    <!--5 row-->

                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFd" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexd" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRated" type="text" class="form-control" runat="server" style="text-align:right;height:30px"   maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexd" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRated" type="text" class="form-control" runat="server" style="text-align:right;height:30px"   maxlength="9" />
                                </div>

                            </div>
                                    <!--6 row-->
                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFe" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexe" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRatee" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexe" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRatee" type="text" class="form-control" runat="server" style="text-align:right;height:30px"   maxlength="9" />
                                </div>

                            </div>
                                    <!--7 row-->
                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFf" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexf" type="text" class="form-control" runat="server"  readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRatef" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexf" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRatef" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                    <!--8-->

                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFg" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexg" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRateg" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexg" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRateg" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                    <!--9-->
                                       <div class="form-group">
                             <div class="col-md-2">
                                    <input id="txtBFh" type="text" class="form-control" runat="server"  style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>
                            </div>
                                     <div class="form-group">
                               <div class="col-md-2">
                                    <input id="txthighRCTIndexh" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-2">
                                    <input id="txthighRCTRateh" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                                <div class="col-md-3">
                                    <input id="txtNormalRCTIndexh" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>

                            </div>
                                      <div class="form-group">
                              <div class="col-md-3">
                                    <input id="txtNormalRCTRateh" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>

                            </div>
                                   <div class="form-group">
                                <label id="Label36" runat="server"  class="col-md-12 control-label" title="lbl1" style="background-color:#F9FF33;align-content:center;"></label>
                            </div>
                          
                                 <div class="form-group">
                                <label id="Label4" runat="server" class="col-md-2 control-label" title="lbl1">Remarks</label>

                               
                                <div class="col-md-10">
                                    <input id="txtRemarks" type="text" placeholder="Remarks(upto 100 Characters)"  class="form-control" runat="server" style="height:50px;text-align:left"  maxlength="150" />
                                </div>
                            </div>
                            
                            <div class="form-group">
                             <div class="col-md-12">
                                 </div>
                            </div>
 

                                   
                                </div>

                        <!-- 3rd-->
                                  <div class="col-md-3">
                              

                                       <div class="form-group">
                                <label id="Label3" runat="server"  class="col-md-12 control-label" title="lbl1" style="background-color:#33FFD1;align-content:center;text-align:center;">Conversion Cost</label>
                               
                            </div>

                                <div class="form-group">
                                <label id="Label56" runat="server" class="col-md-3 control-label" title="lbl1" style="height:35px;background-color:#F9FF33;align-content:center;">Item</label>
                                 <label id="Label57" runat="server" class="col-md-3 control-label" title="lbl1" style="height:35px;background-color:#F9FF33;align-content:center;">Rate</label>
                                 <label id="Label58" runat="server" class="col-md-2 control-label" title="lbl1" style="height:35px;background-color:#F9FF33;align-content:center;">(Y/N)(1/0)</label>
                                  <label id="Label59" runat="server" class="col-md-4 control-label" title="lbl1" style="height:35px;background-color:#F9FF33;align-content:center;">Amount</label>
                            </div>
                                   
                                      <div class="form-group">
                                <label id="Label61" runat="server" class="col-md-3 control-label" title="lbl1">StarchGum</label>

                                <div class="col-md-3">
                                    <input id="txtRateStrch" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNStrch" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtStrch" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label5" runat="server" class="col-md-3 control-label" title="lbl1">PVA Gum</label>

                                <div class="col-md-3">
                                    <input id="txtRatePVA" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNPVA" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtPVA" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label6" runat="server" class="col-md-3 control-label" title="lbl1">Power</label>

                                <div class="col-md-3">
                                    <input id="txtRatePow" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNPow" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtPow" type="text" class="form-control" runat="server"  style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label10" runat="server" class="col-md-3 control-label" title="lbl1">Fuel</label>

                                <div class="col-md-3">
                                    <input id="txtRateFuel" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNFuel" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtFuel" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label12" runat="server" class="col-md-3 control-label" title="lbl1">Stitch_pins</label>

                                <div class="col-md-3">
                                    <input id="txtRateStchPins" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNStchPins" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtStchPins" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label14" runat="server" class="col-md-3 control-label" title="lbl1">Print_Ink</label>

                                <div class="col-md-3">
                                    <input id="txtRatePrint" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNPrint" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtPrint" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label15" runat="server" class="col-md-3 control-label" title="lbl1">Labor</label>

                                <div class="col-md-3">
                                    <input id="txtRatelabor" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNlabor" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtlabor" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label16" runat="server" class="col-md-3 control-label" title="lbl1">Admin_Exp</label>

                                <div class="col-md-3">
                                    <input id="txtRateAdmin" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNAdmin" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtAdmin" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label18" runat="server" class="col-md-3 control-label" title="lbl1">Trans_Exp</label>

                                <div class="col-md-3">
                                    <input id="txtRateTrans" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNTrans" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtTrans" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label20" runat="server" class="col-md-3 control-label" title="lbl1">Oth_Mat.</label>

                                <div class="col-md-3">
                                    <input id="txtRateOtherM" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                <div class="col-md-2">
                                    <input id="txtYNOtherM" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtOtherM" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label21" runat="server" class="col-md-6 control-label" title="lbl1">Contribution %</label>

                                <div class="col-md-2">
                                    <input id="txtRateContri" type="text" class="form-control" runat="server" style="text-align:right;height:30px"  maxlength="9" />
                                </div>
                               
                                <div class="col-md-4">
                                    <input id="txtAmtContri" type="text" class="form-control" runat="server" readonly="readonly" style="text-align:right;height:30px" maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label22" runat="server" class="col-md-6 control-label" title="lbl1">Total_Conv_Cost</label>

                              
                                <div class="col-md-2">
                                   
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtTotalConv" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                      <div class="form-group">
                                <label id="Label23" runat="server" class="col-md-6 control-label" title="lbl1">Conv_Cost/kg</label>

                              
                                <div class="col-md-2">
                                   
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtAmtConvCostperkg" type="text" class="form-control" style="text-align:right;height:30px" readonly="readonly"  runat="server"  maxlength="9" />
                                </div>  
                                        
                                <div class="form-group">
                                <label id="Label24" runat="server" class="col-md-6 control-label" title="lbl1">Paper_Cost</label>

                                <div class="col-md-2">
                                 
                                </div>
                                 
                                   <div class="col-md-4">
                                    <input id="txtAmtPapercost" type="text" class="form-control" readonly="readonly" style="text-align:right;height:30px" runat="server"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                   <div class="form-group">
                                <label id="Label25" runat="server" class="col-md-6 control-label" title="lbl1">Paper_Wastage %</label>

                                <div class="col-md-2">
                                    <input id="txtRatePaperWst" type="text" class="form-control" runat="server" style="text-align:right;height:30px" maxlength="9" />
                                </div>
                              
                                      <div class="col-md-4">
                                    <input id="txtAmtPaperWst" type="text" class="form-control" runat="server" style="text-align:right;height:30px" readonly="readonly"  maxlength="9" />
                                </div>  
                                        
                            </div>
                                           <div class="form-group">
                                <label id="Label26" runat="server" class="col-md-6 control-label" title="lbl1">Box_Costing(Rs.)</label>

                               
                                <div class="col-md-2">
                                 
                                </div>
                                      <div class="col-md-4">
                                    <input id="txtBoxCost" type="text" class="form-control" runat="server" style="text-align:right; font:bold;font-size:large;background-color:cyan;height:30px" readonly="readonly"  maxlength="9" />
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
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab1";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>


