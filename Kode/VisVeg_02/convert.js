


function declarations() {

    DatumEqRad = [6378137.0, 6378137.0, 6378137.0, 6378135.0, 6378160.0, 6378245.0, 6378206.4, 6378388.0, 6378388.0, 6378249.1, 6378206.4, 6377563.4, 6377397.2, 6377276.3];
    DatumFlat = [298.2572236, 298.2572236, 298.2572215, 298.2597208, 298.2497323, 298.2997381, 294.9786982, 296.9993621, 296.9993621, 293.4660167, 294.9786982, 299.3247788,
    299.1527052, 300.8021499];

    Item = 0;
    //skala sentralmeridian (CM)
    k0 = 0.9996;
    //ekvatorial radius, meter
    a = DatumEqRad[Item];
    //polar utflatning
    f = 1 / DatumFlat[Item];
    //polar akse
    b = a * (1 - f);
    //eksentrisitet
    e = Math.sqrt(1 - b * b / a * a);
    //konvertere grader til radianer
    drad = Math.PI / 180;
    //latitude (breddegrad) i grader
    latd = 0;
    //lattitude (nord +, sør -)
    phi = 0;
    e0 = e / Math.sqrt(1 - e * e);
    N = a / Math.sqrt(1 - Math.pow(e * Math.sin(phi)), 2);
    T = Math.pow(Math.tan(phi), 2);
    C = Math.pow(e * Math.cos(phi), 2);
    //longitude (lengdegrad)
    lng = 0;
    //longitude av sentralmeridianen
    lngd0 = 0;
    //longitude i grader
    lngd = 0;
    M = 0;
    //x koordinat
    x = 0;
    //y koordinat
    y = 0;
    k = 1;
    //utm sone
    utmz = 33;
    //sone sentralmeridian
    zcm = 0;
}

//Funksjon som konverterer koordinater og oppretter arrays for viapunkter

window.viaarray = [];

window.onearray = [];


function toUTMVia() {

    declarations();

    k0 = 0.9996;
    b = a * (1 - f);
    e = Math.sqrt(1 - (b / a) * (b / a));

    latd0 = parseFloat(latitude2);
    lngd0 = parseFloat(longitude2);

    lngd = lngd0;
    latd = latd0;

    phi = latd * drad;
    lng = lngd * drad;
    utmz = parseFloat(33);
    latz = 0;

    if (latd > -80 && latd < 72) {
        latz = Math.floor((latd + 80) / 8) + 2;
    }

    if (latd > 72 && latd < 84) {
        latz = 21;
    }

    if (latd > 84) {
        latz = 23;
    }

    zcm = 3 + 6 * (utmz - 1) - 180;
    e0 = e / Math.sqrt(1 - e * e);
    esq = (1 - (b / a) * (b / a));
    e0sq = e * e / (1 - e * e);
    N = a / Math.sqrt(1 - Math.pow(e * Math.sin(phi), 2));
    T = Math.pow(Math.tan(phi), 2);
    C = e0sq * Math.pow(Math.cos(phi), 2);
    A = (lngd - zcm) * drad * Math.cos(phi);
    M = phi * (1 - esq * (1 / 4 + esq * (3 / 64 + 5 * esq / 256)));
    M = M - Math.sin(2 * phi) * (esq * (3 / 8 + esq * (3 / 32 + 45 * esq / 1024)));
    M = M + Math.sin(4 * phi) * (esq * esq * (15 / 256 + esq * 45 / 1024));
    M = M - Math.sin(6 * phi) * (esq * esq * esq * (35 / 3072));
    M = M * a;
    M0 = 0;

    x = k0 * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72 * C - 58 * e0sq) / 120));
    x = x + 500000;
    y = k0 * (M - M0 + N * Math.tan(phi) * (A * A * (1 / 2 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * e0sq) / 720))));

    if (y < 0) {
        y = 10000000 + y;
    }

    var xkordV = 10 * (x) / 10;
    var ykordV = 10 * y / 10;

    //Legger viapunkter pluss stedsnavn i array

    window.viakoordinater = xkordV + ',' + ykordV + ';';

    onearray = [viatext, xkordV, ykordV];
    viaarray.push(onearray);

    console.log(viaarray);

}

//Lager listeelementer for sorterbare viapunkter. Henter kun stedsnavn

i = 0;

function addListElement() {

    if (document.getElementById('text1').value == '') {
        document.getElementById('ErrorMessage').innerHTML = 'Du må fylle inn viapunkt';
        return false;
    }

    else {

        document.getElementById('ErrorMessage').innerHTML = '';

        i++;

        idText = "idList" + i;

        var numberList = document.getElementById("sortable");

        var newNumberListItem = document.createElement("li");

        newNumberListItem.setAttribute("id", idText);

        newNumberListItem.setAttribute("class", "ui-state-default");

        newNumberListItem.innerHTML = viatext;

        numberList.appendChild(newNumberListItem);

        document.getElementById('text1').value = '';

    }

}

