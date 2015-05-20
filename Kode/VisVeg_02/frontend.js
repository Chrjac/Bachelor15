
//Henter Google Autocomplete til tekstfeltene for start, stopp og via.

var inpen = document.getElementById('start');
var inpto = document.getElementById('end');
var via = document.getElementById('text1');
var options = {

    types: ['geocode'],
    componentRestrictions: { country: 'nor' }
    

};

autocomplete = new google.maps.places.Autocomplete(inpen, options);
autocomplete = new google.maps.places.Autocomplete(inpto, options);
autocomplete = new google.maps.places.Autocomplete(via, options);



var geocoder;
var map;
var m;
var m1;
var m2;
var m3;

var directionsDisplay;
var directionsService = new google.maps.DirectionsService();
var map;
var obj;
var opts = {
    lines: 13,
    length: 20,
    width: 10,
    radius: 30,
    corners: 1,
    rotate: 0,
    direction: 1,
    color: '#000',
    speed: 1,
    trail: 60,
    shadow: false,
    hwaccel: false,
    className: 'spinner',
    zIndex: 2e9,
    top: '50%',
    left: '50%'
};

//Initialiserer Google Maps.


function initialize() {
    directionsDisplay = new google.maps.DirectionsRenderer();
    var geilo = new google.maps.LatLng(60.53377649999999, 8.208952299999964);
    var mapOptions = {
        zoom: 5,
        center: geilo
    };
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    directionsDisplay.setMap(map);
}

//Funksjon som kjøres når brukeren trykker "beregn". 
//Sjekker om brukeren har fylt inn start, stopp og ev. glemt tekst i viapunk-feltet
//Kjører loading-animasjon

function calcRoute() {

    window.starttext = document.getElementById('start').value;
    window.stopptext = document.getElementById('end').value;
    window.viatext = document.getElementById('text1').value;

    if ((window.starttext == '') || (window.stopptext == '')){
        document.getElementById('ErrorMessage').innerHTML = 'Fyll inn start og stopp';
        return false;
    }

    else if (window.viatext != '') {
        document.getElementById('ErrorMessage').innerHTML = 'Du har glemt å lagre et viapunkt';
        return false;
    
    }

    else {
        document.getElementById('ErrorMessage').innerHTML = '';
        document.getElementById('noticevia').innerHTML = '';
        getCoordinateStart(window.starttext);

    }
    document.getElementById('beregnknapp').disabled = true;
    document.getElementById('overlay').style.visibility = "visible";
    var target = document.getElementById('foo');
    target.style.zIndex = "100";
    var spinner = new Spinner(opts).spin(target);
    document.getElementById('foo').style.visibility = "visible";
    

    
    timer();
    forceTab();
}

//Funksjoner for å hente koordinatene for start og stopp

function getCoordinateStart(adresse) {

    geocoder.geocode({ 'address': adresse }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            window.startCoordinates = (results[0].geometry.location);
        } else {
            alert('Kunne ikke hente koordinater. Feilmelding: ' + status);
        }

        getCoordinatesStop(window.stopptext);
    });
}
function getCoordinatesStop(adresse) {

    geocoder.geocode({ 'address': adresse }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            window.stopCoordinates = (results[0].geometry.location);
        } else {
            alert('Kunne ikke hente kordinater. Feilmelding: ' + status);
        }

        add(window.startCoordinates.toString(), window.stopCoordinates.toString());
    });
}

//Henter ut koordnatene. Deler (splitter) koordinatene i longitude og latitude. 
//Fjerner parenteser.

function add(startCoordinates, stopCoordinates) {
    var text = startCoordinates.replace(/[\(\)]/g, '').split(',');
    var text1 = stopCoordinates.replace(/[\(\)]/g, '').split(',');

    var latitudeStart = parseFloat(text[0]);
    var longitudeStart = parseFloat(text[1]);

    var latitudeStop = parseFloat(text1[0]);
    var longitudeStop = parseFloat(text1[1]);

    var startConverted = latlongToUTM1(longitudeStart, latitudeStart);
    var stoppConverted = latlongToUTM1(longitudeStop, latitudeStop);

    sendtoAjax(startConverted, stoppConverted);
}


