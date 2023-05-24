import React, { useState, useEffect } from "react";

function Sidebar() {
    // users in conversation
    const userInfo = JSON.parse(localStorage.getItem("user-info"));
    let otherUser = JSON.parse(localStorage.getItem("current-conversation-user"));

    // users with existing conversations
    let [convoUsers, setConvoUsers] = useState([]);

    // partial username search
    let [users, setUsers] = useState([]);
    let [query, setQuery] = useState("");

    // gets list of users with conversations and assigns convoUsers
    async function getConversationList() {
        try {
            const response = await fetch(
                `https://localhost:5001/api/users/conversationlist`,
                {
                    method: "GET",
                    headers: {
                        Authorization: "Bearer " + userInfo.token,
                    },
                }
            );

            setConvoUsers(await response.json());
        } catch (error) {
            console.error(error);
        }
    }

    // gets list of users by partial username
    async function queryUsernames() {
        try {
            if (query != "") {
                const response = await fetch(
                    `https://localhost:5001/api/users/partialusername/${query}`,
                    {
                        method: "GET",
                        headers: {
                            Authorization: "Bearer " + userInfo.token,
                        },
                    }
                );

                setUsers(await response.json());
            }
        } catch (error) {
            console.error(error);
        }
    }

    // request conversation list every "interval" milliseconds
    useEffect(() => {
        getConversationList(); // get conversation list on load

        const interval = setInterval(getConversationList, 4000); // get conversation list at interval

        return () => clearInterval(interval); // clean up
    }, []);

    // request usernames any time query changes
    useEffect(() => {
        queryUsernames();
    }, [query]);

    // selectUser button
    const handleClick = (username) => {
        localStorage.setItem("current-conversation-user", JSON.stringify(username));
        window.location.reload();
    };

    return (
        <nav className="sidebar">
            <p className="sidebar-title"> Direct Messages </p>
            {
                // Returns users with existing conversations
                convoUsers.length >= 1
                    ? convoUsers.map((user, index) => {
                        {
                            if (user.userName == otherUser) {
                                return (
                                    <div key={index} className="conversation-user">
                                        {user.isActive ? (
                                            <span className="active-status"></span>
                                        ) : (
                                            ""
                                        )}
                                        <button onClick={() => handleClick(user.userName)}>
                                            {user.userName}
                                        </button>
                                    </div>
                                );
                            } else {
                                return (
                                    <div key={index} className="user">
                                        {user.isActive ? (
                                            <span className="active-status"></span>
                                        ) : (
                                            ""
                                        )}
                                        <button onClick={() => handleClick(user.userName)}>
                                            {user.userName}
                                        </button>
                                    </div>
                                );
                            }
                        }
                    })
                    : ""
            }
            {/* user search bar*/}
            <input
                className="user-search-bar"
                type="text"
                placeholder={"Search users"}
                value={query}
                onChange={(event) => setQuery(event.target.value)}
            />
            {
                // Returns users filtered by partial username
                users.length >= 1
                    ? users.map((user, index) => {
                        return (
                            <div key={index} className="user">
                                <button onClick={() => handleClick(user.userName)}>
                                    {user.userName}
                                </button>
                                {!user.isActive ? <p>*</p> : ""}
                            </div>
                        );
                    })
                    : ""
            }
        </nav>
    );
}

export default Sidebar;
