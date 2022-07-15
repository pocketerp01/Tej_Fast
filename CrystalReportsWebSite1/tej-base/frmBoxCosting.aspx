<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="frmBoxCosting" CodeFile="frmBoxCosting.aspx.cs" %>
<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1
        {
            width: 131px;
            margin-left: 840px;
        }

        .auto-style19
        {
            width: 192px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style20
        {
            width: 142px;
            font-size: small;
        }

        .auto-style35
        {
        }

        .auto-style36
        {
            width: 131px;
            font-size: xx-small;
        }

        .auto-style41
        {
            width: 143px;
            font-weight: bold;
            font-size: small;
            height: 24px;
        }

        .auto-style42
        {
            width: 99px;
            height: 24px;
        }

        .auto-style43
        {
            width: 131px;
            font-size: xx-small;
            height: 24px;
        }

        .auto-style44
        {
            width: 46px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style45
        {
            width: 133px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style46
        {
            margin-left: 840px;
        }

        .auto-style47
        {
            font-weight: bold;
            font-size: small;
        }

        .auto-style51
        {
            width: 131px;
            margin-left: 840px;
            height: 24px;
        }

        .auto-style54
        {
            width: 95px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style55
        {
            width: 42px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style57
        {
            width: 173px;
            font-weight: bold;
            font-size: small;
        }

        .auto-style58
        {
            width: 173px;
            font-weight: bold;
            font-size: small;
            height: 24px;
        }

        .auto-style70
        {
            width: 26px;
        }

        .auto-style72
        {
            margin-left: 840px;
            font-size: xx-small;
        }

        .auto-style74
        {
            width: 83px;
            margin-left: 840px;
            height: 24px;
        }

        .auto-style75
        {
            width: 77px;
            margin-left: 840px;
            height: 24px;
        }

        .auto-style76
        {
            width: 79px;
            margin-left: 840px;
            height: 24px;
        }

        .auto-style77
        {
            font-size: xx-small;
            width: 87px;
        }

        .auto-style78
        {
            width: 87px;
            margin-left: 840px;
        }

        .auto-style79
        {
            width: 87px;
            margin-left: 840px;
            height: 24px;
        }

        .auto-style80
        {
            width: 87px;
            margin-left: 840px;
            font-weight: bold;
        }

        .auto-style81
        {
            width: 99px;
            margin-left: 840px;
        }

        .auto-style82
        {
            width: 99px;
            margin-left: 840px;
            font-size: xx-small;
        }

        .auto-style84
        {
            width: 75px;
            height: 24px;
        }

        .auto-style85
        {
            width: 75px;
            margin-left: 840px;
        }

        .auto-style86
        {
            font-size: xx-small;
        }
        .auto-style87
        {
            width: 143px;
            font-weight: bold;
            font-size: small;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            calculateSum();
        });
        function calculateSum()
        {
            var purchase=0;var minqty=0; var madhvi = 0; var Splitt=[100];var WholeString=''; var Le=0 ;var ConvertToWhole=0 ; var Substrng=0;var ChangeStr=0; var Final=0;var strConvert=0;
            var element = document.getElementById("ContentPlaceHolder1_cmbBoxTypes");
            var op = element.options[element.selectedIndex].value;
            //alert(op);
            //Universal
            if (element.options[element.selectedIndex].value == "UNIVERSAL")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero( fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value *1)) * 2 +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
      
            //Over Flap Rac
            if (element.options[element.selectedIndex].value == "OVER FLAP RAC")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1)*2) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value *1)) * 2 +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
       
            //Half Rac
            if (element.options[element.selectedIndex].value == "HALF RAC")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1)/2) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value *1)) * 2 +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
       

            //OVER FLAP HALF RAC
            if (element.options[element.selectedIndex].value == "OVER FLAP HALF RAC")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1)) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value *1)) * 2 +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
      
            //Sleeve
            if (element.options[element.selectedIndex].value == "SLEEVE")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value *1)) * 2 +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
      
            //Tray
            if (element.options[element.selectedIndex].value == "TRAY")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1)*2) + fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value *1)*2) +fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
       
            //Sheet
            if (element.options[element.selectedIndex].value == "SHEET")
            {
                document.getElementById('ContentPlaceHolder1_txtDeckle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtWidth').value * 1)) +  fill_zero(document.getElementById('ContentPlaceHolder1_txtReel').value * 1)).toFixed(3) ; 
              //  if(document.getElementById('ContentPlaceHolder1_cmbBoxTypes').options[t.selectedIndex].text=='SHEET'){ 
                    document.getElementById('ContentPlaceHolder1_txtCutSize').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtLength').value * 1)) + fill_zero(document.getElementById('ContentPlaceHolder1_txtCut').value * 1)).toFixed(3) ;
            }
       

            document.getElementById('ContentPlaceHolder1_txtSheet').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtDeckle').value * 1)  * fill_zero(document.getElementById('ContentPlaceHolder1_txtCutSize').value *1))/ 1000000).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtWTop').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtTop').value * 1)  * fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value *1))/ 1000).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtWMiddle').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMiddle').value * 1)  * fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value *1))/ 1000).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtWBottom').value = fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtBottom').value * 1)  * fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value *1))/ 1000).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtWFlute').value = fill_zero(((fill_zero(document.getElementById('ContentPlaceHolder1_txtFluteB').value * 1)  + fill_zero(document.getElementById('ContentPlaceHolder1_txtFluteA').value *1))*1.5)*fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value *1)/ 1000).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtSWeight').value =  fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWTop').value * 1)  + fill_zero(document.getElementById('ContentPlaceHolder1_txtWMiddle').value *1) +fill_zero(document.getElementById('ContentPlaceHolder1_txtWBottom').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtWFlute').value *1)).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtBS').value =fill_zero(((fill_zero(document.getElementById('ContentPlaceHolder1_txtBFTopRate').value * 1)  * fill_zero(document.getElementById('ContentPlaceHolder1_txtTop').value *1))/1000) +fill_zero(((document.getElementById('ContentPlaceHolder1_txtBFMiddleRate').value *1)* fill_zero(document.getElementById('ContentPlaceHolder1_txtMiddle').value *1))/1000) +fill_zero(((document.getElementById('ContentPlaceHolder1_txtBFBottomRate').value *1)* fill_zero(document.getElementById('ContentPlaceHolder1_txtBottom').value *1))/1000)).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtMin').value = fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtMinQty').value*1) / fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value*1)) ;
            //document.getElementById('ContentPlaceHolder1_txtPurchase').value = fill_zero( fill_zero(document.getElementById('ContentPlaceHolder1_txtMinQty').value*1) / fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value*1)).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtGSM').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtTop').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtMiddle').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtBottom').value * 1)+((fill_zero(document.getElementById('ContentPlaceHolder1_txtFluteB').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtFluteA').value * 1))*1.45)).toFixed(3);

            // vipin
            WholeString=(document.getElementById('ContentPlaceHolder1_txtMin').value*1);
            Splitt=WholeString.toString().split('.');
            Le=Splitt[0].length;
            if (Le > 1){ 
                ConvertToWhole=Le-2; if(ConvertToWhole==0) { madhvi = 100 } else {
                    Substrng=WholeString.toString().substring(0,ConvertToWhole);
                    strConvert=parseInt(Substrng);
                    ChangeStr=strConvert+1;
                    madhvi = ChangeStr+'00'} }
            else if (Le ==1){
                ChangeStr=1;
                madhvi = 100};
            if( madhvi - WholeString == 100) {  if ( WholeString >= 100) { document.getElementById('ContentPlaceHolder1_txtMin').value = WholeString; }   }  
            else { document.getElementById('ContentPlaceHolder1_txtMin').value = madhvi;  }
            // If condition for Purcase and min
            purchase=(document.getElementById('ContentPlaceHolder1_txtPurchase').value*1);
            //minqty = (document.getElementById('ContentPlaceHolder1_txtMin').value * 1);
            minqty = (document.getElementById('ContentPlaceHolder1_txtMinQty').value * 1);
            // originally
            //if(purchase<=minqty){
            //    document.getElementById('ContentPlaceHolder1_txtProcessRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value *1)/100)*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
            //    document.getElementById('ContentPlaceHolder1_txtBoardRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtBoard').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
            //    document.getElementById('ContentPlaceHolder1_txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtPrinting').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
            //}
            //else {
            //    document.getElementById('ContentPlaceHolder1_txtProcessRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value *1)/100)*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
            //    document.getElementById('ContentPlaceHolder1_txtBoardRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtBoard').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
            //    document.getElementById('ContentPlaceHolder1_txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtPrinting').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
            //};

            document.getElementById('ContentPlaceHolder1_txtDieRate').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtDie').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value *1)).toFixed(3) ;

            document.getElementById('ContentPlaceHolder1_txtWaterRate').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtWater').value *1)).toFixed(3) ;
            document.getElementById('ContentPlaceHolder1_txtStitchingRate').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtStitching').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value *1)).toFixed(3) ;
            //document.getElementById('ContentPlaceHolder1_txtProfitRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtWastageRate').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtProfit').value *1)/100) ;
            document.getElementById('ContentPlaceHolder1_txtTapingRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtHeight').value * 1)/1000)*4*fill_zero(document.getElementById('ContentPlaceHolder1_txtTaping').value * 1)).toFixed(3);
            //vip = vip + "document.getElementById('ContentPlaceHolder1_txtRateTop').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWTop').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtBFTopRate').value * 1)).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtRateTop').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWTop').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtBfTopG').value * 1)).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtRateMiddle').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWMiddle').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtBfMiddleG').value * 1)).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtRateBottom').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWBottom').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtBfBottomG').value * 1)).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtRateFlute').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtWFlute').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtBfFluteG').value * 1)).toFixed(3);

            //document.getElementById('ContentPlaceHolder1_txtMaterial').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtWTop').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtRateTop').value * 1))+(fill_zero(document.getElementById('ContentPlaceHolder1_txtWMiddle').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtRateMiddle').value * 1))+(fill_zero(document.getElementById('ContentPlaceHolder1_txtWBottom').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtRateBottom').value * 1))+(fill_zero(document.getElementById('ContentPlaceHolder1_txtWFlute').value * 1)*fill_zero(document.getElementById('ContentPlaceHolder1_txtRateFlute').value * 1))).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtMaterial').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtRateTop').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtRateMiddle').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtRateBottom').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtRateFlute').value * 1)).toFixed(3);

            //replaced
           // alert(purchase);
           // alert(minqty);
            //alert(document.getElementById('ContentPlaceHolder1_txtMaterial').value);
            //alert(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1);
            //alert(document.getElementById('ContentPlaceHolder1_txtMin').value * 1);
           // alert(fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value * 1) / 100) * fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1) / fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3));

            if(purchase<=minqty){
                document.getElementById('ContentPlaceHolder1_txtProcessRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value *1)/100)*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtBoardRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtBoard').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtPrinting').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3) ;
            }
            else {
                document.getElementById('ContentPlaceHolder1_txtProcessRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value *1)/100)*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtBoardRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSWeight').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtBoard').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
                document.getElementById('ContentPlaceHolder1_txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtSheet').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtPrinting').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)/fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1)).toFixed(3) ;
            };
           // alert(purchase);
           // alert(minqty);
           // alert(document.getElementById('ContentPlaceHolder1_txtMaterial').value);
           // alert("DD");
           // alert(document.getElementById('ContentPlaceHolder1_txtMin').value * 1);
           // alert(fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value * 1) / 100) * fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1) / fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3));

           // if (cmbBoxTypes.Text == "UNIVERSAL")
            //{
            //   document.getElementById('ContentPlaceHolder1_txtProfitRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtProfit').value *1)/100).toFixed(3);
            //}
            //else
            //{
            document.getElementById('ContentPlaceHolder1_txtProfitRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtBoardRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPrintingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtDieRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtWaterRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtStitchingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtTapingRate').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPacking').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtAny').value * 1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtProfit').value *1)/100).toFixed(3);
            //}

            //vip = vip + "document.getElementById('ContentPlaceHolder1_txtProfitRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtProfit').value *1)/100).toFixed(3);

            document.getElementById('ContentPlaceHolder1_txtFreightRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtProfitRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtBoardRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPrintingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtDieRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtWaterRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtStitchingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtTapingRate').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPacking').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtAny').value * 1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtFreight').value *1)/100).toFixed(3) ;


            document.getElementById('ContentPlaceHolder1_txtPymtRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProfitRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtBoardRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPrintingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtDieRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtWaterRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtStitchingRate').value *1)  + fill_zero(document.getElementById('ContentPlaceHolder1_txtFreightRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtTapingRate').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPacking').value * 1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtAny').value * 1))*2/100 *fill_zero(document.getElementById('ContentPlaceHolder1_txtPymt').value *1)).toFixed(3) ;



            //vip = vip + "document.getElementById('ContentPlaceHolder1_txtBasic').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtPymtRate').value *1)+ (fill_zero(document.getElementById('ContentPlaceHolder1_txtPacking').value *1)/100)+ fill_zero(document.getElementById('ContentPlaceHolder1_txtAny').value *1))).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtBasic').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtPymtRate').value *1)+ (fill_zero(document.getElementById('ContentPlaceHolder1_txtPacking').value *1))+ fill_zero(document.getElementById('ContentPlaceHolder1_txtAny').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProfitRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtProcessRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtBoardRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtPrintingRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtDieRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtWaterRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtStitchingRate').value *1)  + fill_zero(document.getElementById('ContentPlaceHolder1_txtFreightRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtTapingRate').value * 1)).toFixed(3);

            document.getElementById('ContentPlaceHolder1_txtExciseRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtBasic').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtExcise').value *1))/100).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtSalesRate').value =fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtBasic').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtExciseRate').value *1))*fill_zero(document.getElementById('ContentPlaceHolder1_txtSales').value *1)/100).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtTotal').value =fill_zero(fill_zero(document.getElementById('ContentPlaceHolder1_txtBasic').value * 1) + fill_zero(document.getElementById('ContentPlaceHolder1_txtExciseRate').value *1)+fill_zero(document.getElementById('ContentPlaceHolder1_txtSalesRate').value *1)).toFixed(3);
            //alert("last");
            //alert(purchase);
            //alert(minqty);
            //alert(document.getElementById('ContentPlaceHolder1_txtMaterial').value);
           // alert(document.getElementById('ContentPlaceHolder1_txtProcessRate').value * 1);
            //alert(document.getElementById('ContentPlaceHolder1_txtMin').value * 1);
            //alert(fill_zero((fill_zero(document.getElementById('ContentPlaceHolder1_txtMaterial').value * 1) * fill_zero(document.getElementById('ContentPlaceHolder1_txtProcess').value * 1) / 100) * fill_zero(document.getElementById('ContentPlaceHolder1_txtMin').value * 1) / fill_zero(document.getElementById('ContentPlaceHolder1_txtPurchase').value * 1)).toFixed(3));

        }

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
            

            function isDecimalKey(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode != 46 && charCode > 31
                  && (charCode < 48 || charCode > 57))
                    return false;

                else {
                    return true;
                }
            }

        </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
           <%-- <h1>Costing
            </h1>--%>
             <table style="width: 100%">
                <tr>
                    <td>
           <%-- <div class="box-footer">--%>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdnew" onserverclick="cmdnew_Click" accesskey="N"><u>N</u>ew</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdedit" onserverclick="cmdedit_Click" accesskey="i">Ed<u>i</u>t</button>
                <%--</form>--%>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="btnsave" onserverclick="btnsave_Click" accesskey="s"><u>S</u>ave</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdprint" onserverclick="cmdprint_Click" accesskey="P"><u>P</u>rint</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmddel" onserverclick="cmddel_Click" accesskey="l">De<u>l</u>ete</button>
                <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                <button type="submit" class="btn btn-info" style="width: 100px;" runat="server" id="cmdexit" accesskey="X" onserverclick="cmdexit_Click">E<u>x</u>it</button>
           </td>
                 <td>
                     <asp:Label ID="lblheader" Text="Costing" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                 </td>
                    </tr>   
                 </table>     <%--</div>--%>
    
                
            
        </section>
        <section class="content">
            <div class="row">
                <!-- left column -->

                 <div class="col-md-12">
                    <div>

                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                        <div class="box-body">
                            <div class="form-group">

                                <table>
                                    <tr>
                                        <td class="auto-style87" colspan="2">Basic Price</td>
                                        <td class="auto-style1" colspan="3">
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                            <asp:TextBox ID="txtBasic" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                       <button id="btnInvoice" runat="server" onserverclick="btnInvoice_ServerClick" >Last Invoice Price</button>
                                             <button id="btnLast" runat="server" onserverclick="btnLast_ServerClick" >Last SO Price</button>
                                        </td>
              <td class="auto-style45">Minimum Order Qty </td>
                                        <td class="auto-style44">
                                            <asp:TextBox ID="txtMinQty" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" Width="50px" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td class="auto-style85">
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                            <asp:TextBox ID="txtMin" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" BackColor="Silver" MaxLength="30" Width="70px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td class="auto-style82" style="font-size:x-small;">
                                            PER PPC</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style87" colspan="2">Excises</td>
                                        <td class="auto-style85">
                                            <asp:TextBox ID="txtExcise" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style81">
                                            <asp:TextBox ID="txtExciseRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="90px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">%</td>
                                        <td class="auto-style19" colspan="2">Purchase Order Qty</td>
                                        <td class="auto-style1" colspan="2">
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                            <asp:TextBox ID="txtPurchase" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style41" colspan="2">Sales Tax</td>
                                        <td class="auto-style84">
                                            <asp:TextBox ID="txtSales" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style42">
                                            <asp:TextBox ID="txtSalesRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="90px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style43" style="font-size:x-small;">%</td>
                                         <td class="auto-style19" colspan="2">Total Of Add On </td>
                                        <td class="auto-style1" colspan="2">
                                            <%--</form>--%>
                                        <asp:TextBox ID="txtGrdTotal" runat="server"  ReadOnly="True" ></asp:TextBox> </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style87" colspan="2">Total Price</td>
                                        <td class="auto-style1" colspan="3">
                                            <asp:TextBox ID="txtTotal" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                        </td>
                                    </tr>
                                   
                                  
                                </table>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>

                        <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Box Master</td>
                                        <td class="auto-style1" colspan="6">
                                            <asp:DropDownList ID="cmbBoxTypes" runat="server" OnSelectedIndexChanged="cmbBoxTypes_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="auto-style54">Party Name</td>
                                        <td class="auto-style55">
                                            <button id="btnParty" runat="server" onserverclick="btnParty_Click" width="16px">!</button>
                                            
                                        </td>
                                        <td class="auto-style35" colspan="6">

                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>

                                            <asp:TextBox ID="txtPCode" runat="server" Style="margin-bottom: 2px" Width="65px" MaxLength="10" ReadOnly="True"></asp:TextBox>

                                            <asp:TextBox ID="txtParty" runat="server" Style="margin-bottom: 2px" ReadOnly="True" Width="184px" ></asp:TextBox>
                                        </td>



                                    </tr>


                                    <tr>
                                        <td class="auto-style54">Item Name&nbsp;</td>
                                        <td class="auto-style55">
                                            <button id="btnItem" runat="server" onserverclick="btnItem_Click" width="16px">!</button>
                                        </td>
                                        <td class="auto-style46" colspan="6">
                                            <asp:TextBox ID="txtICode" runat="server" Style="margin-bottom: 2px" onKeypress="return isDecimalKey(event);" MaxLength="9" Width="65px" ReadOnly="True"></asp:TextBox>
                                            <%--</form>--%><asp:TextBox ID="txtItem" runat="server" Style="margin-bottom: 2px" MaxLength="30" Width="184px" OnTextChanged="txtItem_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        </td>

                                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                    </tr>

                                    <tr>
                                        <td class="auto-style20" colspan="2">
                                            <label for="exampleInputEmail1">Length</label></td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtLength" runat="server" Style="margin-bottom: 2px" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                        </td>

                                        <td class="auto-style72" style="font-size:x-small;">
                                            MM</td>

                                        <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>

                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Width</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtWidth" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>

                                        <td class="auto-style72" style="font-size:x-small;">
                                            MM</td>

                                        <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>

                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">
                                            <label for="exampleInputEmail1">Height</label></td>
                                        <td class="auto-style75">
                                            <asp:TextBox ID="txtHeight" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtCST" runat="server" placeholder="Sales Tax/CST No" class="form-control" style="width:100px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style72" colspan="3" style="font-size:x-small;">
                                            MM</td>
                                        
                                        
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">ID Or OD</td>
                                        <td class="auto-style74" colspan="3">
                                            <%--</form>--%>
                                            <asp:DropDownList ID="cmbID" runat="server" Width="70px">
                                                <asp:ListItem Value="ID"></asp:ListItem>
                                                <asp:ListItem>OD</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            MM</td>
                                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Double Or Single Part</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtDouble" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            NUMBERS</td>
                                        <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Top Layer</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtTop" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            GSM + BF</td>
                                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Flute B</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtFluteB" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<td class="auto-style31" style="right: 100px">Total&nbsp;
                                                        
                                                    </td>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            GSM + BF</td>
                                        <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Middle Layer</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtMiddle" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            GSM + BF</td>
                                        <%--</form>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Flute A</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtFluteA" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            GSM + BF</td>
                                        <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Bottom layer</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtBottom" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            GSM + BF</td>
                                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Reel Trim Margin</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtReel" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            MM</td>
                                        <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Cut Trim Margin</td>
                                        <td class="auto-style74" colspan="3">
                                            <asp:TextBox ID="txtCut" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style72" style="font-size:x-small;">
                                            MM</td>
                                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                    </tr>
                                    <tr>
                                        <td class="auto-style47" colspan="2">Deckle Size</td>
                                        <td class="auto-style75">
                                            <asp:TextBox ID="txtDeckle" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<td class="auto-style31" style="right: 100px">Total&nbsp;
                                                        
                                                    </td>--%>
                                        </td>
                                        <td  colspan="4" class="auto-style47" >
                                            Cut Size</td>
                                        <td class="auto-style51">
                                            <asp:TextBox ID="txtCutSize" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" ReadOnly="True" BackColor="Silver" MaxLength="30" Width="82px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="auto-style47" colspan="2">Sheet Area</td>
                                        <td class="auto-style76" colspan="2">
                                            <asp:TextBox ID="txtSheet" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style47" colspan="2">
                                            Weight Top</td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:TextBox ID="txtWTop" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" onkeyup="calculateSum();" Width="82px"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="auto-style47" colspan="2">Weight Middle</td>
                                        <td class="auto-style76" colspan="2">
                                            <asp:TextBox ID="txtWMiddle" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style47" colspan="2">
                                            Weight Bottom</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtWBottom" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" onkeyup="calculateSum();" Width="82px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="auto-style47" colspan="2">Weight Flute</td>
                                        <td class="auto-style76" colspan="2">
                                            <asp:TextBox ID="txtWFlute" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style47" colspan="2">
                                            Sheet Weight</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSWeight" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" ReadOnly="True" Width="82px"></asp:TextBox>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td class="auto-style54">BF Top&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="auto-style55">
                                            <button id="btnBFTop" runat="server" onserverclick="btnBFTop_Click" width="16px">!</button>
                                        </td>
                                        <td class="auto-style35" colspan="6">

                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>

                                            <asp:TextBox ID="txtBFTop" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" ReadOnly="True" Width="188px"></asp:TextBox>
                                            
                                            <asp:TextBox ID="txtBFTopRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                            <asp:TextBox ID="txtBfTopG" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style54">BF Middle&nbsp;&nbsp;

                                        </td>
                                        <td class="auto-style55">

                                            <button id="btnBFMiddle" runat="server" onserverclick="btnBFMiddle_Click" width="16px">!</button>

                                        </td>
                                        <td class="auto-style35" colspan="6">

                                            <%--</form>--%>

                                            <asp:TextBox ID="txtBFMiddle" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" ReadOnly="True" Width="188px"></asp:TextBox>
                                            
                                            <asp:TextBox ID="txtBFMiddleRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                            <asp:TextBox ID="txtBfMiddleG" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style54">BF Bottom&nbsp;&nbsp;
                                        </td>
                                        <td class="auto-style55">
                                            <button id="btnBFBottom" runat="server" onserverclick="btnBFBottom_Click" width="16px">!</button>
                                        </td>
                                        <td class="auto-style35" colspan="6">

                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>

                                            <asp:TextBox ID="txtBFBottom" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" ReadOnly="True" Width="188px"></asp:TextBox>
                                            <asp:TextBox ID="txtBFBottomRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                            <asp:TextBox ID="txtBfBottomG" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style54">
                                            <b>BF Flutes&nbsp;</b>&nbsp;
                                        </td>
                                        <td class="auto-style55">
                                            <button id="btnBFFlutes" runat="server" onserverclick="btnBFFlutes_Click" width="16px">!</button>
                                        </td>
                                        <td class="auto-style35" colspan="6">

                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>

                                            <asp:TextBox ID="txtBFFlutes" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" ReadOnly="True" Width="188px"></asp:TextBox>
                                            
                                            <asp:TextBox ID="txtBFFluteRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True" Height="21px"></asp:TextBox>

                                            <asp:TextBox ID="txtBfFluteG" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" Width="50px" BackColor="Silver" ReadOnly="True"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <%--</form>--%>
                                </table>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                    </div>
                </div>


                <div class="col-md-6">
                    <div>

                        <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                   
                                    <tr>
                                        <td class="auto-style57">BS +-10%                                     
                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtBS" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style80">&nbsp;</td>
                                        <td class="auto-style1">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">GSM +-5 %</td>
                                        <td colspan="2">
                                            <%--</form>--%>
                                            <asp:TextBox ID="txtGSM" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style1">&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style57">ECT +_10%</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtECT" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style1">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">BCT +-10%</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtBCT" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%--<td class="auto-style31" style="right: 100px">Total&nbsp;
                                                        
                                                    </td>--%>
                                        </td>
                                        <td class="auto-style1">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style58">COBB Values</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCOBB" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style51"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style58">Moisture</td>
                                        <td>
                                            <asp:TextBox ID="txtMoisture" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style43">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Rates Top</td>
                                        <td>
                                            <asp:TextBox ID="txtRateTop" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" BackColor="Silver" Width="70px" ReadOnly="True"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Rates Middle</td>
                                        <td>
                                            <asp:TextBox ID="txtRateMiddle" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" BackColor="Silver" Width="70px" ReadOnly="True"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Rates Bottom</td>
                                        <td>
                                            <asp:TextBox ID="txtRateBottom" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" BackColor="Silver" Width="70px" ReadOnly="True"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Rates Flute</td>
                                        <td>
                                            <asp:TextBox ID="txtRateFlute" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" BackColor="Silver" Width="70px" ReadOnly="True"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Material Cost</td>
                                        <td>
                                            <asp:TextBox ID="txtMaterial" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" onkeyup="calculateSum();" MaxLength="30" BackColor="Silver" Width="70px" ReadOnly="True"></asp:TextBox>
                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style77" style="font-size:x-small;">
                                            RUPEES PER KG</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%><%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                    <tr>
                                        <td class="auto-style57">Process Wastage</td>
                                        <td class="auto-style70">
                                            <%--</form>--%>
                                            <asp:TextBox ID="txtProcess" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtProcessRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px" onkeyup="calculateSum();"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">%</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Board Making Charges</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtBoard" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtBoardRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">SQM</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Printing/Slotting</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtPrinting" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<td class="auto-style31" style="right: 100px">Total&nbsp;
                                                        
                                                    </td>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtPrintingRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">SQM</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style58">Water Resistance Coating</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtWater" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style79">
                                            <asp:TextBox ID="txtWaterRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style43" style="font-size:x-small;">PER PCS</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Die Cutting</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtDie" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtDieRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">PER SQM</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Stitching Or Flap Costing</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtStitching" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--</form>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtStitchingRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">PER KG</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Taping Or Binding Cloth</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtTaping" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtTapingRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">LMTR</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style58">Packing Charges</td>
                                        <td>
                                            <asp:TextBox ID="txtPacking" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
                                        </td>
                                        <td class="auto-style86" style="font-size:x-small;">
                                            PER PCS</td>
                                        <td class="auto-style43">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Any Other Charges</td>
                                        <td>
                                            <%--</form>--%>
                                            <asp:TextBox ID="txtAny" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style86" style="font-size:x-small;">
                                            PER PCS</td>
                                        <td class="auto-style36">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Profit Margin</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtProfit" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtProfitRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">%</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Freight Charges</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtFreight" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtFreightRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">%</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style57">Payment Terms Charges</td>
                                        <td class="auto-style70">
                                            <asp:TextBox ID="txtPymt" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" MaxLength="30" onkeyup="calculateSum();" Width="70px"></asp:TextBox>
                                            <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                        </td>
                                        <td class="auto-style78">
                                            <asp:TextBox ID="txtPymtRate" runat="server" Style="margin-bottom: 2px;" onKeypress="return isDecimalKey(event);" BackColor="Silver" ReadOnly="True" MaxLength="30" Width="70px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style36" style="font-size:x-small;">%</td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--</form>--%>
                    </div>
                </div>
               
                <div class="col-md-12">
                    <div>

                        <%--<input type="text" id="txtContact" runat="server" placeholder="Contact Person" class="form-control" style="width:200px; height:30px; margin-bottom:2px;"/>--%>
                        <div class="box-body">
                            <div class="form-group">
                                <div id="order_details_grid" style="height: 250px; max-height: 250px; max-width: 1290px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <table>

                                        <tr>
                                                    <%--<td class="auto-style31" style="right: 100px">Total&nbsp;
                                                        
                                                    </td>--%>

                                                </tr>
                                        <tr>
                                            <td colspan="4">

                                                <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound"
                                                    Style="font-size: smaller;" CssClass="table table-bordered table-hover dataTable">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>A</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnadd" runat="server" CommandName="Add" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="11px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>D</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="11px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>

                                                        <%-- <asp:BoundField DataField="Acode" HeaderText="Code" ReadOnly="True">
                                <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                <asp:BoundField DataField="Aname" HeaderText="Party Name" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500px" />
                                    </asp:BoundField>--%>
                                                        <asp:BoundField DataField="Icode" HeaderText="Item Code" ReadOnly="True">
                                                            <HeaderStyle Width="1000px" />
                                                            <ItemStyle Width="1000px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Iname" HeaderText="Item Name" ReadOnly="True">
                                                            <HeaderStyle Width="1000px" />
                                                            <ItemStyle Width="1000px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>Quantity</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQty" runat="server" Width="70px" AutoPostBack="true" MaxLength="25" Text='<%#Eval("T80") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>Rate</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCol16" runat="server" Width="70px" AutoPostBack="true" Text='<%#Eval("Irate") %>' MaxLength="25" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false "  Style="text-align: right" OnTextChanged="txtCol16_TextChanged" ></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- <asp:BoundField  HeaderText="Total" ReadOnly="True">
                                    <HeaderStyle Width="500px" />
                                    <ItemStyle Width="500" />
                                    </asp:BoundField>--%>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#1797c0" ForeColor="White" Height="20px"
                                                        CssClass="GridviewScrollHeader" Font-Bold="True" />
                                                </asp:GridView>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <%--</form>--%>
                    </div>
                </div>

            </div>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="hffield"   ></button>--%>
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="popselected" runat="server" />
    <asp:HiddenField ID="hfReport" runat="server" />
    <%--<button type="submit" class="btn btn-info" style="width:100px;" runat="server" id="edmode"   ></button>--%>
</asp:Content>