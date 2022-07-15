<%@ Page Language="C#" AutoEventWireup="true" Inherits="om_ch_paper" CodeFile="om_ch_paper.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />
    <link rel="stylesheet" type="text/css" href="../tej-base/Styles/vip_vrm.css" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll.css" type="text/css" rel="Stylesheet" />
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closePopup1() { $("#ContentPlaceHolder1_btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() }
        function onlyclose() { parent.$.colorbox.close() }
        function gridviewScroll() { $("#sg1").gridviewScroll({ width: 980, height: 420, startHorizontal: 1, headerrowcount: 1, wheelstep: 5, barhovercolor: "#0e7192", barcolor: "#0e7192" }) }
        $(document).ready(function () {
            gridviewScroll();

            var val = $('#txtsearch').val();
            $('#txtsearch').val('');
            $('#txtsearch').val(val);
            $('#txtsearch').focus();
        });
        window.addEventListener("keydown", function (e) {
            // space and arrow keys
            if ([38, 40].indexOf(e.keyCode) > -1) {
                e.preventDefault();
            }
        }, false);
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        function gridRowsSelection() {
            UpperBound = parseInt('<%= this.sg1.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            else if (KeyCode == 33)
                for (var i = 0; i < 10; i++) {
                    SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
                }
            else if (KeyCode == 34) {
                for (var i = 0; i < 10; i++) {
                    SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
                }
            }

            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="margin-top: 40px;">
        <section class="content">
            <div class="row">
                  <div class="col-md-7">
                    <div>
                        <div class="box-body">
                            <div class="form-group">                                         
                        <asp:Label ID="lbl" runat="server" CssClass="col-sm-3 control-label" Text="Paper Type"></asp:Label>                                                  
                                <div class="col-sm-1">
                    <asp:ImageButton ID="btnlbl" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnlbl_Click" />
                                    </div>
                                <div class="col-sm-8">
                         <asp:TextBox ID="txtlblSubGroup" runat="server" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                    </div>
                   </div>
                            <br />
                            <div class="form-group" style="margin-top:20px" >
                                <div class="col-sm-4" >                        
                                    </div>                                
                                <div class="col-sm-8" >                        
                    <asp:Button ID="btnShow" Text="Try Combination/View Stock" class="btn-success" Width="300px" runat="server" OnClick="btnShow_Click"/>                                         
                        <button id="btnsubmit" onserverclick="btnsubmit_ServerClick" runat="server" class="btn btn-info" style="width: 100px; display:none">Submit</button>
                            </div>
                                </div>
                            </div>
                        </div>
                      </div>                              
                  <div class="col-md-5">
                    <div>
                     <div class="form-group">
                        <asp:Label ID="lblLen" Text="Length from" CssClass="col-sm-4" runat="server"></asp:Label>                                                                     
                         <div class="col-sm-3">
                        <asp:TextBox ID="txtLenFrom" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                         </div>
                                    
                        <asp:Label ID="lblTo1" Text="to" CssClass="col-sm-2" runat="server"></asp:Label>
                          <div class="col-sm-3">
                        <asp:TextBox ID="txtLenTo" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                          </div>
                   </div>                       
                        
                     <div class="form-group">
                        <asp:Label ID="lblWidth" Text="Width from" CssClass="col-sm-4" runat="server"></asp:Label>

                          <div class="col-sm-3">
                        <asp:TextBox ID="txtWidthFrom" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)" ></asp:TextBox>
                              </div>

                        <asp:Label ID="lblTo2" Text="to" CssClass="col-sm-2" runat="server"></asp:Label>

                          <div class="col-sm-3">
                        <asp:TextBox ID="txtWidthTo" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                              </div>
                         </div>
                  
                     <div class="form-group">
                        <asp:Label ID="lblGSM" Text="GSM from" CssClass="col-sm-4" runat="server"></asp:Label>
                  
                          <div class="col-sm-3">
                       <asp:TextBox ID="txtGSMFrom" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox></div>

                        <asp:Label ID="lblTo3" Text="to" CssClass="col-sm-2" runat="server"></asp:Label>

                          <div class="col-sm-3">
                        <asp:TextBox ID="txtGSMTo" runat="server" CssClass="form-control" Height="27px" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox></div>
                        </div>
                  </div>
                      </div>                          
                            
                <div class="col-md-12">                    
                    <div class="box-primary">                                    
                        <table width="100%">
                            <tr>
                                <td style="width:150px">
                        <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" AutoCompleteType="Disabled" EnableViewState="false"
            placeholder="Enter here to search" ToolTip="Enter here to search"></asp:TextBox>
                                    </td>
                                <td>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="srch" runat="server" OnClick="srch_Click" ImageUrl="images/search-button.png" Width="150px" ToolTip="Click to Search" />
                                    </td>
                                <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                                    </td>
                                <td>
                                    
                                </td>
                                <td style="float:right">                                    
                                    <asp:Button ID="btnExit" Text="Exit" runat="server" class="btn-default" style="width: 100px; " OnClick="btnExit_Click" />
                                </td>
                            </tr>
                            </table>
                    <div class="lbBody" id="gridDiv" style="color: White; height:420px; overflow:hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">                        
                    <asp:GridView ID="sg1" runat="server"
                                        OnSelectedIndexChanged="sg1_SelectedIndexChanged" Width="1300px" 
                                        Style="font-family: Arial, Helvetica, sans-serif; font-size: small" 
                                        OnRowDataBound="sg1_RowDataBound" OnRowCreated="sg1_RowCreated"
                                        AllowSorting="true" OnSorting="sg1_Sorting" >
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />                                        
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EmptyDataTemplate>
                                            <asp:Image ID="imgdata" runat="server" ImageUrl="images/nodata.gif" AlternateText="No Data Exist" />
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                        </div>
                </div>
                    </div>
                  
                <div class="col-md-12" style="display:none">
              <div class="form-group">                  
                   <asp:TextBox ID="txtlblPaper" runat="server" CssClass="form-control" Height="27px" MaxLength="10" ReadOnly="True"></asp:TextBox>                                  
               </div></div>    
                
                </div>         
         </section>

        <asp:HiddenField ID="hfval" runat="server" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:Button ID="btniBox" runat="server" OnClick="btniBox_Click" Style="display: none" />

        <asp:HiddenField ID="hfFormWidth" runat="server" />
        <asp:HiddenField ID="hfFormHeight" runat="server" />
    </form>
</body>
</html>
