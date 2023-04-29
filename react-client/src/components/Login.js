﻿import React, { useState } from 'react';

function Login() {

    // user info
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    // send login POST
    const handleSubmit = (event) => {
        event.preventDefault();
        fetch('https://localhost:5001/api/accounts/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username: username, password: password }),
        })
            .then((response) => response.json())
            .then((data) => {
                localStorage.setItem('user-info', JSON.stringify(data));
                window.location.replace('messages');
            })
            .catch((error) => {
                console.error(error);
            });
    };

    return (
            // Login form
        <form className="login-register-form" onSubmit={handleSubmit}>
            <h1>Login</h1>
                <label>
                    Username:
                    <input
                        type="text"
                        value={username}
                        onChange={(event) => setUsername(event.target.value)}
                    />
                </label>
                <br />
                <label>
                    Password:
                    <input
                        type="password"
                        value={password}
                        onChange={(event) => setPassword(event.target.value)}
                    />
                </label>
                <br />
            <input type="submit" value="Submit" />
            <a href="register">Create account</a>
            </form>
    );
}

export default Login;
