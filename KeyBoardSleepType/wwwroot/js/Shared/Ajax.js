export function sendAjax(url, data, successCallback) {
    $.ajax({
        type: "POST",
        data,
        url,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: successCallback,
        error: function (xhr, status, error) {
            console.error("Ошибка отправки данных: " + error);
        }
    });
}