import React, { useState, useEffect } from 'react';

function Sidebar() {

    let userInfo = JSON.parse(localStorage.getItem('user-info'));
    let otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    // fetch users which have existing conversations with user
    const [convoUsers, setConvoUsers] = useState([]);

    // fetch conversation users list
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:5001/api/users/${userInfo.username}/conversationlist`, {
                    method: 'GET',
                    headers: {
                        'Authorization': 'Bearer ' + userInfo.token
                    }
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


    // search for users by partial username
    const [users, setUsers] = useState([]);
    const [query, setQuery] = useState('');

    // fetch filtered users list
    useEffect(() => {

        if (query !== '') {
            fetch(`https://localhost:5001/api/users/partialusername/${query}`, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + userInfo.token
                }
            })
                .then((response) => response.json())
                .then((data) => {
                    setUsers(data);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }, [query]);

    // selectUser button
    const handleClick = (username) => {
        localStorage.setItem('current-conversation-user', JSON.stringify(username));
        window.location.reload();
    };


        return (
            <nav className="sidebar">
                <p className="sidebar-title"> Direct Messages </p>
                {   // Returns users with existing conversations
                    convoUsers.length >= 1 ? convoUsers.map((user, index) => {
                        {
                            if (user.userName == otherUser) {
                                return (
                                    <div key={index} className="conversation-user">
                                        {user.isActive ? (
                                            <span className="active-status"></span>
                                        ) : ''
                                        }
                                        <button onClick={() => handleClick(user.userName)}>{user.userName}</button>
                                    </div>
                                )
                            }
                            else {
                                return (
                                    <div key={index} className="user">
                                        {user.isActive ? (
                                            <span className="active-status"></span>
                                        ) : ''
                                        }
                                        <button onClick={() => handleClick(user.userName)}>{user.userName}</button>
                                    </div>
                                )
                            }

                        }
                        
                    })
                        : ''
                }
                {/* user search bar*/}
                    <input
                        className="user-search-bar"
                        type="text"
                        placeholder={"Search users"}
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