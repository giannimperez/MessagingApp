import React, { useState, useEffect, useRef } from 'react';

function MessageBox() {
    const [text, setText] = useState('');

    const userInfo = JSON.parse(localStorage.getItem('user-info'));
    const otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    const handleSubmit = (event) => {
        event.preventDefault(); // Prevents rerenders from cancelling POST
        if (text != '') {
            fetch('https://localhost:5001/api/messages', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'bearer ' + userInfo.token
                },
                body: JSON.stringify({ recipient: otherUser, text: text }),
            })
                .then((response) => response.json())
                .then((data) => {
                    setText(''); // Reset textbox
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    };

    const inputRef = useRef(null);
    useEffect(() => {
        inputRef.current.select(); // Select textbox
    }, []);

    return (
        <div className="message-box">
            <form onSubmit={handleSubmit}>
                <input
                    ref={inputRef}
                    className="message-box-entry"
                    type="text"
                    value={text}
                    placeholder="Write a message"
                    onChange={(event) => setText(event.target.value)}
                />
                <input className="message-box-button" type="submit" value="Send" />
            </form>
        </div>
    );
}

export default MessageBox;