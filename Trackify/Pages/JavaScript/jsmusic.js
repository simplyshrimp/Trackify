function startAudio(base, offset = 0, volume = 1, paused = false) {
    var audio = document.querySelector("audio");
    audio.volume = volume
    audio.currentTime = offset
    if (audio.paused) {
        audio.autoplay = false
    }
    else {
        audio.autoplay = true
    }
    audio.addEventListener('load', function () {
        audio.play()
    }, true);
    audio.addEventListener('ended', function () {
        this.currentTime = 0
        this.play()
    }, false)

    //sets localStorage values

    localstorage.setItem("song", base)

    audio.addEventListener("volumechange")
    audio.addEventListener("pause")

    setInterval(() => {
        localstorage.setItem("audio_time", audio.currentTime);

        localStorage.setItem("audio_volume", audio.volume)
        localStorage.setItem("audio_paused", audio.paused)
    }, 100);

    audio.src = base

    // Example: Add custom play/pause buttons
    const playButton = document.getElementById("play-button");
    const pauseButton = document.getElementById("pause-button");

    playButton.addEventListener("click", () => {
        audio.play();
    });

    pauseButton.addEventListener("click", () => {
        audio.pause();
    });
}

if ("audio" in localStorage) {
    startAudio(
        localStorage.getItem("song"),
        localStorage.getItem("audio_time"),
        localstorage.getItem("audio_volume"),
        localStorage.getItem("audio_paused")
    );
}