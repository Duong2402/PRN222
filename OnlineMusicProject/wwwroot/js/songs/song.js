document.addEventListener('DOMContentLoaded', function () {
    const songsContainer = document.getElementById('songs-container');
    const autoSlideCheckbox = document.getElementById('auto-slide-checkbox');
    const audioPlayer = document.getElementById('audio-player');
    if (songsContainer && autoSlideCheckbox && audioPlayer) {
        const songs = Array.from(songsContainer.getElementsByClassName('song-container'));
        const songsPerPage = 6;
        let currentPage = 1;
        let currentSongIndex = 0;

        function showPage(page) {
            const startIndex = (page - 1) * songsPerPage;
            const endIndex = startIndex + songsPerPage;

            songs.forEach(song => song.style.display = 'none');

            const currentSongs = songs.slice(startIndex, endIndex);
            currentSongs.forEach(song => song.style.display = 'flex');
        }

        function playSong(index) {
            const song = songs[index];
            const filePath = song.getAttribute('data-file-path');
            var audioSource = document.getElementById('audioSource');

            if (audioSource) {
                audioSource.src = filePath;
                audioPlayer.load();
                audioPlayer.play().catch(error => {
                    console.error('Error playing audio:', error);
                });
            }
        }

        function prevSong() {
            if (currentSongIndex > 0) {
                currentSongIndex--;
                playSong(currentSongIndex);
                console.log('Chuyển đến bài hát trước:', songs[currentSongIndex].getAttribute('data-song-id'));
            }
        }

        function nextSong() {
            if (currentSongIndex < songs.length - 1) {
                currentSongIndex++;
                playSong(currentSongIndex);
                console.log('Chuyển đến bài hát tiếp theo:', songs[currentSongIndex].getAttribute('data-song-id'));
            }
        }

        audioPlayer.addEventListener('ended', function () {
            if (autoSlideCheckbox.checked) {
                nextSong();
            }
        });

        document.getElementById('next').addEventListener('click', nextSong);
        document.getElementById('prev').addEventListener('click', prevSong);

        showPage(currentPage);
        playSong(currentSongIndex);
    }
});