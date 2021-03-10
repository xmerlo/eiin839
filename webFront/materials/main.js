document.getElementById("workingTest").innerHTML="It works";

var contracts;
var stations = []


function retrieveAllContracts() {
    var targetUrl = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + document.getElementById("apiKey").value;
    var requestType = "GET";

    var caller = new XMLHttpRequest();
    caller.open(requestType, targetUrl, true);
    caller.setRequestHeader ("Accept", "application/json");
    caller.onload=contractsRetrieved;

    caller.send();
}

function getStations() {

  console.log("getStations")

  var targetUrl = "https://api.jcdecaux.com/vls/v1/stations?contract=" + document.getElementById("contract-choice").value + "&apiKey=" + document.getElementById("apiKey").value;

  console.log(targetUrl)
  var requestType = "GET";

  var caller = new XMLHttpRequest();
  caller.open(requestType, targetUrl, true);
  caller.setRequestHeader ("Accept", "application/json");
  caller.onload=stationsRetrieved;

  caller.send();

  // forEach((response, s) => {
  //   stations.push(s)
  // });

}

function stationsRetrieved() {
  var response = JSON.parse(this.responseText);
  console.log(response)
}

function contractsRetrieved() {
    var response = JSON.parse(this.responseText);
    contracts = response;
    var label = document.createElement("label");
    label.innerHTML="Choose a contract : ";
    label.setAttribute("for", "contracts");

    var input = document.createElement("input");
    input.setAttribute("list", "contracts")
    input.setAttribute("id", "contract-choice")
    input.setAttribute("name", "contract-choice")

    var datalist = document.createElement("datalist")
    datalist.setAttribute("id", "contracts")

    response.forEach(contract => {



      var option = document.createElement("option")
      option.setAttribute("value", contract.name)
      datalist.appendChild(option)
    });

    var button = document.createElement("button")
    button.innerHTML = "get Stations"
    button.setAttribute("id", "retrieveStation")
    button.setAttribute("onclick", "getStations()")

    document.body.appendChild(label)
    document.body.appendChild(input)
    document.body.appendChild(datalist)
    document.body.appendChild(button)

    console.log(response);
}

function getDistanceFrom2GpsCoordinates(lat1, lon1, lat2, lon2) {
    // Radius of the earth in km
    var earthRadius = 6371;
    var dLat = deg2rad(lat2-lat1);
    var dLon = deg2rad(lon2-lon1);
    var a =
        Math.sin(dLat/2) * Math.sin(dLat/2) +
        Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
        Math.sin(dLon/2) * Math.sin(dLon/2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    var d = earthRadius * c; // Distance in km
    return d;
}

function deg2rad(deg) {
    return deg * (Math.PI/180)
}

function getClosestCity(lat, long, stations){
  var minDist= null;
  var minName = null;
  stations.forEach(station => {

  })
}
