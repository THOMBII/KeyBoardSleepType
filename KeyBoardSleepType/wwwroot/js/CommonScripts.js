var userLimits = {
    wordLimit: 50,
    timeLimit: 60,
};

function changeCountWordLimit(_CountWordsEnd) {
    // Пример объекта ограничений
    userLimits.wordLimit = _CountWordsEnd;
    localStorage.setItem('typingLimits', JSON.stringify(userLimits));

    CountWordsEnd = _CountWordsEnd;
    const countWords = document.getElementById('countWordsLim');
    countWords.textContent = `${CountWordsEnd}`;
}

function changeTimer(Time) {
    userLimits.timeLimit = Time;
    localStorage.setItem('typingLimits', JSON.stringify(userLimits));
}