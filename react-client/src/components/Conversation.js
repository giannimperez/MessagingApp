import { useState, useEffect } from 'react';
import Sidebar from '../components/Sidebar';

function Conversation() {

    const [messageList, setMessageList] = useState([]);
    
    // users in conversation
    let user = JSON.parse(localStorage.getItem('user-info'));
    let otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:5001/api/messages/${user.username}/${otherUser}/conversation`, {
                    method: 'GET'
                });
                const data = await response.json();
                setMessageList(data);
                console.log(user.username);
            } catch (error) {
                console.error(error);
            }
        };

        fetchData();

        const interval = setInterval(fetchData, 2000);

        return () => clearInterval(interval);
    }, []);


    // TODO: change this, it's gross
    if (user) {
        return (
            <div className="messages-convo">
                {
                    messageList.length >= 1 ? messageList.map((message, index) => {
                        return (
                            <div key= { index } >
                                <div className="message">
                                    <p className="message-sender">{message.sender}</p>
                                    <p className="message-text"> {message.text} </p>
                                    <p className="message-date">{message.createDate}</p>
                                </div>
                            </div>
                            ) 
                    })
                        : ''
                }
            </div>
        )
    }

    return (
        <div>
            <h1> Messages</h1>
        </div>
    )

}

export default Conversation;