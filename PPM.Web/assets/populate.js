$.fn.populate = function (data) {
    var frm = $(this);

    $.each(data, function (key, value) {
        var $ctrl = $('[name=' + key + ']', frm);
        switch ($ctrl.attr("type")) {
            case "text":
            case "email":
            case "number":
            case "tel":
            case "hidden":
                $ctrl.val(value);
                break;
            case "radio": case "checkbox":
                $ctrl.each(function () {
                    if ($(this).attr('value') === value) { $(this).attr("checked", value); }
                });
                break;
            default:
                $ctrl.val(value);
        }

        if ($ctrl.hasClass("date-picker")) {
            $ctrl.datepicker('setDate', new Date(value));
        }
    });
};