import { startTimer } from '../Shared/Utils.js';
//import { checkWordLimit } from '../Shared/Utils.js';
import { sendAjax } from '../Shared/Ajax.js';


export function initInput(Page) {
    document.getElementById('inputData').addEventListener('keydown', function (event) {
        const text = this.value;
        const lastSimbols = document.getElementById("CompareTextLine1").textContent;
        const isValid = (event.key === ' ' && text.length === lastSimbols.length - 1) ||
            (event.key === ' ' && text.length === lastSimbols.length) ||
            (text.length === lastSimbols.length);

        if (event.key === 'Enter' || isValid) {
            event.preventDefault();
            document.getElementById('CommonWords').innerText = text.length;

            const errors = localStorage.getItem('ErrorCount');
            document.getElementById('ErrorCount').innerText = JSON.parse(errors);
            localStorage.setItem('ErrorCount', JSON.stringify(0));
            this.value = "";

            document.getElementById('ErrorPercent').innerText = (JSON.parse(errors) / text.length * 100).toFixed(2);

            sendAjax(`/?handler=CheackEnter`, { page: Page }, function (result) {
                for (let i = 1; i <= 4; i++) {
                    document.getElementById(`CompareTextLine${i}`).innerText = result[i - 1];
                }
                $('#key_50').removeClass('BackLightKey');
            });
        }
    });
}
