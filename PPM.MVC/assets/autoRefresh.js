$.fn.autoRefresh = function (options) {
    var defaults = {

    };

    var opts = $.extend(defaults, options);

    return this.each(function () {
        var $this = $(this);
        var url = $this.attr("data-src");
        if (!url) {
            url = window.location.href;
        }
        $.get(url, function (html) {
            var newHtml = html;
            var id = $this.attr("id");
            if (id) {
                var newElement = $(html).find("#" + id);
                if (newElement.length > 0) {
                    newHtml = newElement.html();
                }
            }

            $this.html(newHtml).initControls();
        });
    });

};

var postCommand = function (url, data, element) {

    var errors;
    if (element) {
        errors = element.find(".errors");
        errors.find("p").remove();
        errors.hide();
    }

    var actionButton;
    if (element && element[0].tagName === "A") {
        if (!element.attr("data-command")) {
            actionButton = element;
            actionButton.attr("disabled", "disabled");
        }
    }

    var success = function (result) {
        if (result.success) {
            $.bootstrapGrowl("操作成功!", {
                type: "success",
                offset: {
                    from: "top",
                    amount: 150
                },
                align: "center"
            });

            //刷新列表或页面
            if (element && element.attr("data-refresh")) {
                $(element.attr("data-refresh")).autoRefresh();
                if (element.attr("data-refresh-tab")) {
                    $(element.attr("data-refresh-tab")).autoRefresh();
                }
                if (actionButton) {
                    actionButton.removeAttr("disabled");
                }
            }
            else if (element && element.attr("data-no-refresh")) {
                if (actionButton) {
                    actionButton.removeAttr("disabled");
                }
            }
            else {
                if (result.redirect) {
                    if (element && element.attr("target") && element.attr("target") === "_blank") {
                        window.open(result.redirect);
                        return;
                    }
                    if (window.location.href.indexOf("#") > 0) {
                        window.location.href = result.redirect;
                        window.location.reload();
                    } else {
                        window.location.href = result.redirect;
                    }
                } else {
                    // 临时解决方案
                    window.location.reload();
                }
            }

        } else {
            if (errors && errors.lengt>0) {
                $.each(result.errors, function (i, val) {
                    errors.append("<p>" + val + "</p>");
                });
                errors.show();
            } else {
                var errorMessage = "";
                $.each(result.errors, function (i, val) {
                    errorMessage += val + "<br/>";
                });

                $.bootstrapGrowl("操作失败!<br/>" + errorMessage, {
                    type: "warning",
                    offset: {
                        from: "top",
                        amount: 150
                    },
                    align: "center"
                });

                if (actionButton) {
                    actionButton.removeAttr("disabled");
                }
            }
        }
    };


    $.ajax({
        url: url,
        processData: false,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(data),
        accept: "application/json",

        success: success,
        error: function () {
            $.bootstrapGrowl("操作失败!", {
                type: "warning",
                offset: {
                    from: "top",
                    amount: 150
                },
                align: "center"
            });
        }
    });
};

$.fn.initControls = function (options) {
    return this.each(function () {
        var $this = $(this);

        // Init uniform
        if ($.fn.uniform) {
            $this.find("input:radio:not(.icheck),input:checkbox:not(.icheck)").uniform();
        }

        // Init ajax form
        $this.on('submit', 'form[data-ajax=true]', function (e) {

            var form = $(this);
            var action = form.attr("action");
            var button = form.find("button");
            button.attr("disabled", "disabled");
            if (!action) {
                action = window.location.herf;
            }
            var errors = form.find(".errors");
            errors.find("p").remove();
            errors.hide();

            if (form.attr("enctype") && form.attr("enctype") === "multipart/form-data") {

                e.preventDefault();

                $.ajax({
                    url: $(this).attr("action"),
                    type: 'POST',
                    dataType: 'json',
                    data: new FormData(form[0]),
                    processData: false,
                    contentType: false,
                    success: function (result) {
                        if (result.success) {
                            $.bootstrapGrowl("操作成功!", {
                                type: "success",
                                offset: {
                                    from: "top",
                                    amount: 150
                                },
                                align: "center"
                            });

                            //Close 弹出窗口
                            var modal = form.closest(".modal");
                            if (modal.length > 0) {
                                modal.modal("hide");
                            }
                            window.setTimeout(function() {
                                //刷新列表或页面
                                var refreshSelector = form.attr("data-refresh");
                                if (refreshSelector) {
                                    $(refreshSelector).autoRefresh();
                                    button.removeAttr("disabled");
                                } else {
                                    if (result.redirect) {
                                        window.location.href = result.redirect;
                                    } else {
                                        window.location.reload();
                                    }
                                }
                            }, 2000);
                            

                        } else {
                            $.each(result.errors, function (i, val) {
                                errors.append("<p>" + val + "</p>");
                            });
                            errors.show();
                            if ($(".modal-dialog")) {
                                $(".modal-dialog").unblock();
                            }
                            button.removeAttr("disabled");
                        }
                    },
                    error: function(e) {
                        alert(e);
                    }
                });
                
            } else {
                var json = form.serialize();

                console.log(json);

                $.post(action, json, function (result) {
                    if (result.success) {
                        $.bootstrapGrowl("操作成功!", {
                            type: "success",
                            offset: {
                                from: "top",
                                amount: 150
                            },
                            align: "center"
                        });
                        //Close 弹出窗口
                        var modal = form.closest(".modal");
                        if (modal.length > 0) {
                            modal.modal("hide");
                        }

                        //刷新列表或页面
                        var refreshSelector = form.attr("data-refresh");
                        if (refreshSelector) {
                            $(refreshSelector).autoRefresh();
                            button.removeAttr("disabled");
                        } else {
                            if (result.redirect) {
                                window.location.href = result.redirect;
                            } else {
                                window.location.reload();
                            }
                        }

                    } else {
                        $.each(result.errors, function (i, val) {
                            errors.append("<p>" + val + "</p>");
                        });
                        errors.show();
                        if ($(".modal-dialog")) {
                            $(".modal-dialog").unblock();
                        }
                        button.removeAttr("disabled");
                    }
                });
            }
            
            return false;
        });

        //Init data command
        $this.find("[data-command]").attr('data-href', "javascript:;");
        $this.find("[data-command]").each(function (index, obj) {
            var $confirm = $(obj);
            var message = $confirm.attr('data-confirmation-message');
            $confirm.on("click", function () {
                App.confirm(function() {
                    postCommand($confirm.data("command").Url, $confirm.data("command").Command, $confirm);
                }, function() {

                }, message);
            });
        });
        

        if (jQuery().datepicker) {
            $this.find('.date-picker').each(function () {
                var datepicker = $(this);
                var defaults = {
                    orientation: "left",
                    autoclose: true
                };
                var opts = datepicker.data("opts");
                var settings = $.extend({}, defaults, opts);
                datepicker.datepicker(settings);
            });

            $this.find('.month-picker').each(function () {
                var datepicker = $(this);
                var defaults = {
                    orientation: "left",
                    autoclose: true,
                    startView: "months",
                    minViewMode: "months",
                    format: "yyyy-mm"
                };
                var opts = datepicker.data("opts");
                var settings = $.extend({}, defaults, opts);
                datepicker.datepicker(settings);
            });
        }

        if (jQuery().timepicker) {
            $this.find('.timepicker').timepicker({
                minuteStep: 5,
                showMeridian: false,
            });
        }
    });
};