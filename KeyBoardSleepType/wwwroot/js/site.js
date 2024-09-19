// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {

    $('#inputData').on('input', function () {
        var data = $(this).val() || "";
        const length = data.length-1;

        if (data.trim() === "") {
            // Если data пустое, не отправляем запрос
            return;
        }

        $.ajax({
            type: "POST",
            data: {
                inputData: data.slice(-1),
                LengthWord: length
            },
            url: "https://localhost:7202/?handler=CheackLetter",

            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                // Обработка успешного ответа от сервера
                console.log("Данные отправлены успешно! " + result);
                if (result === "key_BackSpace") {
                    $('#inputData').removeClass('InputTextStile').addClass('InputTextStileError');
                    //$('#' + result[0]).addClass('BackLightKey');
                    //$('#' + result[1]).removeClass('BackLightKey');
                }
                else if (result !== "key_BackSpace") {
                    //$('#key_BackSpace').removeClass('BackLightKey');
                    $('#inputData').removeClass('InputTextStileError').addClass('InputTextStile');
                    //$('#' + result[0]).addClass('BackLightKey');
                    //$('#' + result[1]).removeClass('BackLightKey');
                }
                else if (result === 2) {
        
                }
            },
            error: function (xhr, status, error) {
                // Обработка ошибки
                console.log("Ошибка отправки данных: " + error);
            }
        });  
    });


    document.getElementById('inputData').addEventListener('keydown', function (event) {
        var LastSimbol = document.getElementById("CompareTextLine1");
        var data = $(this).val() || "";
        const length = data.length;
        const LengthCompareText = LastSimbol.textContent.length;

        if (data[length - 1] !== "." && event.key === 'Enter')
            event.preventDefault();

        else if (event.key === 'Enter' && length === LengthCompareText ||
            event.key === ' ' && length === LengthCompareText - 1 ||
            event.key === ' ' && length === LengthCompareText) {

            event.preventDefault(); // Предотвращает стандартное поведение Enter   
            document.getElementById("inputData").value = "";

            console.log('Enter key pressed in myInput field.');
            

            $.ajax({
                type: "POST",
                url: "https://localhost:7202/?handler=CheackEnter",

                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },

                success: function (result) {
                    document.getElementById("CompareTextLine1").innerText = result[0];
                    document.getElementById("CompareTextLine2").innerText = result[1];
                    document.getElementById("CompareTextLine3").innerText = result[2];
                    document.getElementById("CompareTextLine4").innerText = result[3];
                    document.getElementById("total_number_substring").innerText = "-  (" + length + ")";
                    //document.getElementById("total_Count_Error").innerText = "-  (" + result[] + ")";

                    $('#key_50').removeClass('BackLightKey');
                }
            });
        }
    });
});