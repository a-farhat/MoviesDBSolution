function SendAjaxPost(url, data, callback) {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    data.__RequestVerificationToken = token;

    var ajaxXHR = $.ajax({
        url: url,
        data: data,
        type: "POST"
    });

    $.when(ajaxXHR).then(function (data) {
        if (data !== null) {
            callback(data);
        }
    });
};