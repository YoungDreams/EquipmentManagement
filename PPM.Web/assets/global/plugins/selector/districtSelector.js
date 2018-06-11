function districtSelector(obj, e, district) {
    var ths = obj;
    var dal = '<div class="_citys"><span title="关闭" id="cColse" >×</span><ul id="_citysheng" class="_citys0"><li class="citySel">省份</li><li>城市</li><li>区县</li></ul><div id="_citys0" class="_citys1"></div><div style="display:none" id="_citys1" class="_citys1"></div><div style="display:none" id="_citys2" class="_citys1"></div></div>';
    Iput.show({ id: ths, event: e, content: dal,width:"470"});
    $("#cColse").click(function () {
        Iput.colse();
    });
    $.get("/Common/GetProvinces", function (data) {
        var provinces = [];
        for (var i = 0, len = data.length; i < len; i++) {
            provinces.push('<a data-level="0" data-id="' + data[i]['Id'] + '" data-name="' + data[i]['Name'] + '">' + data[i]['Name'] + '</a>');
        }    

        $("#_citys0").append(provinces.join(""));
        $("#_citys0 a").click(function () {
            var $province = $(this);
            var provinceId = $province.data('id');
            $.get("/Common/GetCities?provinceId=" + provinceId, function (data) {
                var cities = [];
                for (var i = 0, len = data.length; i < len; i++) {
                    cities.push('<a data-level="1" data-id="' + data[i]['Id'] + '" data-name="' + data[i]['Name'] + '">' + data[i]['Name'] + '</a>');
                }
                $("#_citysheng li").removeClass("citySel");
                $("#_citysheng li:eq(1)").addClass("citySel");

                $("#_citys1 a").remove();
                $("#_citys1").append(cities);
                $("._citys1").hide();
                $("._citys1:eq(1)").show();
                $("#_citys0 a,#_citys1 a,#_citys2 a").removeClass("AreaS");
                $(this).addClass("AreaS");
                var lev = $province.data("name");
                ths.value = $province.data("name");
                if (document.getElementById("hcity") == null) {
                    var hcitys = $('<input>', {
                        type: 'hidden',
                        name: "hcity",
                        "data-id": $(this).data("id"),
                        id: "hcity",
                        val: lev
                    });
                    $(ths).after(hcitys);
                }
                else {
                    $("#hcity").val(lev);
                    $("#hcity").attr("data-id", $(this).data("id"));
                }

                $("#_citys1 a").click(function () {
                    $("#_citys1 a,#_citys2 a").removeClass("AreaS");
                    $(this).addClass("AreaS");
                    var lev = $(this).data("name");
                    if (document.getElementById("hproper") == null) {
                        var hcitys = $('<input>', {
                            type: 'hidden',
                            name: "hproper",
                            "data-id": $(this).data("id"),
                            id: "hproper",
                            val: lev
                        });
                        $(ths).after(hcitys);
                    }
                    else {
                        $("#hproper").attr("data-id", $(this).data("id"));
                        $("#hproper").val(lev);
                    }
                    var bc = $("#hcity").val();

                    var $city = $(this);
                    ths.value = bc + "-" + $city.data("name");

                    $.get("/Common/GetDistricts?cityId=" + $city.data("id"), function(data) {
                        var districts = [];
                        for (var i = 0, len = data.length; i < len; i++) {
                            districts.push('<a data-level="2" data-id="' + data[i]['Id'] + '" data-name="' + data[i]['Name'] + '">' + data[i]['Name'] + '</a>');
                        }
                        $("#_citysheng li").removeClass("citySel");
                        $("#_citysheng li:eq(2)").addClass("citySel");

                        $("#_citys2 a").remove();
                        $("#_citys2").append(districts);
                        $("._citys1").hide();
                        $("._citys1:eq(2)").show();

                        $("#_citys2 a").click(function () {
                            $("#_citys2 a").removeClass("AreaS");
                            $(this).addClass("AreaS");
                            var lev = $(this).data("name");
                            if (document.getElementById("harea") == null) {
                                var hcitys = $('<input>', {
                                    type: 'hidden',
                                    name: "harea",
                                    "data-id": $(this).data("id"),
                                    id: "harea",
                                    val: lev
                                });
                                $(ths).after(hcitys);
                            }
                            else {
                                $("#harea").val(lev);
                                $("#harea").attr("data-id", $(this).data("id"));
                            }
                            var bc = $("#hcity").val();
                            var bp = $("#hproper").val();
                            ths.value = bc + "-" + bp + "-" + $(this).data("name");
                            district.val($(this).data("id"));
                            Iput.colse();
                        });
                    });
                });
            });
            
        });

        $("#_citysheng li").click(function () {
            $("#_citysheng li").removeClass("citySel");
            $(this).addClass("citySel");
            var s = $("#_citysheng li").index(this);
            $("._citys1").hide();
            $("._citys1:eq(" + s + ")").show();
        });
    });
}