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
    const table = document.getElementById('notesList');
    var oldTBody = null;

    table.children.forEach(node1 => {
        if (node1.nodeType == Node.ELEMENT_NODE
            && node1.nodeName.toLowerCase() === 'tbody') {
            oldTBody = node1;
        }
    });

    const newTBody = document.createElement('tbody');

    notes.forEach(n => {
        const td1 = document.createElement('td');
        td1.textContent = n.title;

        const td2 = document.createElement('td');
        td2.textContent = n.body;

        const td3 = document.createElement('td');
        td3.textContent = n.createdAt ? new Date(n.createdAt).toLocaleString() : '';

        const td4 = document.createElement('td');
        td4.textContent = n.updatedAt ? new Date(n.updatedAt).toLocaleString() : '';

        const tr = document.createElement('tr');
        tr.appendChild(td1);
        tr.appendChild(td2);
        tr.appendChild(td3);
        tr.appendChild(td4);

        newTBody.appendChild(tr);
    });

    table.replaceChild(oldTBody, newTBody);
}

document.getElementById('logoutBtn')?.addEventListener('click', () => {
    logout();
    window.location.href = 'index.html';
})

loadNotes();
