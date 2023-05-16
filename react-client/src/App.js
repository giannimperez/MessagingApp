import React from 'react';
import Navbar from './components/Navbar';
import Login from './components/Login';
import RegisterPage from './pages/RegisterPage';
import LoginPage from './pages/LoginPage';
import MessagesPage from './pages/MessagesPage';
import './App.css';


function App() {
        
    let Component;
    
    switch (window.location.pathname) {
        case "/":
            Component = MessagesPage;
            break;
        case "/register":
            Component = RegisterPage;
            break;
        case "/login":
            Component = LoginPage;
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