//Funksjon for å sortere viapunkter på nøkkelelement

function putnewArray() {
    window.result = [];
    sorting = frase;

    sorting.forEach(function (key) {
        var found = false;
        viaarray = viaarray.filter(function (item) {
            if (!found && item[0] == key) {
                result.push(item);
                found = true;
                return false;
            } else
                return true;
        })
    })
   

}

//Konvertering av start og stopp. Hentes inn som parametere

function latlongToUTM1(y, x) {

    declarations();

    k0 = 0.9996;
    b = a * (1 - f);
    e = Math.sqrt(1 - (b / a) * (b / a));

    latd0 = parseFloat(x);
    lngd0 = parseFloat(y);

    lngd = lngd0;
    latd = latd0;

    phi = latd * drad;
    lng = lngd * drad;
    utmz = parseFloat(33);
    latz = 0;

    if (latd > -80 && latd < 72) {
        latz = Math.floor((latd + 80) / 8) + 2;
    }

    if (latd > 72 && latd < 84) {
        latz = 21;
    }

    if (latd > 84) {
        latz = 23;
    }

    zcm = 3 + 6 * (utmz - 1) - 180;
    e0 = e / Math.sqrt(1 - e * e);
    esq = (1 - (b / a) * (b / a));
    e0sq = e * e / (1 - e * e);
    N = a / Math.sqrt(1 - Math.pow(e * Math.sin(phi), 2));
    T = Math.pow(Math.tan(phi), 2);
    C = e0sq * Math.pow(Math.cos(phi), 2);
    A = (lngd - zcm) * drad * Math.cos(phi);
    M = phi * (1 - esq * (1 / 4 + esq * (3 / 64 + 5 * esq / 256)));
    M = M - Math.sin(2 * phi) * (esq * (3 / 8 + esq * (3 / 32 + 45 * esq / 1024)));
    M = M + Math.sin(4 * phi) * (esq * esq * (15 / 256 + esq * 45 / 1024));
    M = M - Math.sin(6 * phi) * (esq * esq * esq * (35 / 3072));
    M = M * a;
    M0 = 0;

    x = k0 * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72 * C - 58 * e0sq) / 120));
    x = x + 500000;

    y = k0 * (M - M0 + N * Math.tan(phi) * (A * A * (1 / 2 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * e0sq) / 720))));

    if (y < 0) {
        y = 10000000 + y;
    }

    xkoordinat = 10 * (x) / 10;
    ykoordinat = 10 * y / 10;

    var results = [xkoordinat, ykoordinat];
    return results;
}
//bygger objekt som skal sendes til serverside basert på om viapunkter er angitt eller ikke
function sendtoAjax(startConverted, stoppConverted) {

    saveValue();

    if (typeof (viakoordinater) == 'undefined') {


        var startt = [starttext, startConverted[0], startConverted[1]];
        var stoppt = [stopptext, stoppConverted[0], stoppConverted[1]];
        var viaall = null;

    }

    else {

        var startt = [starttext, startConverted[0], startConverted[1]];
        var stoppt = [stopptext, stoppConverted[0], stoppConverted[1]];
        var viaall = [];
        viaall = result;

    }

    openWebservice(startt, stoppt, viaall);


}
var Toll;
var Start;
var Stop;
var Via;
var Distance;
var Comment;
var Route;
var Vehicle;
//
function openWebservice(start, stopp, via) {

   
    var comment = document.getElementById('kommentar').value;

    var vehicleSelected;

    if (document.getElementById('r1').checked)
        vehicleSelected = 1;
    else if (document.getElementById('r2').checked)
        vehicleSelected = 2;
    else if (document.getElementById('r3').checked)
        vehicleSelected = 3;

    var jsonText = JSON.stringify({ Start: start, Stopp: stopp, Via: via, Vehicle: vehicleSelected, Comment: comment });
	//ajaxrequest mot Web Service
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "http://localhost:57366/WebService.asmx/PassToTravelRoute",
            
            data: jsonText,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                //Håndterer data returnert fra serverside
                    var drivingroute = "Start: " + msg.d.Start + "<br>Stopp: " + msg.d.Stopp;

                    if (msg.d.Via.length != 0) {
                        drivingroute = drivingroute + "<br>Via :<br>";
                        for (var i = 0; i < msg.d.Via.length; i++) {
                            drivingroute = drivingroute + msg.d.Via[i] + "<br>";
                        }
                    }

                    Toll = msg.d.ResultToImport.BarrierInfo.TotalCost;
                    Start = msg.d.ResultToImport.DirectionInfo.Start;
                    Stop = msg.d.ResultToImport.DirectionInfo.Stopp;
                    Via = msg.d.ResultToImport.DirectionInfo.Via;
                    Distance = msg.d.ResultToImport.DirectionInfo.Distance;
                    Comment = msg.d.ResultToImport.DirectionInfo.Comment;
                    Route = msg.d.ResultToImport.DirectionInfo.CompressedDirections
                    Vehicle = msg.d.ResultToImport.DirectionInfo.Vehicle

                    document.getElementById("out_takst").innerHTML = "0 Kr";
                    document.getElementById("out").innerHTML = drivingroute;
                    var distance = msg.d.Distance;
                    distance = Math.round(distance / 1000);
                    var bom = "";
                    document.getElementById("out2").innerHTML = "";
                    document.getElementById("out_km").innerHTML = distance + "km";
                    
					//gjør om tid fra minutter til timer og minutter
					var tid = parseInt(msg.d.Time);
                    var min = tid;
                    var timeDescription;

                    if (min > 60) {

                        var hoursDecimal = min / 60;
                        var hours = parseInt(hoursDecimal);
                        var remainingMin = (hoursDecimal - (parseFloat(hours))) * 60
                        timeDescription = hours + " Time(r) og " + Math.round(remainingMin) + " Minutt(er) ";
                    }
                    else {
                        timeDescription = min + " Minutter";
                    }
                    document.getElementById("out_time").innerHTML = timeDescription;
                    document.getElementById("comment").innerHTML = msg.d.ResultToImport.DirectionInfo.Comment;
                    var ferCount = msg.d.FerryCount;
                    if (ferCount != 0) {
                        document.getElementById("out_ferries").innerHTML = "Denne ruten inneholder fergeovergang(er), ekstra kostnader kan påløpe";
                    }
                    else {
                        document.getElementById("out_ferries").innerHTML = "";
                    }

					//skriver ut alle bomstasjoner med priser
                    if (msg.d.Barriers != []) {
                        for (var i = 0; i < msg.d.Barriers.length; i++) {

                            if (msg.d.Vehicle == 1) {
                                bom = "<br>Gratis"
                                var gratis = "Gratis bompassering";
                                document.getElementById("out_takst").innerHTML = gratis;
                            }
                            if (msg.d.Vehicle == 2) {
                                bom = bom + "<br>" + msg.d.Barriers[i].Name + ", pris: " + msg.d.Barriers[i].PriceSmallCar;
                                document.getElementById("out_takst").innerHTML = msg.d.TotalCostSmall + "kr";
                            }
                            if (msg.d.Vehicle == 3) {
                                bom = bom + "<br>" + msg.d.Barriers[i].Name + ", pris: " + msg.d.Barriers[i].PriceTruck;
                                document.getElementById("out_takst").innerHTML = msg.d.TotalCostLarge + "kr";
                            }

                            document.getElementById("out2").innerHTML = "<br>Bomstasjoner på ruta " + bom;
                        }
                    }
                    else {
                        document.getElementById("out2").innerHTML = "<br>Ingen bomstasjoner på ruta";
                        document.getElementById("out_takst").innerHTML = "0";
                    }

                    writeAllDir(msg.d.Directions.FullRoadDescription);
                    document.getElementById("CompRD").innerHTML = msg.d.Directions.CompressedRoadDescription;
					//skriver ruta ut på kart
                    plot(msg.d.Start, msg.d.Stopp, msg.d.Coordinates, distance);
                
            },
            error: function (request, error, errorThrown) {
                //håndterer eventuelle feil som oppstår på serveren
                alert("Kunne ikke beregne angitt rute");
                document.getElementById('overlay').style.visibility = "hidden";
                location.reload();
            },
        });
    });
}

