async function loadNotes() {
    const token = localStorage.getItem('token');

    if (!token) {
        window.location.href = 'login.html';
        return;
    }
    const response = await fetch('https://localhost:5120/api/notes', {
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.status === 401) {
        window.location.href = 'login.html';
        return;
    }

    const notes = await response.json();
    const list = document.getElementById('notesList');

    list.innerHTML = '';

    notes.forEach(n => {
        const li = document.createElement('li');
        li.textContent = n.title;
        list.appendChild(li);
    });
}

document.getElementById('logoutBtn')?.addEventListener('click', () => {
    logout();
    window.location.href = 'login.html';
})

loadNotes();
