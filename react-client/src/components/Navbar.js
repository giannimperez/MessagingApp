import React, { useState } from "react";

function Navbar() {

    const user = localStorage.getItem("user-info");

    // clear user data if on login or register page
    if (window.location.pathname === "/login" || window.location.pathname === "/register") {
        localStorage.clear();
    }
    
    // if on messages page and not logged in,
    // send back to login page
    if (window.location.pathname === "/messages" && !user) {
        return window.location.href = "/login";
    }

    // show "not logged in" navbar
    if (!user) {
        return (
            <nav className="nav">
                <a href="/" className="site-title">
                    Messaging App
                </a>
                <ul>
                    <li>
                        <a href="/login">Login</a>
                    </li>
                </ul>
            </nav>
        );
    }

    // show "logged in" navbar
    if (user) {
        return (
            <nav className="nav">
                <a href="/" className="site-title">
                    Messaging App
                </a>
                <ul>
                    <li>
                        <a href="/about">About</a>
                    </li>
                    <li>
                        <a href="/messages">Messages</a>
                    </li>
                </ul>
                <ul>
                    <li>
                        <a href="/login">Logout</a>
                    </li>
                </ul>
            </nav>
        );
    }
}

export default Navbar;
