window.playSound = function (soundfilepath) {
    const audio = new Audio(soundfilepath);
    audio.play();
}