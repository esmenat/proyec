document.addEventListener("DOMContentLoaded", function () {

    // Chart: Descargas por Canción
    const descargasCtx = document.getElementById('descargasChart')?.getContext('2d');
    if (descargasCtx && window.descargaLabels && window.descargaData) {
        new Chart(descargasCtx, {
            type: 'bar',
            data: {
                labels: window.descargaLabels,
                datasets: [{
                    label: 'Número de Descargas',
                    data: window.descargaData,
                    backgroundColor: 'rgba(75, 192, 192, 0.3)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                plugins: {
                    legend: { display: false }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Descargas',
                            font: {
                                size: 14,
                                weight: 'bold'
                            }
                        }
                    }
                }
            }
        });
    }



    // Chart: Totales
    const totalesCtx = document.getElementById('totalesChart')?.getContext('2d');
    if (totalesCtx && window.totalData) {
        new Chart(totalesCtx, {
            type: 'bar',
            data: {
                labels: ['Canciones', 'Álbumes'],
                datasets: [{
                    data: window.totalData,
                    backgroundColor: ['#aaf', '#faa'],
                    borderColor: ['#88f', '#f88'],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: false } },
                scales: { y: { beginAtZero: true } }
            }
        });
    }

    // Chart: Reproducciones
    const repsCtx = document.getElementById('reproduccionesChart')?.getContext('2d');
    if (repsCtx && window.repsLabels && window.repsData) {
        new Chart(repsCtx, {
            type: 'pie',
            data: {
                labels: window.repsLabels,
                datasets: [{
                    label: 'Reproducciones',
                    data: window.repsData,
                    backgroundColor: [
                        '#FF6384', '#36A2EB', '#FFCE56',
                        '#4BC0C0', '#9966FF', '#FF9F40'
                    ],
                    borderColor: 'white',
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'bottom' },
                    tooltip: {
                        callbacks: {
                            label: function (ctx) {
                                return `${ctx.label}: ${ctx.raw} reproducciones`;
                            }
                        }
                    }
                }
            }
        });
    }
});
