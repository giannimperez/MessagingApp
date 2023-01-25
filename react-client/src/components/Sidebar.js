import React, { useState, useEffect } from 'react';

function Sidebar() {

    console.log("SIDEBAR RENDER");

    let user = JSON.parse(localStorage.getItem('user-info')).username;

    // fetch users which have existing conversations with user
    const [convoUsers, setConvoUsers] = useState([]);

    // fetch conversation users list
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:5001/api/users/${user}/conversationlist`, {
                    method: 'GET'
                });
                const data = await response.json();
                setConvoUsers(data);
            } catch (error) {
                console.error(error);
            }
        };
        fetchData();

        const interval = setInterval(fetchData, 4000);

        return () => clearInterval(interval);
    }, []);



/*    useEffect(() => {
        fetch(`https://localhost:5001/api/users/${user}/conversationlist`, {
            method: 'GET'
        })
            .then((response) => response.json())
            .then((data) => {
                setConvoUsers(data);
            })
            .catch((error) => {
                console.error(error);
            });
    }, []);*/


    // search for users by partial username
    const [users, setUsers] = useState([]);
    const [query, setQuery] = useState('');

    // fetch filtered users list
    useEffect(() => {
        fetch(`https://localhost:5001/api/users/partialusername/${query}`, {
            method: 'GET'
        })
            .then((response) => response.json())
            .then((data) => {
                setUsers(data);
            })
            .catch((error) => {
                console.error(error);
            });
    }, [query]);

    // selectUser button
    const handleClick = (username) => {
        localStorage.setItem('current-conversation-user', JSON.stringify(username));
        window.location.reload();
    };


        return (
            <nav className="sidebar">
                <h3> Conversations </h3>
                {   // Returns users with existing conversations
                    convoUsers.length >= 1 ? convoUsers.map((user, index) => {
                        return (
                            <div key={index} className="user">
                                <button onClick={() => handleClick(user.userName)}>{user.userName}</button>
                                {!user.isActive ? (
                                    <p>*</p>
                                ) : ''
                                }
                            </div>
                        )
                    })
                        : ''
                }
                {/* user search bar*/}
                    <input
                        className="user-search-bar"
                        type="text"
                        placeholder={"Search Users"}
                        value={query}
                        onChange={(event) => setQuery(event.target.value)}
                    />
                {   // Returns users filtered by partial username
                    users.length >= 1 ? users.map((user, index) => {
                        return (
                            <div key={index} className="user">
                                <button onClick={() => handleClick(user.userName)}>{user.userName}</button>
                                    {!user.isActive ? (
                                        <p>*</p>
                                        ) : ''
                                    }
                            </div>
                        )
                    })
                        : ''
                }
            </nav>
        );
}

export default Sidebar;




/*
    const [userList, setUserList] = useState([]);

    // fetch list of all users
    useEffect(() => {
        fetch(`https://localhost:5001/api/users`, {
            method: 'GET'
        })
            .then(response => response.json())
            .then(data => {
                setUserList(data);
            })
            .catch((error) => {
                console.error(error);
            });
    }, []);*/