import { sendAjax } from './ajax.js';
import { countWords, startTimer, fillProgressBar } from './Utils.js';

localStorage.setItem('ErrorCount', JSON.stringify(0));
export function handleInputCheackLetter() {
    $('#inputData').on('input', function () {
        const data = $(this).val() || "";
        if (data.trim() === "") return;
        const length = data.length - 1;

        sendAjax("/?handler=CheackLetter", {
            inputData: data.slice(-1),
            LengthWord: length
        }, function (result) {
            const key = result[0];
            const prevKey = result[1];

            if (key === "key_BackSpace") {
                $('#inputData').removeClass('InputTextStile').addClass('InputTextStileError');
                $('#key_BackSpace').addClass('BackLightKey');
                                JSON.parse(localStorage.getItem('ErrorCount'));
                localStorage.setItem('ErrorCount', JSON.stringify(JSON.parse(localStorage.getItem('ErrorCount'))+1));
            } else {
                $('#inputData').removeClass('InputTextStileError').addClass('InputTextStile');
                $('#key_BackSpace').removeClass('BackLightKey');
            }
            fillProgressBar(length*2.2, 2000);
            $('#' + key).addClass('BackLightKey');
            $('#' + prevKey).removeClass('BackLightKey');
        });
    });
}

export function handleBackspaceCheack() {
    $('#inputData').on('keydown', function (event) {
        const data = $(this).val() || "";
        const length = data.length;

        if (event.key === 'Backspace' && length !== 1) {
            sendAjax("/?handler=CheackBackSpace", {
                inputData: data.slice(-1),
                LengthWord: length
            }, function (result) {
                $('#' + result[1]).removeClass('BackLightKey');
            });
        }
    });
}

export function handlerCountingWords() {
    $('#inputData').on('input', function () {
        const _text = $(this).val();
        const _wordCount = countWords(_text);
        const limitsJSON = localStorage.getItem('typingLimits');

        let limits = {
            wordLimit: 100, // значения по умолчанию
            timeLimit: 0,
        };

        if (limitsJSON) {
            limits = JSON.parse(limitsJSON);
        }

        sendAjax("/?handler=SaveWordCount", {
            wordCount: _wordCount,
            text: _text.slice(-1),
            wordLimiter: limits.wordLimit,
        }, function (result) {
            $('#wordCounter').text(`${_wordCount}`);
            if (result.status === "limit_reached") {
                location.reload(); // или показываем сообщение
            }
        });
    });
}

export function handlerTimer() {
    $('#inputData').on('input', function () {
            const limitsJSON = localStorage.getItem('typingLimits');

            let limits = {
                wordLimit: 0, // значения по умолчанию
                timeLimit: 60,
            };

            if (limitsJSON) {
                limits = JSON.parse(limitsJSON);
            }
            startTimer(limits.timeLimit, () => location.reload());
        
    });
}