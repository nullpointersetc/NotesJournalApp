async function loadNotes() {
    const response = await fetch('https://localhost:5120/api/notes');
    const notes = await response.json();

    const list = document.getElementById('notes');
    list.innerHTML = '';

    notes.forEach(n => {
        const li = document.createElement('li');
        li.textContent = n.title;
        list.appendChild(li);
    });
}

loadNotes();
