import React, { useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function Login() {

    // user info
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    // send login POST
    const handleSubmit = (event) => {
        event.preventDefault();
        if (username != '' && password != '') {
            fetch('https://localhost:5001/api/accounts/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username: username, password: password }),
            })
                .then((response) => {
                    if (!response.ok) { // Handle bad responses
                        return response.json().then((data) => {

                            toast.error(data.Message); // Display error modal
                            throw new Error(data.Message);
                        });
                    }
                    return response.json();
                })
                .then((data) => {
                    localStorage.setItem('user-info', JSON.stringify(data)); // Save user data locally
                    window.location.replace('messages'); 
                })
                .catch((error) => {
                    console.error(error);
                });
        }
        else {
            toast.error("Username and Password Required"); // Display error modal
        }
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
            <ToastContainer
                position="bottom-right"
                autoClose={1500}
                hideProgressBar
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable={false}
                pauseOnHover={false}
                theme="colored"
            />
        </form>
    );
}

export default Login;
