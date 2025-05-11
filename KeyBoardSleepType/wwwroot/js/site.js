import { handleInputCheackLetter, handleBackspaceCheack, handlerCountingWords, handlerTimer } from './Shared/InputHandler.js';
import { initInput } from './Pages/Input.js';
//import { initCountWordsInput } from './Pages/LeftPanel.js'; // если появится

document.addEventListener('DOMContentLoaded', function () {
    const Page = window.location.pathname.split('/').pop();

    handleInputCheackLetter();
    handleBackspaceCheack();

    if (Page === "CountWordsInput")
        handlerCountingWords();
    else if (Page === "WordsInput")
        handlerTimer();

    initInput(Page);

});
