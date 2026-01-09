function startAudio(base, offset = 0, volume = 1, paused = false) {
    var audio = new Audio()
    audio.volume = volume
    audio.currentTime = offset
    if (audio.paused) {
        audio.autoplay = false
    }
    else {
        audio.autoplay = true
    }


    //sets localStorage values

    localstorage.setItem("song", base)

    audio.addEventListener("volumechange")
    audio.addEventListener("pause")

    setInterval(() => {
        localstorage.setItem("audio_time", audio.currentTime);

        localStorage.setItem("audio_volume", audio.volume)
        localStorage.setItem("audio_paused", audio.paused)
    }, 100);
}

if ("audio" in localStorage) {
    startAudio(
        localStorage.getItem("song"),
        localStorage.getItem("audio_time"),
        localstorage.getItem("audio_volume"),
        localStorage.getItem("audio_paused")
}