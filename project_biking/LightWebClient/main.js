var userLatitude = 43.644132925978056;
var userLongitude = 1.4323214205002373;
var destinationLatitude = null;
var destinationLongitude = null;
var macarte = null;
var map;

function searchCoordinates() {
    destinationLatitude = document.getElementById("latitude").value.replace(',','.')
    destinationLongitude = document.getElementById("longitude").value.replace(',','.')

    var url = "http://localhost:8734/Design_Time_Addresses/RoutingWithBikes/Service1/rest/GetRestStationsAndItinary?lat="+userLatitude+"&lon="+userLongitude+"&lat2="+destinationLatitude+"&lon2="+destinationLongitude

    var requestType = "GET";
    var caller = new XMLHttpRequest();
    caller.open(requestType, url, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=manageResult;
    caller.send();
}

function searchAdress() {
    let adress = encodeURI(document.getElementById("adress").value)

    var url = "http://localhost:8734/Design_Time_Addresses/RoutingWithBikes/Service1/rest/GetRestStationsAndItinary?lat="+userLatitude+"&lon="+userLongitude+"&goalAdress="+adress

    var requestType = "GET";
    var caller = new XMLHttpRequest();
    caller.open(requestType, url, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=manageResult;
    caller.send();

}


function manageResult() {
    var response = JSON.parse(this.responseText);
    console.log(response);
    if (response.result == "Ok"){
      drawLine(response.startToS1.features[0].geometry.coordinates, "green");
      drawLine(response.s1ToS2.features[0].geometry.coordinates, "blue");
      drawLine(response.s2ToGoal.features[0].geometry.coordinates, "red");
    }
    else alert(response.result)

}

function getLocation() {
  let x = document.getElementById("geolocalisationError");
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(retrievePosition);
  } else {
    x.innerHTML = "Geolocation is not supported by this browser.";
  }


}

function retrievePosition(position) {
  let x = document.getElementById("geolocalisationError");
  // userLatitude = position.coords.latitude;
  // userLongitude = position.coords.longitude;
  initMap();
}

function initMap() {
    map = new ol.Map({
    target: 'map', // <-- This is the id of the div in which the map will be built.
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],

    view: new ol.View({
        center: ol.proj.fromLonLat([userLongitude, userLatitude]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 10 // You can adjust the default zoom.
    })

});
}

function drawLine(coords, color){
  // Create an array containing the GPS positions you want to draw
//var coords = [[7.0985774, 43.6365619], [7.1682519, 43.67163]];
var lineString = new ol.geom.LineString(coords);

// Transform to EPSG:3857
lineString.transform('EPSG:4326', 'EPSG:3857');

// Create the feature
var feature = new ol.Feature({
    geometry: lineString,
    name: 'Line'
});

// Configure the style of the line
var lineStyle = new ol.style.Style({
    stroke: new ol.style.Stroke({
        color: color,
        width: 10
    })
});

var source = new ol.source.Vector({
    features: [feature]
});

var vector = new ol.layer.Vector({
    source: source,
    style: [lineStyle]
});

map.addLayer(vector);
console.log("alo")
}
