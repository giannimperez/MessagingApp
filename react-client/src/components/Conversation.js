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
                
            } catch (error) {
                console.error(error);
            }
        };

        fetchData();

        const interval = setInterval(fetchData, 2000);

        return () => clearInterval(interval);
    }, []);


        return (
            <div>
                <p className="conversation-user-title">{otherUser}</p>
                {
                    messageList.length >= 1 ? messageList.map((message, index) => {
                        {
                            if (message.sender == user.username) {
                                return (
                                    <div key={index} >
                                        <div className="message-from-user">
                                            <div className="message-bubble">
                                                {/*<p className="message-sender">{message.sender}</p>*/}
                                                <p className="message-text"> {message.text} </p>
                                            </div>
                                            <p className="message-date">{(new Date(message.createDate)).toLocaleTimeString()} {(new Date(message.createDate)).toDateString()}</p>
                                        </div>
                                    </div>
                                )
                            }
                            else {
                                return (
                                    <div key={index} >
                                        <div className="message-from-other-user">
                                            <div className="message-bubble">
                                                {/*<p className="message-sender">{message.sender}</p>*/}
                                                <p className="message-text"> {message.text} </p>
                                            </div>
                                            <p className="message-date">{(new Date(message.createDate)).toLocaleTimeString()} {(new Date(message.createDate)).toDateString()}</p>
                                        </div>
                                    </div>
                                )
                            }
                        }

                        
                    })
                        : ''
                }
            </div>
        )

}

export default Conversation;