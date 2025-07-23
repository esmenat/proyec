document.addEventListener('DOMContentLoaded', function () {
    const audio = document.querySelector('audio');
    let reproducido = false;

    audio.addEventListener('play', () => {
        if (reproducido) return;
        reproducido = true;

        const cancionId = document.getElementById("audioPlayer").dataset.id;

        fetch('https://localhost:7153/api/HistorialReproducciones', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                CancionId: cancionId
            })
        })
            .then(response => {
                if (!response.ok) {
                    console.error('❌ Error al registrar reproducción');
                } else {
                    console.log('✅ Reproducción registrada');
                }
            })
            .catch(error => console.error('Error:', error));
    });
});

// Para las descargas
document.addEventListener("DOMContentLoaded", () => {
    const btnDescargar = document.getElementById("btnDescargar");

    if (btnDescargar) {
        btnDescargar.addEventListener("click", async function (e) {
            const songId = btnDescargar.dataset.id;

            try {
                await fetch("https://localhost:7153/api/Descargas/registrar", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        id: self.crypto.randomUUID(),
                        cancionId: songId
                    })
                });

                console.log("Descarga registrada");
                // La descarga real ocurrirá porque es un <a href="..." download>
            } catch (error) {
                console.error("Error al registrar descarga:", error);
            }
        });
    }
});
