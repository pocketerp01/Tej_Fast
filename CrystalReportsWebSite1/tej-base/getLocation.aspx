<%@ Page Language="C#" AutoEventWireup="true" Inherits="fin_sfiles_getLocation" CodeFile="getLocation.aspx.cs" %>

<html>
<head>
    <title></title>

    <link href="../tej-base/Styles/fin.css" rel="stylesheet" />

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=false">  
    </script>

    <script type="text/javascript">
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success);
        } else {
            alert("There is Some Problem on your current browser to get Geo Location!");
        }

        function success(position) {
            var lat = position.coords.latitude;
            var long = position.coords.longitude;
            var city = position.coords.locality;
            var LatLng = new google.maps.LatLng(lat, long);
            var mapOptions = {
                center: LatLng,
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var map = new google.maps.Map(document.getElementById("MyMapLOC"), mapOptions);
            var marker = new google.maps.Marker({
                position: LatLng,
                title: "<div style = 'height:60px;width:200px'><b>Your location:</b><br />Latitude: "
                            + lat + +"<br />Longitude: " + long
            });

            marker.setMap(map);
            var getInfoWindow = new google.maps.InfoWindow({
                content: "<b>Your Current Location</b><br/> Latitude:" +
                                        lat + "<br /> Longitude:" + long + ""
            });

            document.getElementById("txtLatitude").value = lat;
            document.getElementById("txtLongitude").value = long;

            GetAddress();

            getInfoWindow.open(map, marker);
        }

        function GetAddress() {
            var lat = parseFloat(document.getElementById("txtLatitude").value);
            var lng = parseFloat(document.getElementById("txtLongitude").value);
            var latlng = new google.maps.LatLng(lat, lng);
            var addr = "";
            var geocoder = geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        document.getElementById("txtAddress").value = results[1].formatted_address;
                        addr = results[1].formatted_address;
                        alert(addr);

                        document.getElementById("hfLat").value = lat;
                        document.getElementById("hfLong").value = lng;
                        document.getElementById("hfAddr").value = addr;
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        function closePopup(btn) {
            $(btn, window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <br />
        <div id="MyMapLOC" style="width: 400px; height: 300px;">
        </div>
        <table class="font_css">
            <tr>
                <td>Address : </td>
                <td>
                    <textarea id="txtAddress" style="width: 300px; height: 50px;" class="font_css rounded_corners" readonly="readonly"></textarea>
                </td>
            </tr>
            <tr>
                <td>Latitude : </td>
                <td>
                    <input type="text" id="txtLatitude" style="width: 300px;" class="font_css rounded_corners" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td>Longitude : </td>
                <td>
                    <input type="text" id="txtLongitude" style="width: 300px;" class="font_css rounded_corners" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="align-items: center" align="center">
                    <button id="btnOK" runat="server" class="btnyes" style="width: 200px; height: 30px;" onserverclick="btnOK_ServerClick">Ok</button>
                </td>
            </tr>
        </table>

        <script type="text/javascript" src="../tej-base/Scripts/jquery.min.js"></script>
        <script type="text/javascript" src="../tej-base/Scripts/jquery-ui.min.js"></script>

        <asp:HiddenField ID="hfLat" runat="server" />
        <asp:HiddenField ID="hfLong" runat="server" />
        <asp:HiddenField ID="hfAddr" runat="server" />
    </form>
</body>
</html>
