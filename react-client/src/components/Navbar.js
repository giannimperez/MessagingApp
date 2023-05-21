import React, { useState } from "react";

function Navbar() {
    return (
        <nav className="nav">
            <a href="/" className="site-title">
                Messaging App
            </a>
            <ul>
                <li>
                    <a href="/">About</a>
                </li>
                {localStorage.getItem("user-info") &&
                    window.location.pathname !== "/login" ? (
                    <>
                        <li>
                            <a href="/messages">Messages</a>
                        </li>
                    </>
                ) : null}
            </ul>
            <a
                href={
                    localStorage.getItem("user-info") &&
                        window.location.pathname !== "/login"
                        ? "/login"
                        : "/"
                }
            >
                {localStorage.getItem("user-info") &&
                    window.location.pathname !== "/login"
                    ? "Logout"
                    : "Login"}
            </a>
        </nav>
    );
}

export default Navbar;
