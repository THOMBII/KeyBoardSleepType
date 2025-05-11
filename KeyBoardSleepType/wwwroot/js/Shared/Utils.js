export function startTimer(duration, onEnd) {
    let time = duration;
    const countdownElement = document.getElementById('countdown');
    const timer = setInterval(() => {
        if (time > 0) {
            countdownElement.textContent = `${time--}`;
        } else {
            clearInterval(timer);
            countdownElement.textContent = 'Конец 1-й минуты';
            if (onEnd) onEnd();
        }
    }, 1000);
}

export function countWords(text) {
    // Удаляем лишние пробелы, затем разбиваем по пробелам
    const words = text.trim().split(/\s+/);
    return words.filter(word => word.length > 0).length;
}

export function fillProgressBar(toPercent, duration = 1000) {
    const bar = document.getElementById("progressBar");
    bar.style.transition = `width ${duration}ms ease`;
    bar.style.width = toPercent + "%";
}