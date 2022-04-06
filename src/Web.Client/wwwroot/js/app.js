const getClientCoordinates = (dotnetObj, successMethodName, errorMethodName) => {
    if (navigator && navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const { longitude, latitude } = position.coords;
                dotnetObj.invokeMethodAsync(
                    successMethodName, longitude, latitude);
            },
            async (error) => {
                const { code, message } = error;
                // When the error is greater than 1 is a non-permissions error.
                if (code > 1) {
                    try {
                        const response = await fetch('https://ipinfo.io/?token=21a2f355f4c4b9');
                        const json = await response.json();
                        if (json && json.loc) {
                            const [latitude, longitude] = json.loc.split(',');
                            dotnetObj.invokeMethodAsync(
                                successMethodName, longitude, latitude);
                        }
                    } catch {
                        dotnetObj.invokeMethodAsync(
                            errorMethodName, code, message);
                    }
                } else {
                    dotnetObj.invokeMethodAsync(
                        errorMethodName, code, message);
                }
            });
    }
};

const getClientPrefersColorScheme = (color, dotnetObj, callbackMethodName) => {
    let media = window.matchMedia(`(prefers-color-scheme: ${color})`);
    if (media) {
        media.onchange = args => {
            dotnetObj.invokeMethodAsync(
                callbackMethodName,
                args.matches);
        };
    }

    return media.matches;
}

const getClientVoices = (dotnetObj, callbackMethodName) => {
    let voices = window.speechSynthesis.getVoices();
    if (!voices || !voices.length) {
        speechSynthesis.onvoiceschanged = () => {
            voices = window.speechSynthesis.getVoices();
            dotnetObj.invokeMethodAsync(
                callbackMethodName,
                JSON.stringify(voices.map(
                    voice => ({ Name: voice.name, Lang: voice.lang, Default: voice.default }))));
        }
    }

    return JSON.stringify(voices.map(v => ({ Name: v.name, Lang: v.lang, Default: v.default })));
}

const cancelPendingSpeech = () => {
    if (window.speechSynthesis && window.speechSynthesis.pending === true) {
        window.speechSynthesis.cancel();
    }
};

const speak = (dotnetObj, callbackMethodName, message, defaultVoice, voiceSpeed, lang) => {
    const utterance = new SpeechSynthesisUtterance(message);
    utterance.onend = e => {
        if (dotnetObj) {
            dotnetObj.invokeMethodAsync(callbackMethodName, e.elapsedTime)
        }
    };

    try {
        const voices = window.speechSynthesis.getVoices();
        utterance.voice =
            !!defaultVoice && defaultVoice !== 'Auto'
                ? voices.find(v => v.name === defaultVoice)
                : voices.find(v => !!lang &&
                    v.lang.startsWith(lang)) || voices[0];
    } catch { }
    utterance.volume = 1;
    utterance.rate = voiceSpeed || 1;

    window.speechSynthesis.speak(utterance);
};

const scrollIntoView = (selector) => {
    try {
        const element = document.querySelector(selector);
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    }
    catch (error) {
        console.log(error);
    }
};

// Coalescing assignment, if non-existent create window.app.
window.app = Object.assign({}, window.app, {
    getClientCoordinates,
    getClientVoices,
    getClientPrefersColorScheme,
    speak,
    scrollIntoView
});

// Prevent client from speaking when user closes tab or window.
window.addEventListener('beforeunload', _ => {
    cancelPendingSpeech();
});