//skriver ut fullstendig veibeskrivelse
function writeAllDir(dir) {
    var directions = "";
    console.log(dir[0].TextDescription[0].DescriptionText);
    for (var i = 0; i < dir.length; i++) {
        var tekst = "";
        for (var j = 0; j < dir[i].TextDescription.length; j++)
            tekst = tekst + dir[i].TextDescription[j].DescriptionText + ", ";

        directions = directions + "<br><b>" + dir[i].RoadNumber + "</b> <br> " + tekst + "<br>";
    }
    document.getElementById("allDir").innerHTML = "<br>Kjørebeskrivelse" + directions;

    cleanUp();
}
//nuller ut viapunkter
function cleanUp() {

    counter = 0;
    viaarray.length = 0;
    phrases.length = 0;
    result.length = 0;
    document.getElementById("sortable").innerHTML = '';
    document.getElementById('beregnknapp').disabled = false;
}

//viser hvilken informasjon som lagres i HRessurs
function Storage() {

	if(Vehicle == 1){
		Vehicle = "Mc";
	}
	else if(Vehicle == 2) {
		Vehicle = "Bil";
	}
	else {
		Vehicle = "Lastebil";
	}


    alert("Til HRessurs: \n\nStart: " + Start + "\n" + "Stopp: " + Stop + "\n" + "Via: " + Via + "\n" + "Km: " + Distance / 1000 + "\n" + "Bompenger: " + Toll +"Kr\n" + "Kommentar: " + Comment + "\n" + "Rute: " + Route + "\n" + "Kjøretøy:" + Vehicle);
}






