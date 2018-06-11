$.fn.postCommand = function (url, command, options) {
    var defaults = {

    };

    var opts = $.extend(defaults, options);
    var $this = $(this);

    var errors = $this.find(".errors");
    errors.find("p").remove();
    errors.hide();



    $.ajax({
        url: url,
        processData: false,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(command),
        accept: "application/json",

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

                var refresh = true;

                if (opts.onSuccess) {
                    refresh = opts.onSuccess(result);
                }

                if (refresh) {
                    //刷新列表或页面
                    if ($this && $this.attr("data-refresh")) {
                        $($this.attr("data-refresh")).autoRefresh();
                        if ($this.attr("data-refresh-tab")) {
                            $($this.attr("data-refresh-tab")).autoRefresh();
                        }
                    } else {
                        if (result.redirect) {
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
                }
            } else {
                var showError = true;

                if (opts.onFail) {
                    showError = opts.onFail(result);
                }

                if (showError) {
                    if (errors) {
                        $.each(result.errors, function (i, val) {
                            errors.append("<p>" + val + "</p>");
                        });
                        errors.show();
                    } else {
                        $.bootstrapGrowl("操作失败!", {
                            type: "warning",
                            offset: {
                                from: "top",
                                amount: 150
                            },
                            align: "center"
                        });
                    }
                }
            }
        },
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