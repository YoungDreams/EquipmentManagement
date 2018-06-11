$(document).ajaxStart(
    function() {
        App.blockUI({ iconOnly: true });
    }).ajaxStop(
    function() {
        App.unblockUI();
    });

$(document).ready(function () {
    $(document).initControls();
});

var setFormValues = function (jsonData) {
    var tagName, type, arr;
    $.each(jsonData, function (name, value) {
        //name.replace(/(\w)/, function (v) { return v.toLowerCase() });
        var control = $("[name=" + name + "]");
        if (control == null || control.length === 0)
            return;

        tagName = control[0].tagName;
        type = control.attr('type');
        if (tagName === 'INPUT') {
            if (type === 'radio') {
                $(this).attr('checked', control.val() === value);
            } else if (type === 'checkbox') {
                arr = value.split(',');
                for (var i = 0; i < arr.length; i++) {
                    if (control.val() === arr[i]) {
                        control.attr('checked', true);
                        break;
                    }
                }
            } else {
                control.val(value);
            }
            //} else if (tagName === 'SELECT' || tagName === 'TEXTAREA') {
        } else if (tagName === 'SELECT' || tagName === 'TEXTAREA') {
            control.val(value);
        }
    });
}

var clearFormValues = function (form) {
    $(':input', '#' + form)
        .not(':button, :submit, :reset, input[type=hidden], select')
        .val('')
        .removeAttr('checked')
        .removeAttr('selected');
}

function jsonDateFormat(jsonDate) {
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        //var hours = date.getHours();
        //var minutes = date.getMinutes();
        //var seconds = date.getSeconds();
        //var milliseconds = date.getMilliseconds();
        //return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds + "." + milliseconds;
        return date.getFullYear() + "-" + month + "-" + day;
    } catch (ex) {
        return "";
    }
}

$(document).ready(function() {
    $("a,button").click(function () {
        if ($(this).attr("data-showloading") === "true") {
            App.blockUI({ iconOnly: true });
        }
    });
});