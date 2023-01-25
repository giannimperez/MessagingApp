import React, { useState } from 'react';

function Navbar() {

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
    );
}

export default Navbar;