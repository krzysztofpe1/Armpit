async function fetchSystemMetrics() {
    try {
        const response = await fetch('api/Monitor', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const data = await response.json();
            console.log('Fetched data:', data); // Debug log
            updateMemoryGraph(data.memoryMetrics);
        } else {
            console.error('Failed to fetch system metrics');
        }
    } catch (error) {
        console.error('Error fetching system metrics:', error);
    }
}

const memoryData = {
    labels: [], // Empty labels, as we're not displaying time
    datasets: [{
        label: 'Used Memory (GB)',
        data: [],
        borderColor: 'rgba(75, 192, 192, 1)',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        fill: true,
        pointRadius: 3, // Visible points
        pointBackgroundColor: 'rgba(75, 192, 192, 1)'
    }]
};

const ctx = document.getElementById('memoryUsageChart').getContext('2d');
const memoryUsageChart = new Chart(ctx, {
    type: 'line',
    data: memoryData,
    options: {
        scales: {
            x: {
                display: false // Hide the x-axis
            },
            y: {
                beginAtZero: true,
                title: {
                    display: true,
                    text: 'Memory Usage (GB)'
                }
            }
        }
    }
});

function updateMemoryGraph(memoryMetrics) {
    console.log('Updating graph with:', memoryMetrics); // Debug log

    // Add new data point to the dataset
    memoryUsageChart.data.labels.push(''); // Push empty label
    memoryUsageChart.data.datasets[0].data.push(Math.round(memoryMetrics.used / 1024 * 100) / 100);

    // Keep only the last 10 data points
    if (memoryUsageChart.data.labels.length > 10) {
        memoryUsageChart.data.labels.shift();
        memoryUsageChart.data.datasets[0].data.shift();
    }

    // Update the chart
    memoryUsageChart.update();
    console.log('Graph updated'); // Debug log
}

setInterval(fetchSystemMetrics, 1000); // Fetch metrics every second
