var geocoder;
var map;
var obj;
function initialize() {
    geocoder = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(-34.397, 150.644);
    var mapOptions = {
        zoom: 8,
        center: latlng
    }
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
}

/* function from() {
    var address = document.getElementById(obj).value;
    geocoder.geocode({ 'address': obj }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location

            });
            var m = (results[0].geometry.location);
            console.log(m);
           // document.getElementById("start").innerHTML = m;
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}
*/



google.maps.event.addDomListener(window, 'load', initialize);


