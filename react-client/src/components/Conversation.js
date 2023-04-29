import { useState, useEffect } from 'react';

function Conversation() {

    // list of messages in conversation
    const [messageList, setMessageList] = useState([]);

    // users in conversation
    let userInfo = JSON.parse(localStorage.getItem('user-info'));
    let otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    // default message range
    let defaultMessageRange = 20;

    // number of messages to be displayed
    const [messageRange, setMessageRange] = useState(defaultMessageRange);

    // increase message range to load more messages
    const increaseMessageRange = () => {
        setMessageRange(messageRange + defaultMessageRange);
    }


    // send conversations GET every "interval" milliseconds
    // temporary implementation, should instead use SignalR
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:5001/api/messages/${otherUser}/${messageRange}/conversation`, {
                    method: 'GET',
                    headers: {
                        'Authorization': 'Bearer ' + userInfo.token
                    }
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
    }, [messageRange]);


        return (
            <div>
                {/*Only display name for the first few messages*/}
                {messageList.length < defaultMessageRange && <p className="conversation-user-title">{otherUser}</p>}
                {/*Only display load more button when at defaultMessageRange*/}
                {messageList.length >= messageRange && <button className="load-more-button" onClick={increaseMessageRange}>Load More</button>}
                {
                    // if list contains message(s), add to map
                    messageList.length >= 1 ? messageList.map((message, index) => {
                        {
                            // returns user sent messages, to be displayed on right
                            if (message.sender == userInfo.username) {
                                return (
                                    <div key={index} >
                                        <div className="message-from-user">
                                            <div className="message-bubble">
                                                <p className="message-text"> {message.text} </p>
                                            </div>
                                            <p className="message-date">{(new Date(message.createDate)).toLocaleTimeString()} {(new Date(message.createDate)).toDateString()}</p>
                                        </div>
                                    </div>
                                )
                            }
                            else { // returns other user messages, to be displayed on left
                                return (
                                    <div key={index} >
                                        <div className="message-from-other-user">
                                            <div className="message-bubble">
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