import React, { useState } from 'react';

function Navbar() {


    return (

        <nav className="nav">
            <a href="/" className="site-title">Messaging App</a>
            <ul>
                <li>
                    <a href="/">About</a>
                </li>
                {localStorage.getItem("user-info") &&
                    window.location.pathname !== "/login" ? (
                    <>
                        <li>
                            <a href="/friends">Friends</a>
                        </li>
                        <li>
                            <a href="/messages">Messages</a>
                        </li>
                    </>
                ) : null}
            </ul>
            <a href={localStorage.getItem("user-info") &&
                window.location.pathname !== "/login" ? "/login" : "/"}>
                {localStorage.getItem("user-info") &&
                    window.location.pathname !== "/login" ? "Logout" : "Login"}
            </a>
        </nav>
    );
/*
    return (
        if (localStorage.getItem("user-info") && window.location.pathname != "/login") {    // <- TODO: CONVERT TO TERNARY
            return (
                <nav className="nav">
                    <a href="/" className="site-title">Messaging App</a>
                    <ul>
                        <li>
                            <a href="/">About</a>
                        </li>
                        <li>
                            <a href="/friends">Friends</a>
                        </li>
                        <li>
                            <a href="/messages">Messages</a>
                        </li>
                    </ul>
                    <a href="/login">Logout</a>
                </nav>
            );
        }
        else {
            return (
                <nav className="nav">
                    <a href="/" className="site-title">Messaging App</a>
                    <ul>
                        <li>
                            <a href="/">About</a>
                        </li>
                    </ul>
                    <a href="/login">Login</a>
                </nav>
            );
        }
    );

    if (localStorage.getItem("user-info") && window.location.pathname != "/login") {
        return (
            <nav className="nav">
                <a href="/" className="site-title">Messaging App</a>
                <ul>
                    <li>
                        <a href="/">About</a>
                    </li>
                    <li>
                        <a href="/friends">Friends</a>
                    </li>
                    <li>
                        <a href="/messages">Messages</a>
                    </li>
                </ul>
                <a href="/login">Logout</a>
            </nav>
        );
    }

    return (
        <nav className="nav">
            <a href="/" className="site-title">Messaging App</a>
            <ul>
                <li>
                    <a href="/">About</a>
                </li>
            </ul>
            <a href="/login">Login</a>
        </nav>
    );*/
}

export default Navbar;