import React from 'react';
import Navbar from './components/Navbar';
import RegisterPage from './pages/RegisterPage';
import LoginPage from './pages/LoginPage';
import AboutPage from './pages/AboutPage';
import MessagesPage from './pages/MessagesPage';
import './App.css';


function App() {
        
    let Component;
    
    switch (window.location.pathname) {
        case "/":
            return window.location.href = "/messages"; // direct to messages page
            break;
        case "/register":
            Component = RegisterPage;
            break;
        case "/login":
            Component = LoginPage;
            break;
        case "/about":
            Component = AboutPage;
            break;
        case "/messages":
            Component = MessagesPage;
            break;
    }

    return (
        <div>
            <Navbar />
            <Component />
        </div>
        
    );
}

export default App;