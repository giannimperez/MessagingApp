import React, { useState } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Register() {
    // user info
    let [username, setUsername] = useState("");
    let [password, setPassword] = useState("");
    let [dob, setDob] = useState("");

    // creates a new user and saves jwt to local storage
    async function register() {
        try {
            if (username != "" && password != "" && dob != "") {
                const response = await fetch(
                    "https://localhost:5001/api/accounts/register",
                    {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify({
                            username: username,
                            password: password,
                            dateOfBirth: dob,
                        }),
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
                toast.error("Username, Password and DOB Required"); // Display error modal
            }
        } catch (error) {
            console.error(error);
        }
    }

    // handle register form submit
    const handleSubmit = (event) => {
        event.preventDefault();
        register();
    };

    return (
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
