async function login(username, password) {
    const restApiURL = window.appConfig.restApiURL;

    const response = await fatch(`${restApiURL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName: username, password: password })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        return data.token;
    } else {
        return null;
    }
}

function logout() {
    localStorage.removeItem('token');
}

document.getElementById('loginForm')?.addEventListener('submit',
    async (e) => {
        e.preventDefault();

        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;
        const status = document.getElementById('loginStatus');
        status.textContent = "Please wait...";

        const token = await login(username, password);
        if (token) {
            status.textContent = "Login successful";
            window.location.href = "notes.html";
        } else {
            status.textContent = "Invalid username or password";
        }
    }
);
