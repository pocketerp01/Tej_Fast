<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Desktop_VN" CodeFile="Desktop_VN.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        //$(document).ready(function () { reSize(); });
        //$(window).resize(function () { reSize(); });
        //function reSize() { $("#divContent").height(($(window).height() - 120)); }

        function myFunction(buttonID) {
            document.getElementById("myDropdown").classList.toggle("show");
        }

        // Close the dropdown if the user clicks outside of it
        window.onclick = function (event) {
            if (!event.target.matches('.dropbtn')) {
                var dropdowns = document.getElementsByClassName("dropdown-content");
                var i;
                for (i = 0; i < dropdowns.length; i++) {
                    var openDropdown = dropdowns[i];
                    if (openDropdown.classList.contains('show')) {
                        openDropdown.classList.remove('show');
                    }
                }
            }
        }
    </script>
    <style>
        .roundDiv {
            -webkit-border-radius: 12px;
            -moz-border-radius: 12px;
            border-radius: 12px;
            position: relative;
            background-color: white;
            border: solid 1px #ccc;
            box-shadow: 2px 2px 4px #b4b4b4;
        }

        .roundDiv2 {
            -webkit-border-radius: 10px;
            -moz-border-radius: 10px;
            border-radius: 10px;
            position: absolute;
            top: 80px;
            border: solid 1px #ccc;
        }

        .dropbtn {
            border: none;
            cursor: pointer;
        }

            .dropbtn:hover, .dropbtn:focus {
                background-color: #2980B9;
            }

        .dropdown {
            position: relative;
            display: inline-block;
        }

        .dropdown-content {
            display: none;
            position: absolute;
            background-color: #f1f1f1;
            min-width: 160px;
            overflow: auto;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }

            .dropdown-content a {
                color: black;
                padding: 12px 16px;
                text-decoration: none;
                display: block;
            }

        .dropdown a:hover {
            background-color: #ddd;
        }

        .show {
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <section class="content">
                <div class="row">
                    <div class="roundDiv2" style="background-color: white; height: 100px; width: 97%">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8 roundDiv">
                        <div class="box-body no-padding">
                            <ul class="users-list clearfix">
                                <li style="max-width: 145px;">
                                    <img src="images/asn.png" />
                                    <a class="users-list-name" href="#">ASN</a>
                                    <span class="users-list-date">Advance Shipment Note</span>
                                </li>
                                <li style="max-width: 145px;">
                                    <img src="images/add_to_basket-512.png" class="dropbtn" onclick="myFunction(this);" />
                                    <a class="users-list-name" href="#">Purchase Module</a>
                                    <span class="users-list-date">My Orders</span>
                                    <div id="myDropdown" class="dropdown-content">
                                        <a href="#">All Purchase Orders</a>
                                        <a href="#">Pending Purchase Order</a>
                                        <a href="#">Order vs Despatch Material</a>
                                    </div>
                                </li>
                                <li style="max-width: 145px;">
                                    <img src="images/mrr.png" />
                                    <a class="users-list-name" href="#">MRR Module</a>
                                    <span class="users-list-date">Material Rcvd</span>
                                </li>
                                <li style="max-width: 145px;">
                                    <img src="images/qa.png" />
                                    <a class="users-list-name" href="#">Quality Module</a>
                                    <span class="users-list-date">Rcvd Quality</span>
                                </li>
                                <li style="max-width: 145px;">
                                    <img src="images/account.jpg" />
                                    <a class="users-list-name" href="#">Accounts Module</a>
                                    <span class="users-list-date">Rcvd Quality</span>
                                </li>
                                <li style="max-width: 145px;">
                                    <img src="images/others.gif" />
                                    <a class="users-list-name" href="#">Others</a>
                                    <span class="users-list-date">Other Reports</span>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
            </section>
        </section>
    </div>
</asp:Content>

