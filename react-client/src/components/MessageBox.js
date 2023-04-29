import React, { useState, useEffect, useRef } from 'react';

function MessageBox() {

    // messagebox text
    const [text, setText] = useState('');

    // users in conversation
    let userInfo = JSON.parse(localStorage.getItem('user-info'));
    let otherUser = JSON.parse(localStorage.getItem('current-conversation-user'));

    // send message POST
    const handleSubmit = (event) => {
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

            })
            .catch((error) => {
                console.error(error);
            });
    };

    // selects input every render
    const inputRef = useRef(null);
    useEffect(() => {
        inputRef.current.select();
    }, []);


    return (
        // MessageBox form
        <div className="message-box" >
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