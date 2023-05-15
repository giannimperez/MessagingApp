import React, { useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function Register() {

    // user info
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [dob, setDob] = useState('');

    // send register POST
    const handleSubmit = (event) => {
        event.preventDefault();
        if (username != '' && password != '' && dob != '') {
            fetch('https://localhost:5001/api/accounts/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username: username, password: password, dateOfBirth: dob }),
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
            toast.error("Username, Password and DOB Required"); // Display error modal
        }
    };

    return (
            // Register form
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

export default Register;