//Henter koordinater for viapunkter.
window.counter = 0;
function via1() {
    window.viatext = document.getElementById('text1').value;
    var viapunkt = document.getElementById('text1');
        geocoder.geocode({ 'address': viapunkt.value }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                window.m2 = (results[0].geometry.location);
                //console.log('Jeg er viapunkt fra tekst1 ' + m2);
            } else {
                console.log('Kunne ikke hente kordinater. Feilmelding: ' + status);
            }



            var test2 = window.m2.toString();


            var text2 = test2.replace(/[\(\)]/g, '').split(',');


            window.latitude2 = parseFloat(text2[0]);
            window.longitude2 = parseFloat(text2[1]);


            toUTMVia();
        });

        
        counter++;
       
        checkViaButton();

}

//Sjekker om knappen er trykket 5 ganger. Blir fjernet om telleren er satt til 5.

function checkViaButton() {

   
    var buttonplus = document.getElementById('buttonplus');
    var notice = document.getElementById('noticevia');

    if (counter == 5) {

        buttonplus.style.display = 'none';
        notice.innerHTML = 'Maks antall viapunkter lagt til';
    }
}

//Sletter alle viapunkter. Dersom knappen for å legge til er fjernet, blir den lagt til.

function deleteVias() {

    viaarray.length = 0;
    document.getElementById('sortable').innerHTML = '';
    counter = 0;
    var buttonplus = document.getElementById('buttonplus');

    if (buttonplus.style.display == 'none') {

        buttonplus.style.display = 'inline-block';
        document.getElementById('noticevia').innerHTML = '';
    }

}

//Henter alle stedsnavn fra ul-listen over viapunkter. Legger de i et array.

function saveValue() {
    phrases = [];
    phrase = '';
    $('#sortable').each(function () {

        $(this).find('li').each(function () {
            var current = $(this);
            if (current.children().size() > 0) {
                return true;
            }
            phrase += $(this).text() + ':';
        });
        phrases.push(phrase);
    });

    var fras = phrases.toString();

    window.frase = fras.split(':');

    putnewArray();
}

//skriver ut alle koordinater og trekker kjøreruta ved hjelp av polyline. 
function plot(start, stopp, array, distance) {
    document.getElementById('TabView').style.pointerEvents = "all";
    console.log(array);
    console.log(array[0].Item1);

    //beregner zoomnivå for kart
    var arrcount = array.length;
    var arr1 = arrcount / 2;
    var arr2 = Math.round(arr1);
    var Zoom;
    if (distance < 10)
    {
        Zoom = 13;
    }
    else if(distance < 40)
    {
        Zoom = 10;
    }
    else if (distance < 75) {
        Zoom = 9;
    }
    else if (distance < 100) {
        Zoom = 8;
    }
    else if (distance < 500) {
        Zoom = 7;
    }
    else if (distance < 1000) {
        Zoom = 5;
    }
    else {
        Zoom = 4;
    }

    document.getElementById('map-canvas').style.visibility = 'visible';
    var mapOptions = {
        zoom: Zoom,
        preserveViewport: false,
        
        center: new google.maps.LatLng(array[arr2].Item1, array[arr2].Item2),
    };

    var map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    var drivingCoordinates = [

    ];


    for (var i = 0; i < array.length; i++) {
        drivingCoordinates.push(new google.maps.LatLng(array[i].Item1, array[i].Item2));
    }
    //tilpasser polyline
    var drivingPath = new google.maps.Polyline({
        path: drivingCoordinates,
        geodesic: true,
        preserveViewport: false,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 4
    });


    drivingPath.setMap(map);
    document.getElementById('map-canvas').style.visibility = 'visible';
    fade_out();
    document.getElementById('overlay').style.visibility = "hidden";
    document.getElementById('foo').style.visibility = "hidden";
    
    
}
//animasjon når ruten beregnes
function Click() {
    var target = document.getElementById('foo');
    var spinner = new Spinner(opts).spin(target);
    
}

//viser bruksanvisning
function popitup(url) {
    newwindow = window.open(url, 'name', 'height=300,width=550');
    if (window.focus) { newwindow.focus() }
    return false;
}


google.maps.event.addDomListener(window, 'load', initialize);
