import React, { useState } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Login() {
    // user info
    let [username, setUsername] = useState("");
    let [password, setPassword] = useState("");

    // authenticates and saves jwt to local storage
    async function login() {
        try {
            if (username != "" && password != "") {
                const response = await fetch(
                    "https://localhost:5001/api/accounts/login",
                    {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify({ username: username, password: password }),
                    }
                );

                // Handle bad API response
                if (!response.ok) {
                    const errorData = await response.json();
                    const errorMessage = errorData.Message;

                    toast.error(errorMessage); // display error modal
                    throw new Error(errorMessage);
                }

                // Save user data and redirect
                localStorage.setItem(
                    "user-info",
                    JSON.stringify(await response.json())
                );
                window.location.replace("messages");
            } else {
                toast.error("Username and Password Required"); // Display error modal
            }
        } catch (error) {
            console.error(error);
        }
    }

    // handle logic form submit
    const handleSubmit = (event) => {
        event.preventDefault(); // prevents rerenders from cancelling POST request
        login();
    };

    return (
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
