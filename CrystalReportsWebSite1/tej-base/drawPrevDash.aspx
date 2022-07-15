<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="drawPrevDash" CodeFile="drawPrevDash.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align: right">
                      <%--  <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>--%>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <%--<button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>--%>
                        <%--<button type="submit" id="btnReport" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnReport_ServerClick"><u>R</u>eport</button>--%>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                </tr>
            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-sm-12">
                            <div class="form-group">

                                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
                                </asp:Timer>
                                <asp:UpdatePanel ID="Panel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DataList ID="dtList1" runat="server" RepeatDirection="Vertical" 
                                            RepeatColumns="3" onselectedindexchanged="dtList1_SelectedIndexChanged" 
                                            onitemdatabound="dtList1_ItemDataBound">
                                            <ItemTemplate>                        
                                            <asp:Button id="SelectButton" Text="View Full" CommandName="Select" runat="server"/>
                                        <div style="border-width: thick; border-style: groove; margin-left: 5px; margin-right: 5px;">
                                            Issue To : <b>
                                                <%#Eval("username") %></b>,Till :
                                            <%#Eval("endtime")%>&nbsp;&nbsp; Issue By : <b>
                                                <%#Eval("ISSUED_BY")%></b>
                                            <br />
                                            Drawing No : <b>
                                                <%#Eval("DRAWING_NAME")%></b>
                                            <br />
                                            <iframe style="width: 400px; height: 250px; position: relative; z-index: 1;" frameborder="0"
                                                id="Iframe1" runat="server" src='<%#Eval("filepath") %>'></iframe>                                
                                        </div>
                            <asp:HiddenField ID="dtHd1" runat="server" Value='<%#Eval("FSTR") %>' />
                                                <asp:HiddenField ID="hfaddr" runat="server" Value='<%#Eval("hfaddr") %>' />
                            </ItemTemplate>
                            </asp:DataList>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </section>
            </div>
    
                            
                          
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
     <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
</asp:Content>
