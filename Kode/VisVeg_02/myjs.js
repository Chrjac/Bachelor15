
/*
function addinpcss() {

    var startpunkt = document.getElementById('start');
    var endpunkt = document.getElementById('end');
    var leggtilknapp = document.getElementById('leggtilknapp');
    var beregnknapp = document.getElementById('beregnknapp');
    var inputcss = document.getElementById('sortabletwo');

     if ((startpunkt.value == '') || (endpunkt.value == '')) {
        document.getElementById('ErrorMessage').innerHTML = 'Fyll inn start og stopp';
        return false;

    }

     else {

         leggtilknapp.style.display = 'none';
         beregnknapp.style.visibility = 'visible';
         document.getElementById('ErrorMessage').innerHTML = '';
         inputcss.style.display = 'block';

    }
   
    autoComplete();

}

function autoComplete() {

    var inpen = document.getElementById('text1');
    var options = {

        types: ['geocode'],
        componentRestrictions: { country: 'nor' }

    };
    autocomplete = new google.maps.places.Autocomplete(inpen, options);

}*/

s = 0;
b = 1;
k = 0;
p = 0;
h = 0;
q = 0;


/*function dirList(k) {
    for (var p = 0; p < k.length ; p++) {


        s++;
        b++;


        var bom = "bom" + s;

        var ol = document.getElementById("hest");

        var li = document.createElement("li");

        var label = document.createElement("label");
        label.innerHTML = k[p][0] + " - (" + k[p][1].length +")";
        label.setAttribute("for", bom);

        var input = document.createElement("input");
        input.setAttribute("type", "checkbox");
        input.setAttribute("id", bom);
        var ol2 = document.createElement("ol");
        ol2.setAttribute("id", "aids");


        for (var d = 0; d < k[p][1].length; d++) {
            var li2 = document.createElement("li");

            li2.innerHTML = "- " + k[p][1][d];
            ol2.appendChild(li2);
        }

        li.appendChild(label);
        li.appendChild(input);
        li.appendChild(ol2);
        ol.appendChild(li);
        q++;

    }


}*/



