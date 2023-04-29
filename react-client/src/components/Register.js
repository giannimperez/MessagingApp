import React, { useState } from 'react';

function Register() {

    // user info
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [dob, setDob] = useState('');


    // TODO: change to register POST
    // send login POST
    const handleSubmit = (event) => {
        event.preventDefault();
        fetch('https://localhost:5001/api/accounts/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username: username, password: password, dateOfBirth: dob}),
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
            <h1>Create Account</h1>
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
            <label>
                Date of birth:
                <input
                    type="date"
                    value={dob}
                    onChange={(event) => setDob(event.target.value)}
                />
            </label>
            <input type="submit" value="Submit" />
            <a href="login">Already have an account?</a>
            </form>
    );
}

export default Register;
