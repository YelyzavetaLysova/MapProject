function loadRegionProperties(regionId, dataSetName, mapName) {



    if (dataSetName !== "") {

        $.ajax(

            {
                url: "/Map/ManageProperties",
                data: "regionId=" + regionId + "&dataSetName=" + dataSetName + "&mapName=" + mapName,
                success: function (htmlToInsert) {

                    $("#propertiesManagement").html(htmlToInsert);

                }
            }

        );
    }
}


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

    //loadRegionProperties(clicked_id, dataSetName, mapName);


    if (regionElement.getAttribute("fill") === "white") {

        regionElement.setAttribute("fill", "red");

        return;
    }

    if (regionElement.getAttribute("fill") === "red") {

        regionElement.setAttribute("fill", "green");

        return;
    }

    if (regionElement.getAttribute("fill") === "green") {

        regionElement.setAttribute("fill", "blue");

        return;
    }

    if (regionElement.getAttribute("fill") === "blue") {

        regionElement.setAttribute("fill", "yellow");

        return;
    }

    if (regionElement.getAttribute("fill") === "yellow") {

        regionElement.setAttribute("fill", "white");

        return;
    }

}


