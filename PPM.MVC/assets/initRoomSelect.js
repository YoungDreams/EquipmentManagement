var initRoomSelect = function (contractId, isCompartmentSelector, buildingSelector, unitSelector, floorSelector, roomSelector, bedSelector, projectSelector, isAgreement) {
    var compartment = $(isCompartmentSelector);
    var building = $(buildingSelector);
    var unit = $(unitSelector);
    var floor = $(floorSelector);
    var room = $(roomSelector);
    var bed = $(bedSelector);
    var project = $(projectSelector);

    var compartmentIsChanged = false;
    compartment.change(function () {
        floor.trigger("change");
        compartmentIsChanged = true;
    });

    project.change(function() {
        building.empty();
        building.append("<option value=''>--未选择--</option>");
        if (!project.val()) {
            building.trigger("change");
            return;
        }

        $.get("/Contract/GetBuildings" + "?projectId=" + project.val(), function (result) {
            $.each(result, function (idx, data) {
                building.append("<option value='" + data.Value + "'>" + data.Text + "</option>");
            });

            if (building.attr("data-default-value")) {
                building.val(building.attr("data-default-value"))
                    .attr("data-default-value", "");
            }

            building.trigger("change");
        });
    });

    building.change(function () {
        unit.empty();
        unit.append("<option value=''>--未选择--</option>");
        if (!building.val()) {
            unit.trigger("change");
            return;
        }

        $.get("/Contract/GetUnits" + "?buildingId=" + building.val(), function (result) {
            $.each(result, function (idx, data) {
                unit.append("<option value='" + data.Value + "'>" + data.Text + "</option>");
            });

            if (unit.attr("data-default-value")) {
                unit.val(unit.attr("data-default-value"))
                    .attr("data-default-value", "");
            }

            unit.trigger("change");
        });
    });
    project.trigger("change");

    unit.change(function () {
        floor.empty();
        floor.append("<option value=''>--未选择--</option>");

        if (!unit.val()) {
            floor.trigger("change");
            return;
        }

        $.get("/Contract/GetFloors/?unitId=" + unit.val(),
            function (result) {
                $.each(result, function (idx, data) {
                    floor.append("<option value='" + data.Value + "'>" + data.Text + "</option>");
                });

                if (floor.attr("data-default-value")) {
                    floor.val(floor.attr("data-default-value"))
                         .attr("data-default-value", "");
                }

                floor.trigger("change");
            });
    });

    floor.change(function () {
        room.empty();
        if (floor.val() && !compartment.val()) {
            room.val("").trigger("change").append("<option value=''>--请选择是否包房--</option>");
            return;
        }

        room.append("<option value=''>--未选择--</option>");

        if (!floor.val()) {
            room.trigger("change");
            return;
        }

        $.get("/Room/GetRooms" + "?floorId=" + floor.val() + "&isFullRoom=" + compartment.val() + "&contractId=" + contractId + "&isAgreement=" + isAgreement,
            function (result) {
                $.each(result, function (idx, data) {
                    room.append("<option data-enabledTime='" + data.RoomCostDate + "' data-shortsermcost='" + data.ShortCost + "' data-longtermcost='" + data.LongCost + "' data-type='" + data.Type + "' value='" + data.Value + "'>" + data.Text + "</option>");
                });

                if (room.attr("data-default-value")) {
                    room.val(room.attr("data-default-value"))
                        .attr("data-default-value", "");
                }

                room.trigger("change");
            });
    });

    room.change(function () {
        //tempraory solution
        var shortTermRoomCost = $("input[name='ShortTermRoomCost']");
        var longTermRoomCost = $("input[name='LongTermRoomCost']");
        var shortRoomRate = $("input[name='ShortRoomRate']");
        var longRoomRate = $("input[name='LongRoomRate']");
       
        $("input[name='RoomCostDate']").val($(this).find("option[value='" + room.val() + "']").attr("data-enabledTime"));

        $("input[name='RoomType']").val($(this).find("option[value='" + room.val() + "']").attr("data-type"));
        if (shortTermRoomCost.attr("data-default-value")) {
            shortTermRoomCost.attr("data-default-value", "");
        } else {
            shortTermRoomCost.val($(this).find("option[value='" + room.val() + "']").attr("data-shortsermcost"));
        }
        if (longTermRoomCost.attr("data-default-value")) {
            longTermRoomCost.attr("data-default-value", "");
        } else {
            longTermRoomCost.val($(this).find("option[value='" + room.val() + "']").attr("data-longtermcost"));
        }

        $("input[name='NewRoomType']").val($(this).find("option[value='" + room.val() + "']").attr("data-type"));

        if (shortRoomRate.attr("data-default-value")) {
            shortRoomRate.attr("data-default-value", "");
        } else {
            shortRoomRate.val($(this).find("option[value='" + room.val() + "']").attr("data-shortsermcost"));
        }
        if (longRoomRate.attr("data-default-value") && !compartmentIsChanged) {
            longRoomRate.attr("data-default-value", "");
        } else {
            longRoomRate.val($(this).find("option[value='" + room.val() + "']").attr("data-longtermcost"));
        }

        if($("#LiquidatedDamages").length > 0) {
            changeLiquidatedDamages();    
        }
        

        bed.empty();
        bed.append("<option value=''>--未选择--</option>");

        if (!room.val()) {
            bed.trigger("change");
            return;
        }


        $.get("/Room/GetBeds" + "?roomId=" + room.val() + "&contractId=" + contractId,
            function (result) {
                $.each(result, function (idx, data) {
                    bed.append("<option value='" + data.Value + "'>" + data.Text + "</option>");
                });

                if (bed.attr("data-default-value")) {
                    bed.val(bed.attr("data-default-value"))
                       .attr("data-default-value", "");
                }
            });
    });
}