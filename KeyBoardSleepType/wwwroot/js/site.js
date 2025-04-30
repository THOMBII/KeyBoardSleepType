// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
const pageHandlers = {
    "WordsInput": "CheackEnterWords",
    "CountWordsInput": "CheackCountWords"
    //Регистрация страниц
};
let time = 60;
const pathParts = window.location.pathname.split('/').pop();
let CountWords = 0;
let CountWordsEnd = 50;


$(document).ready(function () {

    $('#inputData').on('input', function () {
        var data = $(this).val() || "";
        const length = data.length - 1;

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
                //if (data.slice(-1) === " ") {
                //    CountWordsEnd++;
                //}
                // Обработка успешного ответа от сервера
                console.log("Данные отправлены успешно! " + result[0]);
                if (result[0] === "key_BackSpace") {
                    $('#inputData').removeClass('InputTextStile').addClass('InputTextStileError');
                    $('#' + result[0]).addClass('BackLightKey');
                    $('#' + result[1]).removeClass('BackLightKey');
                  
                }
                else if (result[0] !== "key_BackSpace") {
                    $('#key_BackSpace').removeClass('BackLightKey');
                    $('#inputData').removeClass('InputTextStileError').addClass('InputTextStile');
                    $('#' + result[0]).addClass('BackLightKey');
                    $('#' + result[1]).removeClass('BackLightKey');
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

    if (pathParts === "WordsInput") {
        const timer = setInterval(() => {
            const countdownElement = document.getElementById('countdown'); // Последим за элементом отсчёта
            if (time > 0) {
                countdownElement.textContent = `${time--}`;

            } else {
                clearInterval(timer); // Заканчиваем работу таймера
                countdownElement.textContent = 'Конец 1-й минуты'; // Сообщение о старте загрузки
                location.reload();
            }
        }, 1000);
    }

    if (CountWords === CountWordsEnd) {
        CountWords = 0;
        location.reload();
    }

    document.getElementById('inputData').addEventListener('keydown', function (event) {
        var LastSimbol = document.getElementById("CompareTextLine1");
        var cheackError = document.getElementById("inputData");
        var data = $(this).val() || "";
        const length = data.length;
        const LengthCompareText = LastSimbol.textContent.length;

        if (event.key === 'Enter')
            event.preventDefault();

        else if (cheackError.className !== 'InputTextStileError' && (length === LengthCompareText ||
            event.key === ' ' && length === LengthCompareText - 1 ||
            event.key === ' ' && length === LengthCompareText)) {
            
            event.preventDefault(); // Предотвращает стандартное поведение Enter   
            document.getElementById("inputData").value = "";

            console.log('Enter key pressed in myInput field.');

            console.log(pathParts);

            $.ajax({
                type: "POST",
                data: {
                    page: pathParts
                },
                url: `https://localhost:7202/?handler=CheackEnter`,

                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },

                success: function (result) {
                    document.getElementById("CompareTextLine1").innerText = result[0];
                    document.getElementById("CompareTextLine2").innerText = result[1];
                    document.getElementById("CompareTextLine3").innerText = result[2];
                    document.getElementById("CompareTextLine4").innerText = result[3];
                    //document.getElementById("total_number_substring").innerText = "-  (" + length + ")"; //идикатор количества символов

                    $('#key_50').removeClass('BackLightKey');
                }
            });
        }
    });

    document.getElementById('inputData').addEventListener('keydown', function (event) {
        var data = $(this).val() || "";
        const length = data.length;

        console.log("Bakcspase result " + length);

        if (event.key === 'Backspace' && length !== 1) {
            $.ajax({
                type: "POST",
                data: {
                    inputData: data.slice(-1),
                    LengthWord: length
                },
                url: "https://localhost:7202/?handler=CheackBackSpace",

                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },

                success: function (result) {
                    console.log("Данные отправлены успешно Бекс! " + result[0]);
                    console.log("Данные отправлены успешно Бекс! " + result[1]);
                    //('#' + result[0]).addClass('BackLightKey');
                    $('#' + result[1]).removeClass('BackLightKey');
                }
            });
        }
    });
});

function changeTimer(Time) {
    time = Time;
}

function changeCountWord(_CountWordsEnd) {
    CountWordsEnd = _CountWordsEnd;
    const countWords = document.getElementById('countWords');
    countWords.textContent = `${CountWordsEnd}`;
}