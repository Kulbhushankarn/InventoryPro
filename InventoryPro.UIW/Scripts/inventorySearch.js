// Scripts/inventorySearch.js

// Global search
function globalSearch() {
    const input = document.getElementById('globalSearch').value.toLowerCase();
    const rows = document.querySelectorAll('#inventoryTable tbody tr');
    rows.forEach(row => {
        const text = row.textContent.toLowerCase();
        row.style.display = text.includes(input) ? '' : 'none';
    });
}

// Column-specific search
function columnSearch(columnIndex, event) {
    const input = event.target.value.toLowerCase();
    const rows = document.querySelectorAll('#inventoryTable tbody tr');
    rows.forEach(row => {
        const cell = row.cells[columnIndex];
        if (cell) {
            const text = cell.textContent.toLowerCase();
            row.style.display = text.includes(input) ? '' : 'none';
        }
    });
}
