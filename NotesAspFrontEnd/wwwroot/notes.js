async function loadNotes() {
    const restApiURL = window.appConfig.restApiURL;
    const token = localStorage.getItem('token');

    if (!token) {
        window.location.href = 'index.html';
        return;
    }
    const response = await fetch(`${restApiURL}/api/notes`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.status === 401) {
        window.location.href = 'index.html';
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
    window.location.href = 'index.html';
})

loadNotes();
