
function renderRegionManagement(regionId, dataSetName, mapName) {

    $.ajax(

        {
            url: "/Map/ManageRegion",
            data: "regionId=" + regionId + "&dataSetName=" + dataSetName + "&mapName=" + mapName,
            success: function (htmlToInsert) {

                $("#regionManagement").html(htmlToInsert);

            }
        }

    );

}


function dataPropertyClick(element, isStatistic) {



}

function regionOnClickHandler(clicked_id) {


    var dataSetName = document.getElementById("currentDataSet").innerText;

    var mapName = document.getElementById("currentMapName").innerText;

    var regionElement = document.getElementById(clicked_id);



    renderRegionManagement(clicked_id, dataSetName, mapName);


    $("[stroke=red]").each(function() {

        $(this).attr("stroke", "black");

    })

    if (regionElement.getAttribute("stroke") === "black") {

        regionElement.setAttribute("stroke", "red");

    }

    //if (regionElement.getAttribute("fill") === "white") {

    //    regionElement.setAttribute("fill", "red");

    //    return;
    //}

    //if (regionElement.getAttribute("fill") === "red") {

    //    regionElement.setAttribute("fill", "green");

    //    return;
    //}

    //if (regionElement.getAttribute("fill") === "green") {

    //    regionElement.setAttribute("fill", "blue");

    //    return;
    //}

    //if (regionElement.getAttribute("fill") === "blue") {

    //    regionElement.setAttribute("fill", "yellow");

    //    return;
    //}

    //if (regionElement.getAttribute("fill") === "yellow") {

    //    regionElement.setAttribute("fill", "white");

    //    return;
    //}

}


function RenderSaveMapLink() {


    //get svg element.
    var svg = document.getElementById("mapSvg");

    //get svg source.
    var serializer = new XMLSerializer();
    var source = serializer.serializeToString(svg);

    //add name spaces.
    if (!source.match(/^<svg[^>]+xmlns="http\:\/\/www\.w3\.org\/2000\/svg"/)) {
        source = source.replace(/^<svg/, '<svg xmlns="http://www.w3.org/2000/svg"');
    }
    if (!source.match(/^<svg[^>]+"http\:\/\/www\.w3\.org\/1999\/xlink"/)) {
        source = source.replace(/^<svg/, '<svg xmlns:xlink="http://www.w3.org/1999/xlink"');
    }

    //add xml declaration
    source = '<?xml version="1.0" standalone="no"?>\r\n' + source;

    //convert svg source to URI data scheme.
    var url = "data:image/svg+xml;charset=utf-8," + encodeURIComponent(source);

    //set url value to a element's href attribute.
    document.getElementById("saveMapButton").href = url;
//you can download svg file by right click menu.

}





