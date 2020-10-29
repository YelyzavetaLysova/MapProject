function loadRegionProperties(regionId, dataSetId, mapName) {

    

    if (dataSetId !== "") {

        $.ajax(

            {
                url: "/Map/GetAllProperties",
                data: "regionId=" + regionId + "&dataSetName=" + dataSetId + "&mapName=" + mapName,
                success: function (htmlToInsert) {

                    $("#regionProperties").html(htmlToInsert);

                }
            }

        );
    }
}


function dataPropertyClick(element, isStatistic) {



}

function regionOnClickHandler(clicked_id) {


    var dataSetId = document.getElementById("currentDataSet").innerText;

    if (dataSetId === "") {


        alert("Please load dataset to be able to choose region and see details");
        return;

    }

    var mapName = document.getElementById("currentMapName").innerText;

    var element = document.getElementById(clicked_id);

    var currentRegionIdElement = document.getElementById("currentRegionId");

    if (currentRegionIdElement !== null) {

        currentRegionIdElement.innerText = clicked_id;

        $(".regionIdInput").val(clicked_id);


        //document.getElementById("regionEditText").hidden = true;

        document.getElementById("createNewPropertyForm").hidden = false;

        document.getElementById("regionIdLavel").innerText = "Current region - " + clicked_id;

        document.getElementById("createNewStatisticForm").hidden = false;

    }

    loadRegionProperties(clicked_id, dataSetId, mapName);


    if (element.getAttribute("fill") === "white") {

        element.setAttribute("fill", "red");

        return;
    }

    if (element.getAttribute("fill") === "red") {

        element.setAttribute("fill", "green");

        return;
    }

    if (element.getAttribute("fill") === "green") {

        element.setAttribute("fill", "blue");

        return;
    }

    if (element.getAttribute("fill") === "blue") {

        element.setAttribute("fill", "yellow");

        return;
    }

    if (element.getAttribute("fill") === "yellow") {

        element.setAttribute("fill", "white");

        return;
    }

    //element.setAttribute("fill", "red");
}


