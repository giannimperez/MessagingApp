import React, { useState, useEffect, useRef } from 'react';

function MessageBox() {

    const [text, setText] = useState('');

    // users in conversation
    let user = JSON.parse(localStorage.getItem('user-info')).username;
    let otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    // Posts the message
    const handleSubmit = (event) => {
        fetch('https://localhost:5001/api/messages', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ sender: user, recipient: otherUser, text: text }),
            })
            .then((response) => response.json())
            .then((data) => {

            })
            .catch((error) => {
                console.error(error);
            });
    };

    // Selects input every render
    const inputRef = useRef(null);
    useEffect(() => {
        inputRef.current.select();
    }, []);


    return (
        <div className = "message-box" >
            <form onSubmit = { handleSubmit } >
                <input
                    ref={inputRef}
                    className = "message-box-entry"
                    type = "text"
                    value={text}
                    placeholder="Write a message"
                    onChange = {(event) => setText(event.target.value)}
                />
                <input
                    className="message-box-button"
                    type = "submit"
                    value="Send"
                />
            </form> 
        </div >
    );
}

export default MessageBox;