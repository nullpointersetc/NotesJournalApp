async function login() {
    const response = await fatch('/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName: 'darren', password: 'password' })
    });

    const data = await response.json();
    localStorage.setItem('token', data.token);
}


async function loadNotes() {
    const token = localStorage.getItem('token');

    const response = await fetch('https://localhost:5120/api/notes', {
        headers: { 'Authorization': `Bearer ${token}` }
    });

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
